using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.Library.SimpleMapper
{
    public class DB2Convert : ISQLConvert
    {
        public string BuildIfElseStatement(string judgement, string ifStatement, string elseStatement)
        {
            throw new NotImplementedException();
        }

        public string BuildTopN(string sql, int TopN)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(sql);
            if (TopN > 0) builder.AppendFormat(" fetch first {0} rows only", TopN);
            return builder.ToString();
        }

        public string FormatColumn(string columnName)
        {
            return columnName;
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
