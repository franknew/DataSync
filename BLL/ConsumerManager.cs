
using Chainway.Library.MQ;
using Chainway.Library.SimpleMapper;
using org.apache.rocketmq.client.consumer.listener;
using SOAFramework.Library;
using SOAFramework.Library.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Chainway.SyncData.BLL
{
    public class ConsumerManager
    {
        #region attribute
        private IDBHelper _helper = null;
        private int i = 0;
        private SimpleLogger _logger = new SimpleLogger();

        private List<ChainwayPushConsumer> _consumer;
        #endregion

        #region constructor
        public ConsumerManager(string connectionString, DBType type)
        {
            _helper = DBFactory.CreateDBHelper(connectionString, type);
            _helper.AutoCloseConnection = false;
            _helper.Lockable = true;
            Common.CreateFileWatcher(new FileInfo(MQFactory.Consumeconfigpath).Directory.FullName);
        }

        public ConsumerManager()
        {
            _helper = DBFactory.CreateDBHelper();
            _helper.AutoCloseConnection = false;
            _helper.Lockable = true;
            Common.CreateFileWatcher(new FileInfo(MQFactory.Consumeconfigpath).Directory.FullName);
        }

        public ConsumerManager(string groupName, string nameAddress, string topic, int port, bool enabledLog)
        {
            _helper = DBFactory.CreateDBHelper();
            _helper.AutoCloseConnection = false;
            _helper.Lockable = true;
        }
        #endregion

        #region public action
        public void StartConsumer(ChainwayMQConfig config)
        {
            _consumer = MQFactory.CreatePushComsumer(config);
            _consumer.ForEach(t =>
            {
                t.ConsumeMessage += Consumer_ConsumeMessage;
                _logger.Write(t.getNamesrvAddr());
                t.start();
            });
        }

        public bool HandleMessage(string tags, string tableName, string body, bool enableLog)
        {
            try
            {
                var config = Factory.SyncDataConfig.Find(p => p.OrginalTableName.ToLower().Equals(tableName.ToLower()));
                if (config == null) throw new Exception(string.Format("表名:{0}没有配置", tableName));
                if (config.Ignore) return true;
                if (string.IsNullOrEmpty(tags) || tags.Trim().Equals("null")) tags = "";
                var data = JsonHelper.Deserialize<Dictionary<string, object>>(body);
                ISQLAction action = Factory.CreateAction(tags, config, _helper);
                string status = "";
                if (data.ContainsKey("__Status") && data["__Status"] != null) status = data["__Status"].ToString();
                var effectCount = InvokeAction(action, status, data, null);
                if (enableLog) _logger.Debug(string.Format("同步了一条数据到数据库。table name:{0}-----json:{1}", tableName, body));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("table name:{0}-----json:{1}", tableName, body));
                _logger.WriteException(ex);
                return false;
            }
            return true;
        }

        public void StopConsumer()
        {
            _consumer.ForEach(t =>
            {
                t.shutdown();
            });
            _helper.CloseConnection();
        }
        #endregion

        #region private action
        private int InvokeAction(ISQLAction action, string status, Dictionary<string, object> o, List<WhereClause> where)
        {
            int result = -1;
            switch (status.ToLower())
            {
                case "insert":
                    result = action.Insert(o);
                    break;
                case "insertorupdate":
                    result = action.InsertOrUpdate(o);
                    break;
                case "update":
                    result = action.Update(o, where);
                    break;
                case "delete":
                    result = action.Delete(o, where);
                    break;
                default:
                    result = action.InsertOrUpdate(o);
                    break;
            }
            return result;
        }

        private ConsumeConcurrentlyStatus Consumer_ConsumeMessage(object obj, ConsumeEventArgs args)
        {
            //lock (this)
            //{
            foreach (var m in args.Messages)
            {
                string body = Encoding.UTF8.GetString(m.getBody());
                string tableName = m.getKeys();
                string tag = m.getTags();
                if (!HandleMessage(tag, tableName, body, MQFactory.Consumerconfig[0].EnableLog)) return ConsumeConcurrentlyStatus.RECONSUME_LATER;
            }
            //}
            return ConsumeConcurrentlyStatus.CONSUME_SUCCESS;
        }


        #endregion
    }
}