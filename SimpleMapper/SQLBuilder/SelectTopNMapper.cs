using SOAFramework.Library;
using SOAFramework.Library.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Chainway.Library.SimpleMapper
{
    public class SelectTopNMapper : ISqlMapper
    {
        private SimpleLogger _logger = new SimpleLogger();
        public SelectTopNMapper(ISQLConvert converter)
        {
            Converter = converter;
        }

        public SqlModel ObjectToSql(string tableName, Dictionary<string, object> o, List<WhereClause> where, TableConfig config = null)
        {
            SqlModel model = new SqlModel();
            StringBuilder builder = new StringBuilder();
            List<Parameter> list = new List<Parameter>();
            var t = o.GetType();

            if (string.IsNullOrEmpty(config?.SelectSQL))
            {
                builder.Append("SELECT * ");
                builder.AppendFormat(" FROM {0}", tableName);
            }
            else
            {
                builder.Append(config.SelectSQL);
                builder.Replace("{TableName}", tableName);
            }

            var whereModel = Common.BuildWhere(where, Converter);
            string sql = builder.ToString();
            bool hasWhere = sql.ToLower().IndexOf(" where ") > -1;
            if (hasWhere && where != null && where.Count > 0) builder.Append(" AND ");
            else if (!hasWhere && where != null && where.Count > 0) builder.Append(" WHERE ");
            builder.Append(whereModel.SQL);
            if (OrderBy != null) builder.AppendFormat(" ORDER BY {0} {1}", this.OrderBy.ColumnName, this.OrderBy.Direction.ToString());

            model.SQL = Converter.BuildTopN(builder.ToString(), TopN);
            //_logger.Write(model.SQL);
            model.Parameters = Common.MergeParameter(list, whereModel.Parameters);
            StringBuilder sb = new StringBuilder();
            //foreach (var p in model.Parameters)
            //{
            //    sb.AppendFormat(" param name:{0} value:{1} ", p.Name, p.Value?.ToString());

            //}
            //_logger.Debug(sb.ToString());
            return model;
        }

        public int TopN { get; set; }

        public OrderByClause OrderBy { get; set; }

        public ISQLConvert Converter { get; set; }
    }
}
