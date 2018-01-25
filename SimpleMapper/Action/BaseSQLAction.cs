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
        protected bool _lockable;

        public BaseSQLAction(string key, TableConfig config, IDBHelper helper, bool lockable = true)
        {
            _config = config;
            _helper = helper;
            _key = key;
            _lockable = lockable;
        }
        public virtual int Delete(Dictionary<string, object> o, List<WhereClause> where)
        {
            ISqlMapper mapper = new DeleteByIDMapper(Factory.CreateConverter(_helper.DBType));
            var model = mapper.ObjectToSql(Common.GetTableName(_key, _config.Owner, o.GetType(), _config, o), o, where, _config);
            model.SQL = Common.ReplaceParameter(model.SQL, model.Parameters);
            int result = 0;
            if (_lockable)
            {
                lock(this)
                {
                    result = _helper.ExecNoneQueryWithSQL(model.SQL, model.Parameters.ToArray());
                }
            }
            else result = _helper.ExecNoneQueryWithSQL(model.SQL, model.Parameters.ToArray());
            return result;
        }

        public virtual int Insert(Dictionary<string, object> o)
        {
            ISqlMapper mapper = new InsertMapper(Factory.CreateConverter(_helper.DBType));
            var model = mapper.ObjectToSql(Common.GetTableName(_key, _config.Owner, o.GetType(), _config, o), o, null, _config);
            model.SQL = Common.ReplaceParameter(model.SQL, model.Parameters);
            int result = 0;
            if (_lockable)
            {
                lock (this)
                {
                    result = _helper.ExecNoneQueryWithSQL(model.SQL, model.Parameters.ToArray());
                }
            }
            else result = _helper.ExecNoneQueryWithSQL(model.SQL, model.Parameters.ToArray());
            return result;
        }

        public virtual int InsertOrUpdate(Dictionary<string, object> o)
        {
            throw new NotImplementedException();
        }

        public virtual DataTable SelectTopN(int TopN, Dictionary<string, object> o, List<WhereClause> where, OrderByClause orderby)
        {
            SelectTopNMapper mapper = new SelectTopNMapper(Factory.CreateConverter(_helper.DBType));
            mapper.TopN = TopN;
            mapper.OrderBy = orderby;

            var model = mapper.ObjectToSql(Common.GetTableName(_key, _config.Owner, o.GetType(), _config, o), o, where, _config);
            model.SQL = Common.ReplaceParameter(model.SQL, model.Parameters);
            DataTable table = new DataTable();
            if (_lockable)
            {
                lock (this)
                {
                    table = _helper.GetTableWithSQL(model.SQL, model.Parameters.ToArray());
                }
            }
            else table = _helper.GetTableWithSQL(model.SQL, model.Parameters.ToArray());
            return table;
        }

        public virtual int Update(Dictionary<string, object> o, List<WhereClause> where)
        {
            ISqlMapper mapper = new UpdateByIDMapper(Factory.CreateConverter(_helper.DBType));
            var model = mapper.ObjectToSql(Common.GetTableName(_key, _config.Owner, o.GetType(), _config, o), o, where, _config);
            model.SQL = Common.ReplaceParameter(model.SQL, model.Parameters);
            int result = 0;
            if (_lockable)
            {
                lock (this)
                {
                    result = _helper.ExecNoneQueryWithSQL(model.SQL, model.Parameters.ToArray());
                }
            }
            else result = _helper.ExecNoneQueryWithSQL(model.SQL, model.Parameters.ToArray());
            return result;
        }
    }
}
