using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Chainway.Library.SimpleMapper
{
    public class InsertOrUpdateMapper : ISqlMapper
    {
        public InsertOrUpdateMapper(ISQLConvert converter)
        {
            Converter = converter;
        }

        public ISQLConvert Converter { get; set; }

        public SqlModel ObjectToSql(string tableName, IDictionary<string, object> o, IList<WhereClause> where, TableConfig config = null)
        {
            InsertMapper insertMapper = new InsertMapper(Converter);
            UpdateByIDMapper updateMapper = new UpdateByIDMapper(Converter);
            StringBuilder judgement = new StringBuilder();
            SqlModel model = new SqlModel();


            judgement.AppendFormat(" EXISTS(SELECT 1 FROM {0} WHERE ", tableName);
            foreach (var key in o.Keys)
            {
                var column = config?.ColumnMapping?.Find(t => t.SourceColumn.ToLower().Equals(key.ToLower()));
                if (column != null && column.Ingore) continue;
                if (column == null || !column.Primarykey) continue;
                if (column != null && column.GenerateID && o[key] == null)
                {
                    IIDGenerator generator = IDGeneratorFactory.Create(GeneratorType.SnowFlak);
                    o.SetValue(key, generator.Generate());
                }
                string columnName = Common.GetColumnName(key, column);
                if (string.IsNullOrEmpty(columnName)) continue;
                judgement.AppendFormat("{0}={1} AND ", Converter.FormatColumn(columnName), Converter.FormatParameter(columnName));
            }

            var insertModel = insertMapper.ObjectToSql(tableName, o, where, config);
            var updateModel = updateMapper.ObjectToSql(tableName, o, where, config);

            judgement.Remove(judgement.Length - 4, 4);
            judgement.Append(")");
            string sql = Converter.BuildIfElseStatement(judgement.ToString(), updateModel.SQL, insertModel.SQL);

            model.SQL = sql;
            model.Parameters = updateModel.Parameters;
            return model;
        }
    }
}
