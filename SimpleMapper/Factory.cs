using SOAFramework.Library;
using SOAFramework.Library.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chainway.Library.SimpleMapper
{
    public class Factory
    {
        private static List<TableConfig> _watcherConfig;
        private static List<TableConfig> _syncDataConfig;

        const string watcherConfigFile = @"Config\watcherConfig.json";
        const string syncDataConfigFile = @"Config\syncDataConfig.json";

        private static string _watcherFileName = AppDomain.CurrentDomain.BaseDirectory + watcherConfigFile;
        private static string _syncDataFileName = AppDomain.CurrentDomain.BaseDirectory + syncDataConfigFile;

        public static List<TableConfig> WatcherConfig { get => _watcherConfig; set => _watcherConfig = value; }
        public static List<TableConfig> SyncDataConfig { get => _syncDataConfig; set => _syncDataConfig = value; }

        static Factory()
        {
            LoadConfig();
        }

        public static ISQLConvert CreateConverter(DBType type)
        {
            ISQLConvert c = null;
            switch (type)
            {
                case DBType.DB2:
                    c = new DB2Convert();
                    break;
                case DBType.MSSQL2005P:
                case DBType.MSSQL:
                    c = new T_SQLConvert();
                    break;
            }
            return c;
        }

        public static ISQLAction CreateAction(string key, TableConfig config, IDBHelper helper)
        {
            ISQLAction a = null;
            switch (helper.DBType)
            {
                case DBType.DB2:
                    a = new DB2Action(key, config, helper);
                    break;
                case DBType.MSSQL:
                case DBType.MSSQL2005P:
                    a = new MSSQLAction(key, config, helper);
                    break;
            }

            return a;
        }

        public static void LoadConfig()
        {
            if (File.Exists(_watcherFileName)) WatcherConfig = JsonHelper.Deserialize<List<TableConfig>>(File.ReadAllText(_watcherFileName));
            if (File.Exists(_syncDataFileName)) SyncDataConfig = JsonHelper.Deserialize<List<TableConfig>>(File.ReadAllText(_syncDataFileName));
        }
    }
}
