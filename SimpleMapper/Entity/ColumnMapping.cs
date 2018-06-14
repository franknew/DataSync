using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.Library.SimpleMapper
{
    public class ColumnMapping
    {
        /// <summary>
        /// 源列
        /// </summary>
        public string SourceColumn { get; set; }
        /// <summary>
        /// 匹配列，用来匹配列名
        /// </summary>
        public string TargetColumn { get; set; }
        /// <summary>
        /// 是否忽略该列
        /// </summary>
        public bool Ingore { get; set; }
        /// <summary>
        /// 是否主键
        /// </summary>
        public bool Primarykey { get; set; }
        /// <summary>
        /// 数据类型，.net类型
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 数据从sql中获取
        /// </summary>
        public string FromSQL { get; set; }
        /// <summary>
        /// 是否同步中产生id
        /// </summary>
        public bool GenerateID { get; set; }
    }
}
