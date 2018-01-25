using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.Library.SimpleMapper
{
    public class TableConfig
    {
        private string _tableName;
        private DateTime tableTime = DateTime.Now;

        public string TableName
        {
            get => _tableName;
            set => _tableName = value;
        }

        public string ID { get; set; }

        public string TypeName { get; set; }

        /// <summary>
        /// 轮询的间隔时间，毫秒
        /// </summary>
        public int Tick { get; set; }

        /// <summary>
        /// db owner
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// 表名的规则
        /// </summary>
        public string Regex { get; set; }

        /// <summary>
        /// 推送到mq之后是否删除数据
        /// </summary>
        public bool DeleteData { get; set; }

        /// <summary>
        /// 条件列
        /// </summary>
        public string CheckColumn { get; set; }

        /// <summary>
        /// 转换列
        /// </summary>
        public string Mapping { get; set; }

        /// <summary>
        /// 是否忽略该配置
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// 同步动作:Insert,Delete,Update,InsertOrUpdate
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 生成update语句时的列，用","分割
        /// </summary>
        public string UpdateColumns { get; set; }

        public List<ColumnMapping> ColumnMapping { get; set; }

        /// <summary>
        /// 可以自定义查询sql语句
        /// </summary>
        public string SelectSQL { get; set; }

        public string Tag
        {
            get
            {
                string tag = null;
                if (!string.IsNullOrEmpty(this.Regex)) tag = string.Format(this.Regex, DateTime.Now);

                return tag;
            }
        }

        public DateTime TableTime { set => tableTime = value; }

        public string OrginalTableName
        {
            get
            {
                string tableName = "";
                tableName = _tableName + Tag;
                return tableName;
            }
        }

        public string FormatedTableName
        {
            get
            {
                string tableName;
                if (string.IsNullOrEmpty(Mapping)) tableName = OrginalTableName;
                else tableName = Mapping + Tag;
                return tableName;
            }
        }

        public string IDOrTableName
        {
            get
            {
                string name = FormatedTableName;
                if (!string.IsNullOrEmpty(ID)) name = ID;
                return name;
            }
        }

        public TableConfig Copy()
        {
            return new TableConfig
            {
                Action = this.Action,
                CheckColumn = this.CheckColumn,
                ColumnMapping = this.ColumnMapping,
                DeleteData = this.DeleteData,
                ID = this.ID,
                Mapping = this.Mapping,
                Owner = this.Owner,
                Regex = this.Regex,
                SelectSQL = this.SelectSQL,
                TableName = this.TableName,
                Tick = this.Tick,
                TypeName = this.TypeName,
                
            };
        }
    }
}
