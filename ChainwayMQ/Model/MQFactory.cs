
using org.apache.rocketmq.common.consumer;
using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chainway.Library.MQ
{
    public class MQFactory
    {
        private static List<ChainwayMQConfig> _consumerconfig;
        private static List<ChainwayMQConfig> _producerconfig;
        private static string _path = null;
        public const string consumerConfigfile = @"Config\consumerconfig.json";
        public const string producerConfigfile = @"Config\producerconfig.json";

        private static string _consumeconfigpath = AppDomain.CurrentDomain.BaseDirectory + consumerConfigfile;
        private static string _producerconfigpath = AppDomain.CurrentDomain.BaseDirectory + producerConfigfile;
        private static SimpleLogger _logger = new SimpleLogger();

        public static void LoadConfig(string path = null)
        {
            if (!string.IsNullOrEmpty(path))
            {
                _path = path;
                path = path.Trim('\\');
                Consumeconfigpath = path + "\\" + consumerConfigfile;
                Producerconfigpath = path + "\\" + producerConfigfile;
            }
            if (File.Exists(Consumeconfigpath)) _consumerconfig = JsonHelper.Deserialize<List<ChainwayMQConfig>>(File.ReadAllText(Consumeconfigpath));
            if (File.Exists(Producerconfigpath)) _producerconfig = JsonHelper.Deserialize<List<ChainwayMQConfig>>(File.ReadAllText(Producerconfigpath));
        }

        public static List<ChainwayMQConfig> Producerconfig
        {
            get
            {
                if (_producerconfig == null) LoadConfig(_path);
                return _producerconfig;
            }
        }
        public static List<ChainwayMQConfig> Consumerconfig
        {
            get
            {
                if (_consumerconfig == null) LoadConfig(_path);
                return _consumerconfig;
            }
        }

        public static string Consumeconfigpath { get => _consumeconfigpath; set => _consumeconfigpath = value; }
        public static string Producerconfigpath { get => _producerconfigpath; set => _producerconfigpath = value; }

        public static ChainwayProducer CreateProducer(string group, string nameAddress, int port)
        {
            if (string.IsNullOrEmpty(group)) throw new Exception("没有配置Group");
            if (string.IsNullOrEmpty(nameAddress)) throw new Exception("没有配置Address");
            ChainwayProducer producer = new ChainwayProducer(group);
            producer.setNamesrvAddr(nameAddress);
            if (port > 0) producer.setClientPort(port);
            return producer;
        }

        public static ChainwayProducer CreateProducer(ChainwayMQConfig config = null)
        {
            if (config == null) config = Producerconfig[0];
            return CreateProducer(config.Group, config.Address, config.Port);
        }

        public static List<ChainwayPushConsumer> CreatePushComsumer(string group, string nameAddress, List<string> topic, int port)
        {
            if (string.IsNullOrEmpty(group)) throw new Exception("没有配置Group");
            if (topic == null || topic.Count == 0) throw new Exception("没有配置Topic");
            if (string.IsNullOrEmpty(nameAddress)) throw new Exception("没有配置Address");
            List<ChainwayPushConsumer> consumers = new List<ChainwayPushConsumer>();
            topic.ForEach(t =>
            {
                ChainwayPushConsumer consumer = new ChainwayPushConsumer();
                consumer.setNamesrvAddr(nameAddress);
                consumer.subscribe(t, "*");
                consumer.setConsumerGroup(group);
                consumer.setConsumeFromWhere(ConsumeFromWhere.CONSUME_FROM_FIRST_OFFSET);
                if (port > 0) consumer.setClientPort(port);
                consumers.Add(consumer);
            });

            return consumers;
        }

        public static ChainwayPullConsumer CreatePullComsumer(string group, string nameAddress, int port)
        {
            if (string.IsNullOrEmpty(group)) throw new Exception("没有配置Group");
            if (string.IsNullOrEmpty(nameAddress)) throw new Exception("没有配置Address");
            ChainwayPullConsumer consumer = new ChainwayPullConsumer();
            consumer.setNamesrvAddr(nameAddress);
            consumer.setConsumerGroup(group);
            if (port > 0) consumer.setClientPort(port);

            return consumer;
        }

        public static List<ChainwayPushConsumer> CreatePushComsumer(ChainwayMQConfig config = null)
        {
            if (config == null) config = Consumerconfig[0];
            return CreatePushComsumer(config.Group, config.Address, config.Topic, config.Port);
        }
    }
}