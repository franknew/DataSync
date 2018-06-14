using SOAFramework.Library;
using SOAFramework.Library.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.Library.SimpleMapper
{
    public class Common
    {
        private static SimpleLogger _logger = new SimpleLogger();
        /// <summary>
        /// 根据配置组合表名
        /// </summary>
        /// <param name="key"></param>
        /// <param name="owner"></param>
        /// <param name="t"></param>
        /// <param name="config"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetTableName(string key, string owner, Type t, TableConfig config, IDictionary<string, object> data)
        {
            string tableName = config.TableName;
            if (!string.IsNullOrEmpty(config.Mapping)) tableName = config.Mapping;
            if (string.IsNullOrEmpty(tableName)) tableName = owner + "." + t.Name + key;
            else tableName = owner + "." + tableName + key;
            tableName = string.Format(tableName, DateTime.Now);
            var viarables = tableName.GetVairable('{', '}');
            if (viarables.Count > 0)
            {
                foreach (var v in viarables)
                {
                    var name = v.Trim();
                    if (data.ContainsKey(name)) tableName = tableName.Replace("{" + v + "}", data[name]?.ToString());
                }
            }
            return tableName;
        }

        /// <summary>
        /// 根据配置组合列名
        /// </summary>
        /// <param name="column"></param>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public static string GetColumnName(string column, ColumnMapping mapping)
        {
            if (mapping == null) return column;
            if (mapping.Ingore) return null;
            string name = mapping.TargetColumn;
            if (string.IsNullOrEmpty(name)) name = column;
            return name;
        }

        /// <summary>
        /// 拼接where语句
        /// </summary>
        /// <param name="wheres"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static SqlModel BuildWhere(IList<WhereClause> wheres, ISQLConvert converter)
        {
            SqlModel model = new SqlModel();
            StringBuilder where = new StringBuilder();
            List<Parameter> list = new List<Parameter>();
            if (wheres == null || wheres.Count == 0) return model;
            foreach (var t in wheres)
            {
                if (t.Value == null)
                {
                    where.AppendFormat("{0}{1} NULL AND ", converter.FormatColumn(t.ColumnName), t.Seperator);
                }
                else
                {
                    where.AppendFormat("{0}{1}{2} AND ", converter.FormatColumn(t.ColumnName), t.Seperator, converter.FormatParameter(t.ColumnName));
                    list.Add(new Parameter { Name = converter.FormatParameter(t.ColumnName), Value = t.Value, Type = t.DataType == null ? t.Value.GetType() : t.DataType });
                }
            }
            where.Remove(where.Length - 4, 4);

            model.SQL = where.ToString();
            model.Parameters = list;
            return model;
        }

        /// <summary>
        /// 组合参数
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static List<Parameter> MergeParameter(IList<Parameter> left, IList<Parameter> right)
        {
            if (right == null || right.Count == 0) return left.ToList();
            if (left == null) left = new List<Parameter>();
            foreach (var t in right)
            {
                if (!left.ToList().Exists(p => p.Name.Equals(t.Name))) left.Add(t);
            }
            return left.ToList();
        }
        

        /// <summary>
        /// 获得目标值类型
        /// </summary>
        /// <param name="toType"></param>
        /// <param name="sourceType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Type GetType(string toType, Type sourceType, object value)
        {
            Type t = sourceType;
            if (!string.IsNullOrEmpty(toType)) t = Type.GetType(toType);
            if (t == null) throw new Exception("转换类型失败，请检查类型配置");
            return t;
        }
    }
}
