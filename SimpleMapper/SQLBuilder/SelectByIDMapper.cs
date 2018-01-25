using SOAFramework.Library;
using SOAFramework.Library.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Chainway.Library.SimpleMapper
{
    public class SelectByIDMapper : ISqlMapper
    {
        public ISQLConvert Converter { get; set; }

        public SelectByIDMapper(ISQLConvert converter)
        {
            Converter = converter;
        }

        public SqlModel ObjectToSql(string tableName, Dictionary<string, object> o, List<WhereClause> where, TableConfig config = null)
        {
            SqlModel model = new SqlModel();

            StringBuilder wherebuilder = new StringBuilder();
            StringBuilder sql = new StringBuilder();

            sql.Append(" SELECT * ");
            
            List<Parameter> list = new List<Parameter>();
            if (where == null) where = new List<WhereClause>();

            var columns = config?.ColumnMapping?.FindAll(t => !t.Ingore && t.Primarykey);
            foreach (var column in columns)
            {
                string columnName = Common.GetColumnName(column.SourceColumn, column);
                if (string.IsNullOrEmpty(columnName)) continue;
                object value = o[column.SourceColumn];
                if (value == null) where.Add(new WhereClause { ColumnName = columnName, Seperator = "=", Value = DBNull.Value });
                else where.Add(new WhereClause { ColumnName = columnName, Seperator = "=", Value = value, DataType = Common.GetType(column?.DataType, value.GetType(), value) });
            }

            sql.Remove(sql.Length - 1, 1);
            sql.AppendFormat(" FROM {0} ", tableName);
            if (where.Count > 0) sql.Append(" WHERE ");
            var whereModel = Common.BuildWhere(where, Converter);

            sql.Append(whereModel.SQL);

            model.SQL = Converter.BuildTopN(sql.ToString(), 1);
            model.Parameters = Common.MergeParameter(list, whereModel.Parameters);
            return model;
        }
    }
}
