
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SOAFramework.Library;
using SOAFramework.Library.DAL;
using System.Diagnostics;
using System.Threading;
using SendStudyTime;
using System.Reflection;
using System.Data.OleDb;
using Chainway.SyncData.BLL;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Chainway.Library.SimpleMapper;
using System.IO;
using org.apache.rocketmq.client.consumer.listener;
using System.Net;
using System.Net.Sockets;
using Chainway.SyncData.Pipe;
using System.Management;
using MicroService.Library;
using System.Configuration;
using Chainway.Library.MQ;
using Chainway.ServiceMonitor.BLL;
using System.Net.NetworkInformation;

namespace WinformTest
{
    public partial class Form1 : Form
    {
        private Pipe _serverSocket;
        private SimpleLogger logger = new SimpleLogger();
        public Form1()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var arrs = args.Name.Split(',');
            string dll = arrs[0] + ".dll";
            return Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + dll);
        }

        private void btnConsume_Click(object sender, EventArgs e)
        {
            var consumer = MQFactory.CreatePushComsumer();
            consumer.ForEach(t =>
            {
                t.ConsumeMessage += Consumer_ConsumeMessage;
                t.start();
            });
            MessageBox.Show("listener started");
        }

        private ConsumeConcurrentlyStatus Consumer_ConsumeMessage(object obj, ConsumeEventArgs args)
        {
            StringBuilder builder = new StringBuilder();
            args.Messages.ForEach(t =>
            {
                builder.AppendLine(Encoding.UTF8.GetString(t.getBody()));
            });
            txbConsume.SetUIValue(builder.ToString(), "Text");
            //txbConsume.Text = builder.ToString();
            return ConsumeConcurrentlyStatus.CONSUME_SUCCESS;
        }

        private void btnProduce_Click(object sender, EventArgs e)
        {
            var producer = MQFactory.CreateProducer();
            producer.start();

            producer.send(new ChainwayMessage("syncdata-100-7") { Body = txbProduce.Text });
            producer.shutdown();
        }

        public delegate void SetValueDelegate(string value);

        private void SetValue(string value)
        {
            txbConsume.Text = value;
        }

        private void btnExec_Click(object sender, EventArgs e)
        {
            IDBHelper helper = DBFactory.CreateDBHelper("Database = Demo; uid=admin; pwd=frank;Server=localhost", DBType.MSSQL2005P);
            helper.ExecNoneQueryWithSQL(txbSQL.Text);
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            IDBHelper helper = DBFactory.CreateDBHelper("Database = SyncDemo; uid=admin; pwd=frank;Server=localhost", DBType.MSSQL2005P);
            //helper.AutoCloseConnection = false;
            SnowFlakGenerator g = new SnowFlakGenerator(Thread.CurrentThread.ManagedThreadId);
            ISqlMapper mapper = new InsertMapper(new DB2Convert());
            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < 10000; i++)
            {
                Dictionary<string, object> t = new Dictionary<string, object>();
                t["ID"] = g.Generate();
                t["Name"] = "hello world";
                t["Type"] = 3;

                var model = mapper.ObjectToSql(Chainway.Library.SimpleMapper.Common.GetTableName("201707", "dbo", t.GetType(), null, t), t, null);

                helper.ExecNoneQueryWithSQL(model.SQL, model.Parameters.ToArray());
            }
            watch.Stop();
            helper.CloseConnection();
            MessageBox.Show("time:" + watch.ElapsedMilliseconds.ToString());
        }

        private void btnLink_Click(object sender, EventArgs e)
        {
            string conn = "Provider=DB2OLEDB;User ID=db2admin;Password=chainway-db2@admin;Initial Catalog=gbnav;Network Transport Library=TCP;Host CCSID=1208;PC Code Page=936;Network Address=117.34.91.127;Network Port=50000;Package Collection=gbnav;Process Binary as Character=False;Units of Work=RUW;DBMS Platform=DB2/MVS;Defer Prepare=False;Rowset Cache Size=0;Persist Security Info=True;Connection Pooling=False;Derive Parameters=False;";
            //var ds = DB2Helper1.GetDataSet(CommandType.Text, txbSQL.Text);
            string conn2 = "Database=gzgbnav;UserID=db2admin; Password=chainway-db2;Server=14.17.70.141:50000";
            string conn4 = "Database=gbnav;UserID=db2admin; Password=chainway-db2@admin;Server=117.34.91.127:50000";
            string conn3 = "Provider=IBMDADB2.DB2COPY1;Data Source=gbnav;Persist Security Info=True;User ID=db2admin;Password=chainway-db2;Location=192.168.10.51:50000;";
            IDBHelper _helper = DBFactory.CreateDBHelper(conn3, DBType.DB2);
            var table = _helper.GetTableWithSQL(txbSQL.Text);
            //DB2Connection connection = new DB2Connection(conn2);
            //DB2Command com = new DB2Command(txbSQL.Text);
            //com.Connection = connection;
            //connection.Open();
            //com.ExecuteNonQuery();
            //connection.Close();
        }

        private void btnStartService_Click(object sender, EventArgs e)
        {
            //string localdb = "Database=Demo;uid=admin; pwd=frank;Server=localhost";
            //string localdb = "server=192.168.100.7;database=GZPT_Center;uid=sa;pwd=chainway-123";
            //string localdb = "Database=gbnav;UserID =db2admin; Password=chainway-db2;Server=192.168.100.51:50000";
            //string localdb = "Database =GBNAV; UserID =db2admin; Password =chainway-db2; Server =203.86.28.34:50123";

            ConsumerManager manager = new ConsumerManager();
            manager.StartConsumer(MQFactory.Consumerconfig[0]);
        }

        private void btnStartWatcher_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                WatcherManager manager = new WatcherManager();
                manager.StartTableWatcher(MQFactory.Producerconfig[0]);
            });
        }

        private void btnFormat_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 100000;i++)
            {
                logger.Write("this is the world hahahahahahahahahahaahahahahahahahahahahas;dflkjas;dkfja;sldkjf;hhaahahahahahahaahahahahaha:" + i.ToString());
            }
            var processes = Process.GetProcessesByName("java");
            var command = MonitorBusiness.GetCommandLine(processes[0]);

        }

        private void btnCommonjar_Click(object sender, EventArgs e)
        {
            var t = Task.Factory.StartNew(() =>
            {

                DirectoryInfo d = new DirectoryInfo(txbJar.Text);
                var files = d.GetFiles("*.jar");
                foreach (var file in files)
                {
                    if (file.Name.StartsWith("rocketmq")) continue;
                    Process p = new Process();
                    var name = file.Name.TrimEnd('.', 'j', 'a', 'r');
                    p.StartInfo = new ProcessStartInfo
                    {
                        FileName = @"D:\software\ikvm-7.2.4630.5\bin\ikvmc.exe",
                        Arguments = string.Format(@"-target:library {2} -out:{1}\{0}.dll", name, txbDll.Text.TrimEnd('\\'), file.FullName),
                        UseShellExecute = false,
                    };

                    p.Start();
                    p.WaitForExit();
                }
                MessageBox.Show("finished");
            });

            var form = Form.FromHandle(Process.GetCurrentProcess().MainWindowHandle);
        }

        private void btnRocketMQ_Click(object sender, EventArgs e)
        {
            string json = File.ReadAllText("jarDependency.json");
            DirectoryInfo directory = new DirectoryInfo(txbJar.Text);
            var files = directory.GetFiles("*.jar");
            List<Dependency> dep = JsonHelper.Deserialize<List<Dependency>>(json);
            StringBuilder cmd = new StringBuilder();
            List<string> fileNames = (from f in files
                                      select f.Name.TrimEnd('.', 'j', 'a', 'r')).ToList();
            var task = Task.Factory.StartNew(() =>
            {
                dep.ForEach(t =>
                {
                    //cmd.Clear();
                    var name = fileNames.FirstOrDefault(y => y.StartsWith(t.name));
                    var dllpath = txbDll.Text.TrimEnd('\\');
                    cmd.AppendFormat("ikvmc -target:library \"{2}\\{0}.jar\" -out:\"{1}\\{0}.dll\" ", name, dllpath, txbJar.Text.TrimEnd('\\'));
                    t.dependencies.ForEach(d =>
                    {
                        var dname = fileNames.FirstOrDefault(x => x.StartsWith(d));
                        cmd.AppendFormat(" -r:\"{0}\\{1}.dll\"", dllpath, dname);
                    });
                    Process p = new Process();
                    p.StartInfo = new ProcessStartInfo
                    {
                        FileName = @"D:\software\ikvm-7.2.4630.5\bin\ikvmc.exe",
                        Arguments = cmd.ToString(),
                        UseShellExecute = false,
                    };
                    cmd.AppendLine();
                    //p.Start();
                    //p.WaitForExit();
                });
                //MessageBox.Show("finished");
            });
            task.Wait();
            File.WriteAllText("RocketmqJarToDll.bat", cmd.ToString());
            txbConsume.Text = cmd.ToString();
        }

        private void btnError_Click(object sender, EventArgs e)
        {
            ErrorWatcher w = new ErrorWatcher();
            w.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse("183.60.136.195");
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Bind(new IPEndPoint(IPAddress.Any, Convert.ToInt32(txbSelfPort.Text)));
            try
            {
                clientSocket.Connect(new IPEndPoint(ip, Convert.ToInt32(txbPort.Text))); //配置服务器IP与端口  
                MessageBox.Show("连接成功");
                clientSocket.Disconnect(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            try
            {
                ChainwayProducer producer = new ChainwayProducer();
                producer.setProducerGroup("connectiontest");
                producer.setNamesrvAddr("183.60.136.195:9876");
                producer.setClientPort(Convert.ToInt32(txbSelfPort.Text));
                ChainwayMessage message = new ChainwayMessage("connectiontest");
                message.Body = "ok";
                producer.start();

                producer.Send(message);

                producer.shutdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Pipe _server;
        private void btnListen_Click(object sender, EventArgs e)
        {
            var end = new IPEndPoint(IPAddress.Any, Convert.ToInt32(txbSelfPort.Text));
            CreateServer(end);
            //_serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            MessageBox.Show("已监听端口：" + txbSelfPort.Text);
        }

        private void ListenClientConnect()
        {
            while (true)
            {
                var clientSocket = _server.Accept();
                //clientSocket.Send(Encoding.ASCII.GetBytes("  ok  "));
                Thread receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start(clientSocket);
            }
        }

        private static void ReceiveMessage(object clientSocket)
        {
            Pipe myClientSocket = (Pipe)clientSocket;
            while (true)
            {
                try
                {
                    byte[] result = new byte[1024];
                    //通过clientSocket接收数据  
                    myClientSocket.Receive(result);
                    myClientSocket.Send(Encoding.UTF8.GetBytes("     received        "));
                    MessageBox.Show(string.Format("接收客户端消息{0}", Encoding.ASCII.GetString(result)));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();
                    break;
                }
            }
        }

        private Pipe _client;

        private void btnSend_Click(object sender, EventArgs e)
        {
            byte[] result = new byte[1024];
            //int receiveLength = clientSocket.Receive(result);
            //MessageBox.Show("接收服务器消息：" + Encoding.ASCII.GetString(result, 0, receiveLength));
            ////通过 clientSocket 发送数据  


            //Thread th = new Thread(ReceiveMessageClient);
            //th.Start(clientSocket);
            try
            {
                //clientSocket.Send(Encoding.ASCII.GetBytes("hello world!!!!!!!!!"));
                //_client.Shutdown(SocketShutdown.Both);
                //_client.Disconnect();
                _client.Socket.Close();
                //_server.Socket.Close();
                var end = new IPEndPoint(IPAddress.Any, Convert.ToInt32(txbSelfPort.Text));
                var ip = new IPEndPoint(IPAddress.Parse(txbProduce.Text), Convert.ToInt32(txbSelfPort.Text));
                CreateClient(ip);
                _client.Socket.ReceiveTimeout = 5000;
                _client.Send(Encoding.ASCII.GetBytes(txbSQL.Text));
                _client.Receive();
                _client.Receive();
                //MessageBox.Show("向服务器发送消息：" + txbSQL.Text);
                //result = new byte[1024];
                //clientSocket.Receive(result);
                //MessageBox.Show("接受消息：" + Encoding.UTF8.GetString(result));
            }
            catch (Exception ex)
            {
                _client.Shutdown(SocketShutdown.Both);
                _client.Close();
            }
        }

        private void ClientSocket_OnReceiveMessage(byte[] data)
        {
            string json = Encoding.UTF8.GetString(data);
            MessageBox.Show("接受消息：" + json);

        }

        private static void ReceiveMessageClient(object clientSocket)
        {
            Pipe myClientSocket = (Pipe)clientSocket;
            while (true)
            {
                try
                {
                    byte[] result = new byte[1024];
                    //通过clientSocket接收数据  
                    myClientSocket.Receive(result);
                    string str = Encoding.ASCII.GetString(result);
                    MessageBox.Show(string.Format("接收服务端消息{0}", str));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();
                    break;
                }
            }
        }

        private void btnPipeProxy_Click(object sender, EventArgs e)
        {
            PipeProxy proxy = new PipeProxy();
            proxy.Start();
        }

        private void btnPipeWatcher_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                WatcherManager manager = new WatcherManager();
                manager.StartPipeWatcher(7172);
            });
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse(txbProduce.Text);
            //Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _client = new Pipe();
            try
            {
                _client.Connect(new IPEndPoint(ip, Convert.ToInt32(txbSelfPort.Text))); //配置服务器IP与端口  
                _client.OnReceiveMessage += ClientSocket_OnReceiveMessage;
                //_client.ReceiveSync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接服务器失败，请按回车键退出！");
                return;
            }
        }

        private Pipe CreateServer(IPEndPoint end)
        {
            _server = new Pipe();
            _server.Bind(end);
            _server.Listen(100);
            Thread myThread = new Thread(new ThreadStart(ListenClientConnect));
            myThread.Start();
            return _server;
        }

        private Pipe CreateClient(IPEndPoint end)
        {
            _client = new Pipe();
            _client.OnReceiveMessage += ClientSocket_OnReceiveMessage;
            //_client.ReceiveSync();

            _client.Connect(end);
            return _client;
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            MonitorBusiness bll = new MonitorBusiness();
            bll.StartWatcher();
        }

        private static string GetCommandLine(Process process)
        {
            var commandLine = new StringBuilder();

            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE ProcessId = " + process.Id))
            {
                foreach (var @object in searcher.Get())
                {
                    commandLine.Append(@object["CommandLine"]);
                    commandLine.Append(" ");
                }
            }

            return commandLine.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void btnStartMonitor_Click(object sender, EventArgs e)
        {
            MasterServer _server = new MasterServer();
            string displayShell = ConfigurationManager.AppSettings["DisplayShell"];
            _server.DisplayShell = displayShell == "1";
            Task.Factory.StartNew(() =>
            {
                _server.Start();
            });

            MonitorBusiness bll = new MonitorBusiness();
            bll.StartWatcher();
        }

        private void btnStartWatchService_Click(object sender, EventArgs e)
        {
            string id = txbConsume.Text;
            //WindowsServiceController bll = new WindowsServiceController();
            //bll.StartService("redis");
        }

        private void btnGetService_Click(object sender, EventArgs e)
        {
            WindowsServiceController controller = new WindowsServiceController();
            var list = controller.GetAllServices();
        }

        private void btnGetServiceInfo_Click(object sender, EventArgs e)
        {
            WindowsServiceController controller = new WindowsServiceController();
            var info = controller.GetServiceInfo("错误数据监视服务");
        }

        private void btndb2_Click(object sender, EventArgs e)
        {
            IDBHelper helper = DBFactory.CreateDBHelper();
            var table = helper.GetTableWithSQL(txbSQL.Text);
        }

        private void btnIpCheck_Click(object sender, EventArgs e)
        {

            bool inUse = false;
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

            var connections = properties.GetActiveTcpListeners();
            foreach (IPEndPoint t in connections)
            {
                if (t.ToString().Equals("0.0.0.0:9876"))
                {
                    inUse = true;
                    break;
                }
            }
        }

        private void btnAsync_Click(object sender, EventArgs e)
        {
            AsyncTest();
        }

        public async Task AsyncTest()
        {
            await Task.Factory.StartNew(()=>
            {
                Thread.Sleep(5000);
            });
        }
    }

    public class Img
    {
        public byte[] a { get; set; }

        public DateTime dt { get; set; }
    }
}
