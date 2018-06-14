using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinformTest
{
    public class Dependency
    {
        public string name { get; set; }
        public List<string> dependencies { get; set; }
    }
}
