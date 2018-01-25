using Chainway.Library.MQ;
using SOAFramework.Library.DAL;
using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;
using System.IO;
using Chainway.Library.SimpleMapper;
using Chainway.SyncData.Pipe;

namespace Chainway.SyncData.BLL
{
    public class TableWatcher : BaseWatcher
    {
        #region attribute
        private IDBHelper _helper = DBFactory.CreateDBHelper();
        private ChainwayProducer _producer = null;
        #endregion

        #region contructor
        public TableWatcher(TableConfig config, ChainwayMQConfig mqconfig, ChainwayProducer producer)
            : base (config, mqconfig)
        {
            _producer = producer;
            _sender = producer;
            _helper.AutoCloseConnection = false;
        }
        #endregion

        #region public action
        public override void Open()
        {
            if (_producer == null)
            {
                _producer = MQFactory.CreateProducer();
                _producer.start();
            }
        }

        public override void SendData(Contract data, object sender)
        {
            IProxyHandler proxy = new MQProxyHandler(_producer);
            proxy.Send(data);
        }

        public override void Close()
        {
            if (_helper != null) _helper.CloseConnection();
            if (_producer != null) _producer.shutdown();
        }
        #endregion

        #region private action
        private void Producer_OnException(object sender, SendExceptionEvent args)
        {
            _logger.WriteException(args.Ex);
        }

        public override void DeleteSendData(Dictionary<string, object> data)
        {
            ISQLAction action = Factory.CreateAction(_tableconfig.Tag, _tableconfig, _helper);
            lock (locker)
            {
                action.Delete(data, null);
            }
        }
        #endregion
    }

    public class Condition
    {
        public List<WhereClause> Where { get; set; }
        public OrderByClause OrderBy { get; set; }
    }
}
