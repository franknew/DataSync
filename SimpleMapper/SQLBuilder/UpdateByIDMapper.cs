
using SOAFramework.Library;
using SOAFramework.Library.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Chainway.Library.SimpleMapper
{
    public class UpdateByIDMapper : ISqlMapper
    {
        public UpdateByIDMapper(ISQLConvert converter)
        {
            Converter = converter;
        }

        public ISQLConvert Converter { get; set; }

        public SqlModel ObjectToSql(string tableName, Dictionary<string, object> o, List<WhereClause> where, TableConfig config = null)
        {
            SqlModel model = new SqlModel();
            StringBuilder prefix = new StringBuilder();
            StringBuilder whereBuilder = new StringBuilder();
            StringBuilder sql = new StringBuilder();
            List<Parameter> list = new List<Parameter>();
            if (where == null) where = new List<WhereClause>();

            prefix.AppendFormat("UPDATE {0} SET ", tableName);
            foreach (var key in o.Keys)
            {
                if (key.StartsWith("__")) continue;
                var column = config?.ColumnMapping?.Find(p => p.SourceColumn.ToLower().Equals(key.ToLower()));
                if (column != null && column.Ingore) continue;
                var value = o[key];
                if (column != null && column.GenerateID)
                {
                    IIDGenerator generator = IDGeneratorFactory.Create(GeneratorType.SnowFlak);
                    value = generator.Generate();
                }

                string columnName = Common.GetColumnName(key, column);
                if (string.IsNullOrEmpty(columnName)) continue;
                //生成配置在updatecolumn属性的列
                if (config != null && !string.IsNullOrEmpty(config.UpdateColumns))
                {
                    var updateColumnArr = config.UpdateColumns.Split(',');
                    if (!updateColumnArr.Any(p => p.ToLower().Equals(columnName.ToLower()))) continue;
                }
                //生成parameter
                Type t = null;
                if (value != null) t = Common.GetType(column?.DataType, value.GetType(), value);
                if (column != null && column.Primarykey)
                {
                    if (value == null) where.Add(new WhereClause { ColumnName = columnName, Seperator = "=", Value = DBNull.Value, DataType = typeof(DBNull) });
                    else where.Add(new WhereClause { ColumnName = columnName, Seperator = "=", Value = value.ChangeTypeTo(t), DataType = t });
                }
                else
                {
                    if (value == null) prefix.AppendFormat("{0}=NULL,", Converter.FormatColumn(columnName));
                    else
                    {
                        if (string.IsNullOrEmpty(column?.DataFrom)) prefix.AppendFormat("{0}={1},", Converter.FormatColumn(columnName), Converter.FormatParameter(columnName));
                        else prefix.AppendFormat("{0}=({1}),", Converter.FormatColumn(columnName), Converter.BuildTopN(column.DataFrom, 1));
                        list.Add(new Parameter { Name = Converter.FormatParameter(columnName), Value = value.ChangeTypeTo(t), Type = t });
                    }
                }
            }
            //去掉最后一个逗号
            prefix.Remove(prefix.Length - 1, 1);
            var whereModel = Common.BuildWhere(where, Converter);
            sql.Append(prefix);
            if (!string.IsNullOrEmpty(whereModel.SQL)) sql.Append(" WHERE ");
            sql.Append(whereModel.SQL);
            model.Parameters = Common.MergeParameter(list, whereModel.Parameters);
            model.SQL = sql.ToString();
            return model;
        }
    }
}