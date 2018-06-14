using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chainway.Library.SimpleMapper
{
    public interface ISqlMapper
    {
        SqlModel ObjectToSql(string tableName, IDictionary<string, object> o, IList<WhereClause> wheres, TableConfig config = null);

        ISQLConvert Converter { get; set; }
    }
}