using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chainway.ServiceMonitor.SDK
{
    public class Command
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 程序名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public string Argruments { get; set; }

        /// <summary>
        /// 是否停止
        /// </summary>
        public bool Stop { get; set; }

        /// <summary>
        /// id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Executor { get; set; }

        /// <summary>
        /// 延迟启动
        /// </summary>
        public int Delay { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否windows service
        /// </summary>
        public bool IsWindowsService { get; set; }
    }
}
