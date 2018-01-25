using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.Library.SimpleMapper
{
    public interface ISQLConvert
    {
        string FormatColumn(string columnName);
        string BuildTopN(string sql, int TopN);
        string FormatParameter(string columnName);
        string BuildIfElseStatement(string judgement, string ifStatement, string elseStatement);
    }
}
