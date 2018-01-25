using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chainway.Library.SimpleMapper
{
    public interface ISqlMapper
    {
        SqlModel ObjectToSql(string tableName, Dictionary<string, object> o, List<WhereClause> wheres, TableConfig config = null);

        ISQLConvert Converter { get; set; }
    }
}