using SOAFramework.Library.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.Library.SimpleMapper
{
    public class SqlModel
    {
        public string SQL { get; set; }
        public List<Parameter> Parameters { get; set; }
    }
}
