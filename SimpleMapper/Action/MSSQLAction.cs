using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SOAFramework.Library.DAL;

namespace Chainway.Library.SimpleMapper
{
    public class MSSQLAction : BaseSQLAction
    {
        public MSSQLAction(string key, TableConfig config, IDBHelper helper, bool lockable = true)
            : base(key, config, helper, lockable)
        { }

        public override int InsertOrUpdate(IDictionary<string, object> o)
        {
            ISqlMapper mapper = new InsertOrUpdateMapper(Factory.CreateConverter(_helper.DBType));
            var model = mapper.ObjectToSql(Common.GetTableName(_key, _config.Owner, o.GetType(), _config, o), o, null, _config);
            int result = 0;
            result = _helper.ExecNoneQueryWithSQL(model.SQL, model.Parameters.ToArray());
            return result;
        }
    }
}
