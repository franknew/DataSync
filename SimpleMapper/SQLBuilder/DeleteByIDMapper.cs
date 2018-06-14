
using SOAFramework.Library.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using SOAFramework.Library;

namespace Chainway.Library.SimpleMapper
{
    public class DeleteByIDMapper : ISqlMapper
    {
        public ISQLConvert Converter { get; set; }

        public DeleteByIDMapper(ISQLConvert converter)
        {
            Converter = converter;
        }

        public SqlModel ObjectToSql(string tableName, IDictionary<string, object> o, IList<WhereClause> where, TableConfig config = null)
        {
            SqlModel model = new SqlModel();
            StringBuilder sql = new StringBuilder();
            List<Parameter> list = new List<Parameter>();
            if (where == null) where = new List<WhereClause>();

            sql.AppendFormat("DELETE FROM {0} ", tableName);
            var primarykeys = config?.ColumnMapping?.FindAll(t => t.Primarykey);
            foreach (var column in primarykeys)
            {
                string columnName = Common.GetColumnName(column.SourceColumn, column);
                if (string.IsNullOrEmpty(columnName)) continue;
                var value = o[column.SourceColumn];
                where.Add(new WhereClause { ColumnName = columnName, Seperator = "=", Value = value, DataType = Common.GetType(column?.DataType, value.GetType(), value) });
            }
            if (where.Count > 0) sql.Append(" WHERE ");
            var whereModel = Common.BuildWhere(where, Converter);
            sql.Append(whereModel.SQL);

            model.Parameters = Common.MergeParameter(list, whereModel.Parameters);
            model.SQL = sql.ToString();
            return model;
        }
    }
}