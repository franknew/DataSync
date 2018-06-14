using SOAFramework.Library.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SOAFramework.Library;

namespace Chainway.Library.SimpleMapper
{
    public class BaseSQLAction : ISQLAction
    {
        protected IDBHelper _helper;
        protected TableConfig _config;
        protected string _key;

        public BaseSQLAction(string key, TableConfig config, IDBHelper helper, bool lockable = true)
        {
            _config = config;
            _helper = helper;
            _key = key;
        }
        public virtual int Delete(IDictionary<string, object> o, IList<WhereClause> where)
        {
            ISqlMapper mapper = new DeleteByIDMapper(Factory.CreateConverter(_helper.DBType));
            var model = mapper.ObjectToSql(Common.GetTableName(_key, _config.Owner, o.GetType(), _config, o), o, where, _config);
            int result = 0;
            result = _helper.ExecNoneQueryWithSQL(model.SQL, model.Parameters.ToArray());
            return result;
        }

        public virtual int Insert(IDictionary<string, object> o)
        {
            ISqlMapper mapper = new InsertMapper(Factory.CreateConverter(_helper.DBType));
            var model = mapper.ObjectToSql(Common.GetTableName(_key, _config.Owner, o.GetType(), _config, o), o, null, _config);
            int result = 0;
            result = _helper.ExecNoneQueryWithSQL(model.SQL, model.Parameters.ToArray());
            return result;
        }

        public virtual int InsertOrUpdate(IDictionary<string, object> o)
        {
            throw new NotImplementedException();
        }

        public virtual DataTable SelectTopN(int TopN, IDictionary<string, object> o, IList<WhereClause> where, OrderByClause orderby)
        {
            SelectTopNMapper mapper = new SelectTopNMapper(Factory.CreateConverter(_helper.DBType));
            mapper.TopN = TopN;
            mapper.OrderBy = orderby;

            var model = mapper.ObjectToSql(Common.GetTableName(_key, _config.Owner, o.GetType(), _config, o), o, where, _config);
            DataTable table = new DataTable();
            table = _helper.GetTableWithSQL(model.SQL, model.Parameters.ToArray());
            return table;
        }

        public virtual int Update(IDictionary<string, object> o, IList<WhereClause> where)
        {
            ISqlMapper mapper = new UpdateByIDMapper(Factory.CreateConverter(_helper.DBType));
            var model = mapper.ObjectToSql(Common.GetTableName(_key, _config.Owner, o.GetType(), _config, o), o, where, _config);
            int result = 0;
            result = _helper.ExecNoneQueryWithSQL(model.SQL, model.Parameters.ToArray());
            return result;
        }
    }
}
