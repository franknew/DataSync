using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.Library.SimpleMapper
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute :Attribute
    {
        public TableAttribute(string tableName)
        {
            TableName = tableName;
        }

        public string TableName { get; set; }
    }
}
