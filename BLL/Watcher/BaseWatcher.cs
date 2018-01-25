using Chainway.Library.MQ;
using Chainway.Library.SimpleMapper;
using Chainway.SyncData.Pipe;
using SOAFramework.Library;
using SOAFramework.Library.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Chainway.SyncData.BLL
{
    public abstract class BaseWatcher
    {
        #region attribute
        protected ChainwayMQConfig _mqConfig;
        protected TableConfig _tableconfig;
        protected int _topn = 1000;
        protected SimpleLogger _logger = new SimpleLogger();
        protected static object locker = new object();
        protected object _sender;
        #endregion

        #region property
        public bool Stop { get; set; }

        public bool Break { get; set; }
        #endregion

        #region contructor
        public BaseWatcher(TableConfig tableconfig, ChainwayMQConfig mqconfig)
        {
            _mqConfig = mqconfig;
            _tableconfig = tableconfig;
            if (mqconfig != null && mqconfig.Top > 0) _topn = mqconfig.Top;
        }
        #endregion

        #region abstract method
        public void Start()
        {
            Open();
            do
            {
                try
                {
                    var table = GetTopNData(_topn);
                    HandleData(table, _sender);
                }
                catch (ReadyException ex)
                {
                    throw ex;
                }
                catch (TimeoutException ex)
                {
                    _logger.WriteException(ex);
                    throw ex;
                }
                catch (Exception ex)
                {
                    _logger.Error("table:" + _tableconfig.FormatedTableName);
                    _logger.WriteException(ex);
                    Thread.Sleep(_tableconfig.Tick * 1000);
                }
            }
            while (!Break && !Stop);
        }

        public abstract void Open();

        public abstract void Close();

        public virtual void HandleData(DataTable table, object sender)
        {
            if (table.Rows.Count > 0)
            {
                string tableName = _tableconfig.FormatedTableName;
                foreach (DataRow row in table.Rows)
                {
                    if (Stop) break;
                    var entity = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(_tableconfig.UpdateColumns))
                    {
                        var columns = _tableconfig.UpdateColumns.Split(',');
                        foreach (var c in columns)
                        {
                            if (string.IsNullOrEmpty(c) || !table.Columns.Contains(c)) continue;
                            entity[c] = row[c];
                        }
                    }
                    else entity = row.ToDictionary();
                    entity["__Status"] = _tableconfig.Action;
                    string json = JsonHelper.Serialize(entity);
                    _mqConfig.Topic.ForEach(t =>
                    {
                        Contract data = new Contract
                        {
                            Data = json,
                            Tags = _tableconfig.Tag,
                            GroupName = _mqConfig.Group,
                            NameAddress = _mqConfig.Address,
                            Topic = t,
                            Command = MessageTypeEnum.Work,
                        };
                        if (string.IsNullOrEmpty(_tableconfig.Mapping)) data.Keys = tableName;
                        else data.Keys = _tableconfig.Mapping;
                        SendData(data, sender);
                    });
                    if (_tableconfig.DeleteData) DeleteSendData(entity);
                    RecordCondition(table, row);
                    if (_mqConfig.EnableLog) _logger.Log(string.Format("表{1}成功推送数据到消息队列,json:{0}", json, tableName));
                }
                _logger.Write(string.Format("表{1}成功推送{0}条数据到消息队列", table.Rows.Count, tableName));
            }
            //没有数据就休眠
            else
            {
                //_logger.Write(string.Format("表:{0}没有查询到数据", _tableType.Name));
                Thread.Sleep(_tableconfig.Tick * 1000);
            }
        }

        public abstract void SendData(Contract data, object sender);

        public virtual void DeleteSendData(Dictionary<string, object> entity)
        { }

        public virtual DataTable GetTopNData(int topN)
        {
            var condition = BuildSearchClause();
            var table = GetTopNData(topN, condition.Where, condition.OrderBy);
            return table;
        }
        #endregion

        #region private action

        /// <summary>
        /// 查询topn条数据
        /// </summary>
        /// <param name="topN"></param>
        /// <param name="entityType"></param>
        /// <param name="where"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        protected DataTable GetTopNData(int topN, List<WhereClause> where, OrderByClause orderby)
        {
            IDBHelper helper = DBFactory.CreateDBHelper();
            ISQLAction action = Factory.CreateAction(_tableconfig.Tag, _tableconfig, helper);
            var entity = Activator.CreateInstance<Dictionary<string, object>>();
            return action.SelectTopN(topN, entity, where, orderby);
        }

        /// <summary>
        /// 建造查询条件
        /// </summary> 
        /// <param name="config"></param>
        /// <returns></returns>
        protected Condition BuildSearchClause()
        {
            Condition condition = new Condition();
            List<WhereClause> list = new List<WhereClause>();
            if (string.IsNullOrEmpty(_tableconfig.CheckColumn)) return condition;
            //读取上次查询的结束点
            string path = AppDomain.CurrentDomain.BaseDirectory + @"Clause\";
            string fileName =  path + _tableconfig.IDOrTableName + ".txt";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            string dt = "";
            if (File.Exists(fileName))
            {
                dt = File.ReadAllText(fileName).Trim();
                //if (dt.IsDate()) dt = Convert.ToDateTime(dt).ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            else
            {
                dt = DateTime.Now.AddYears(-3).ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            list.Add(new WhereClause { ColumnName = _tableconfig.CheckColumn, Seperator = ">", Value = dt, });
            condition.Where = list;
            if (list.Count > 0) condition.OrderBy = new OrderByClause { ColumnName = _tableconfig.CheckColumn };
            return condition;
        }


        /// <summary>
        /// 记录查询条件
        /// </summary>
        /// <param name="config"></param>
        /// <param name="table"></param>
        protected void RecordCondition(DataTable table, DataRow row)
        {
            if (string.IsNullOrEmpty(_tableconfig.CheckColumn)) return;
            DateTime date = DateTime.Now;
            string checkColumn = _tableconfig.CheckColumn;
            if (checkColumn.IndexOf(".") > -1) checkColumn = checkColumn.Split('.')[1];
            string value = row[checkColumn].ToString();
            string path = AppDomain.CurrentDomain.BaseDirectory + @"Clause\";
            string fileName = path + _tableconfig.IDOrTableName + ".txt";
            if (table.Columns[checkColumn].DataType.Equals(typeof(DateTime))) value = ((DateTime)row[checkColumn]).ToString("yyyy-MM-dd HH:mm:ss.fff");
            else if (DateTime.TryParse(value, out date)) value = row[checkColumn].ChangeTypeTo<DateTime>().ToString("yyyy-MM-dd HH:mm:ss.fff");
            else value = row[checkColumn].ToString();
            lock (this)
            {
                File.WriteAllText(fileName, value);
            }
        }

        #endregion
    }
}
