using Chainway.Library.MQ;
using Chainway.Library.SimpleMapper;
using Chainway.SyncData.Pipe;
using SOAFramework.Library;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chainway.SyncData.BLL
{
    public class WatcherManager
    {
        private List<TableConfig> config;
        private ConcurrentDictionary<string, BaseWatcher> _tasks;
        private SimpleLogger _logger = new SimpleLogger();
        private ChainwayProducer _producer = null;
        private FileSystemWatcher _fileWatcher = new FileSystemWatcher();
        private PipeHost _host;

        public WatcherManager()
        {
            config = Factory.WatcherConfig;
            _tasks = new ConcurrentDictionary<string, BaseWatcher>();
            Common.CreateFileWatcher(new FileInfo(MQFactory.Producerconfigpath).Directory.FullName);
        }

        public void StartTableWatcher(ChainwayMQConfig mqconfig)
        {
            if (_producer == null)
            {
                _logger.Write("准备监听mq");
                _producer = MQFactory.CreateProducer();
                _producer.start();
                _logger.Write("mq监听完毕");
            }
            _logger.Write("准备进入循环");
            while (true)
            {
                foreach (var c in config)
                {
                    try
                    {
                        //替换表名中的日期占位符
                        if (_tasks.ContainsKey(c.IDOrTableName)) continue;
                        _logger.Write(string.Format("数据监视器正在启动，以监视表:{0}", c.IDOrTableName));
                        Task t = new Task(() =>
                        {
                            try
                            {
                                TableWatcher watcher = new TableWatcher(c, mqconfig, _producer);
                                _tasks[c.IDOrTableName] = watcher;
                                watcher.Start();
                            }
                            catch (Exception ex)
                            {
                                _logger.WriteException(ex);
                            }
                        });
                        t.Start();

                        _logger.Write(string.Format("数据监视器正在监视表:{0}", c.IDOrTableName));
                    }
                    catch (Exception ex)
                    {
                        _logger.WriteException(ex);
                    }
                }
                //休眠一小时
                Thread.Sleep(1000 * 60 * 60);
            }
        }

        public void StartPipeWatcher(int port)
        {
            if (_host == null)
            {
                _logger.Write("代理准备连接");
                while (true)
                {
                    if (!PortInUse(port))
                    {
                        _logger.Write("监测到监听没有启动，准备启动监听");
                        Task task = new Task(() =>
                        {
                            _host = new PipeHost(port);
                            _host.OnConnected += _host_OnConnected;
                            _host.Start();
                        });
                        task.Start();
                    }
                    //10秒钟监测一次
                    Thread.Sleep(1000 * 10);
                }
            }
        }

        private bool PortInUse(int port)
        {
            bool inUse = false;
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

            var connections = properties.GetActiveTcpListeners();
            foreach (IPEndPoint t in connections)
            {
                if (t.ToString().Equals("0.0.0.0:" + port.ToString()))
                {
                    inUse = true;
                    break;
                }
            }
            return inUse;
        }

        private void _host_OnConnected(object sender, ConnectedEventArgs args)
        {
            _logger.Write("代理已连接");
            while (true) 
            {
                try
                {
                    var client = args.Client;
                    //接受需要同步的数据
                    while (true)
                    {
                        Contract contract = client.Receive<Contract>();
                        switch (contract.Command)
                        {
                            case MessageTypeEnum.Work:
                                ConsumerManager manager = new ConsumerManager();
                                Contract response = new Contract();
                                if (manager.HandleMessage(contract.Tags, contract.Keys, contract.Data, true)) response.Command = MessageTypeEnum.OK;
                                else response.Command = MessageTypeEnum.Error;
                                client.Send(response);
                                break;
                        }
                        if (contract.Command == MessageTypeEnum.End) break;
                    }

                    List<Task> tasks = new List<Task>();
                    //发送需要同步的数据
                    foreach (var c in config)
                    {
                        Task t = new Task(() =>
                        {
                            try
                            {
                                PipeWatcher watcher = new PipeWatcher(c, client);
                                watcher.Break = true;
                                _tasks[c.IDOrTableName] = watcher;
                                watcher.Start();
                            }
                            catch (ReadyException ex)
                            {
                                _logger.Write("host get ready");
                                //StopPipeWatcher();
                            }
                            catch (Exception ex)
                            {
                                _logger.WriteException(ex);
                            }
                        });
                        t.Start();
                        tasks.Add(t);
                    }
                    Task.WaitAll(tasks.ToArray());
                    //发送结束消息
                    Contract end = new Contract { Command = MessageTypeEnum.End };
                    client.Send(end);
                }
                catch (Exception ex)
                {
                    _logger.WriteException(ex);
                }
            }
        }

        public void StopTableWatcher()
        {
            if (_producer != null) _producer.shutdown();
            foreach (var key in _tasks.Keys)
            {
                _tasks[key].Stop = true;
            }
            _tasks.Clear();
        }

        public void StopPipeWatcher()
        {
            _host.Close();
            foreach (var key in _tasks.Keys)
            {
                var task = _tasks[key] as PipeWatcher;
                task.Close();
            }
            _tasks.Clear();
        }
    }
}
