using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.Library.SimpleMapper
{
    public class WhereClause
    {
        public string ColumnName { get; set; }

        public string Seperator { get; set; }

        public object Value { get; set; }

        public Type DataType { get; set; }
    }
}
