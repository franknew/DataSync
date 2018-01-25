
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SOAFramework.Library;
using System.Reflection;
using SOAFramework.Library.DAL;

namespace Chainway.Library.SimpleMapper
{
    public class InsertMapper : ISqlMapper
    {
        private SimpleLogger _logger = new SimpleLogger();

        public InsertMapper(ISQLConvert converter)
        {
            Converter = converter;
        }

        public ISQLConvert Converter { get; set; }

        public SqlModel ObjectToSql(string tableName, Dictionary<string, object> o, List<WhereClause> where, TableConfig config = null)
        {
            SqlModel model = new SqlModel();
            StringBuilder sql = new StringBuilder();
            StringBuilder prefix = new StringBuilder();
            StringBuilder values = new StringBuilder();

            List<Parameter> list = new List<Parameter>();
            prefix.AppendFormat("INSERT INTO {0} (", tableName);
            values.Append(" VALUES(");
            foreach (var key in o.Keys)
            {
                if (key.StartsWith("__")) continue;
                var column = config?.ColumnMapping?.Find(p => p.SourceColumn.ToLower().Equals(key.ToLower()));
                if (column != null && column.Ingore) continue;
                string columnName = Common.GetColumnName(key, column);
                if (string.IsNullOrEmpty(columnName)) continue;
                var value = o[key];
                if (column != null && column.GenerateID && value == null)
                {
                    IIDGenerator generator = IDGeneratorFactory.Create(GeneratorType.SnowFlak);
                    value = generator.Generate();
                }
                if (value == null)
                {
                    continue;
                }
                prefix.AppendFormat("{0},", Converter.FormatColumn(columnName));
                if (string.IsNullOrEmpty(column?.DataFrom)) values.AppendFormat("{0},", Converter.FormatParameter(columnName));
                else values.AppendFormat("({0}),", Converter.BuildTopN(column.DataFrom, 1));
                Parameter param;
                Type t = Common.GetType(column?.DataType, value.GetType(), value);
                param = new Parameter { Name = string.Format("{0}", Converter.FormatParameter(columnName)), Value = value.ChangeTypeTo(t), Type = t };
                list.Add(param);
            }
            //去掉最后一个逗号
            prefix.Remove(prefix.Length - 1, 1);
            values.Remove(values.Length - 1, 1);

            prefix.Append(")");
            values.Append(")");
            sql.Append(prefix.ToString()).Append(values.ToString());
            model.SQL = sql.ToString();
            model.Parameters = list;
            return model;
        }
    }
}