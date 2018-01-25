using Chainway.Library.MQ;
using SOAFramework.Library;
using SOAFramework.Library.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;

namespace Chainway.SyncData.BLL
{
    public class ErrorWatcher
    {
        private IDBHelper _helper = DBFactory.CreateDBHelper();
        private SimpleLogger _logger = new SimpleLogger();

        public bool IsStop { get; set; }

        public void Start()
        {
            MQFactory.LoadConfig();
            while (!IsStop)
            {
                try
                {
                    var table = GetTopNData(MQFactory.Producerconfig[0].Top);
                    if (table.Rows.Count > 0)
                    {
                        var list = table.ToList<T_Sync_Temp>();
                        list = (from l in list
                                orderby l.Address
                                select l).ToList();
                        string currentAddress = null;
                        ChainwayProducer producer = null;
                        foreach (var data in list)
                        {
                            if (string.IsNullOrEmpty(currentAddress)) currentAddress = data.Address;
                            if (!currentAddress.Equals(data.Address))
                            {
                                if (producer != null) producer.shutdown();
                                producer = new ChainwayProducer(data.Group);
                                producer.setNamesrvAddr(data.Address);
                                producer.start();
                            }
                            else if (producer == null)
                            {
                                producer = new ChainwayProducer(data.Group);
                                producer.setNamesrvAddr(data.Address);
                                producer.start();
                            }
                            var topics = data.Topic.Split(',');
                            foreach (var t in topics)
                            {
                                ChainwayMessage msg = new ChainwayMessage(t);
                                msg.Body = data.Data;
                                msg.setKeys(data.TableName);
                                msg.setTags(data.Tags);
                                producer.Send(msg);

                            }

                            currentAddress = data.Address;
                        }
                        if (producer != null) producer.shutdown();
                    }
                    else
                    {
                        Thread.Sleep(10 * 1000);
                    }
                }
                catch (Exception ex)
                {
                    _logger.WriteException(ex);
                    Thread.Sleep(10 * 1000);
                }
            }
        }

        public void Stop()
        {
            IsStop = true;
        }

        /// <summary>
        /// 查询topn条数据
        /// </summary>
        /// <param name="topN"></param>
        /// <param name="entityType"></param>
        /// <param name="where"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        private DataTable GetTopNData(int topN)
        {
            IDBHelper helper = DBFactory.CreateDBHelper();
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" SELECT TOP {0} * FROM T_Sync_Temp ", topN);
            return helper.GetTableWithSQL(sql.ToString());
        }

        private void Delete(int id)
        {

        }
    }
}
