using SOAFramework.Library.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.Library.SimpleMapper
{
    public class OrderByClause
    {
        private OrderBy direction = OrderBy.ASC;

        public string ColumnName { get; set; }

        public OrderBy Direction { get => direction; set => direction = value; }
    }
}
