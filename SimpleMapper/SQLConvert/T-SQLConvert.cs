using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.Library.SimpleMapper
{
    public class T_SQLConvert : ISQLConvert
    {
        public string BuildIfElseStatement(string judgement, string ifStatement, string elseStatement)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("IF ({0}) BEGIN {1} END ", judgement, ifStatement);
            if (!string.IsNullOrEmpty(elseStatement)) sql.AppendFormat(" ELSE BEGIN {0} END ", elseStatement);
            return sql.ToString();
        }

        public string BuildTopN(string sql, int TopN)
        {
            StringBuilder builder = new StringBuilder();
            if (TopN > 0)
            {
                var selectIndex = sql.ToLower().IndexOf("select ") + 7;
                builder.Append(sql.Substring(0, selectIndex)).AppendFormat(" TOP {0} ", TopN).Append(sql.Substring(selectIndex, sql.Length - selectIndex));
            }
            else builder.Append(sql);
            return builder.ToString();
        }

        public string FormatColumn(string columnName)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("[{0}]", columnName);
            return builder.ToString();
        }

        public string FormatParameter(string columnName)
        {
            StringBuilder builder = new StringBuilder();
            int dotIndex = columnName.IndexOf(".");
            if (dotIndex > -1) columnName = columnName.Remove(0, dotIndex + 1);
            builder.AppendFormat("@{0}", columnName);
            return builder.ToString();
        }
    }
}
