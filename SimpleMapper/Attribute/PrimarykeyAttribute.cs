
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chainway.Library.SimpleMapper
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimarykeyAttribute : Attribute
    {
        public bool Autocreasement { get; set; }
    }
}