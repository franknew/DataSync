using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Chainway.Library.SimpleMapper;
using Chainway.SyncData.Pipe;
using SOAFramework.Library;
using System.Threading;
using Chainway.Library.MQ;

namespace Chainway.SyncData.BLL
{
    public class PipeWatcher : BaseWatcher
    {
        private PipeHandler _handler;

        public PipeWatcher(TableConfig config, PipeHandler handler)
            : base(config, null)
        {
            //_port = mqconfig.Port;
            //_host.OnPipeReceive += _host_OnPipeReceive;
            _handler = handler;
            _sender = handler;
        }

        public override void Close()
        {
            this.Stop = true;
            _handler?.Close();
        }

        public override void Open()
        {
        }

        public override void SendData(Contract data, object sender)
        {
            PipeHandler handler = sender as PipeHandler;
            handler.Send(data);
        }

        public override void HandleData(DataTable table, object sender)
        {
            if (table.Rows.Count > 0)
            {
                string tableName = _tableconfig.FormatedTableName;
                foreach (DataRow row in table.Rows)
                {
                    if (Stop) return;
                    var entity = row.ToDictionary();
                    entity["__Status"] = _tableconfig.Action;
                    string json = JsonHelper.Serialize(entity);
                    Contract data = new Contract
                    {
                        Data = json,
                        Command = MessageTypeEnum.Work,
                        Keys = tableName,
                    };
                    SendData(data, sender);

                    var response = _handler.Receive<Contract>(30);
                    if (response == null) throw new TimeoutException("接收代理回执超时");
                    if (response.Command == MessageTypeEnum.Error) throw new Exception(response.Message);
                    else if (response.Command == MessageTypeEnum.Ready) throw new ReadyException("proxy says ready, so let's get ready");
                    if (_tableconfig.DeleteData) DeleteSendData(entity);

                    _logger.Write(string.Format("表{1}成功推送数据到消息队列,json:{0}", json, _tableconfig.IDOrTableName));
                    RecordCondition(table, row);
                }
                _logger.Write(string.Format("表{1}成功推送{0}条数据到消息队列", table.Rows.Count, _tableconfig.IDOrTableName));
            }
        }
    }
}
