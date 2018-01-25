using SOAFramework.Library;
using SOAFramework.Library.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Chainway.Library.SimpleMapper
{
    public class DB2Action : BaseSQLAction
    {
        private SimpleLogger _logger = new SimpleLogger();
        public DB2Action(string key, TableConfig config, IDBHelper helper, bool lockable = true)
            : base(key, config, helper, lockable)
        { }

        public override int InsertOrUpdate(Dictionary<string, object> o)
        {
            SelectByIDMapper existsMapper = new SelectByIDMapper(Factory.CreateConverter(_helper.DBType));
            var configs = _config.ColumnMapping?.FindAll(t => t.GenerateID);
            if (configs != null && configs.Count > 0)
            {
                configs.ForEach(t =>
                {
                    var value = o[t.SourceColumn];
                    if (value == null)
                    {
                        IIDGenerator generator = IDGeneratorFactory.Create(GeneratorType.SnowFlak);
                        o[t.SourceColumn] = generator.Generate();
                    }
                });
            }
            string tableName = Common.GetTableName(_key, _config.Owner, o.GetType(), _config, o);
            var existsModel = existsMapper.ObjectToSql(tableName, o, null, _config);
            existsModel.SQL = Common.ReplaceParameter(existsModel.SQL, existsModel.Parameters);
            int result = 0;
            result = Execute(tableName, o, existsModel);
            return result;
        }

        private int Execute(string tableName, Dictionary<string, object> o, SqlModel model)
        {
            int result = 0;
            DataTable table = null;
            try
            {

                if (_lockable)
                {
                    lock (this)
                    {
                        table = _helper.GetTableWithSQL(model.SQL, model.Parameters.ToArray());
                    }
                }
                else table = _helper.GetTableWithSQL(model.SQL, model.Parameters.ToArray());
            }
            catch (Exception ex)
            {
                _logger.Write(model.SQL);
                throw ex;
            }
            ISqlMapper mapper = null;
            if (table.Rows.Count > 0) mapper = new UpdateByIDMapper(Factory.CreateConverter(_helper.DBType));
            else mapper = new InsertMapper(Factory.CreateConverter(_helper.DBType));

            var saveModel = mapper.ObjectToSql(tableName, o, null, _config);
            saveModel.SQL = Common.ReplaceParameter(saveModel.SQL, saveModel.Parameters);
            try
            {
                if (_lockable)
                {
                    lock (this)
                    {
                        result = _helper.ExecNoneQueryWithSQL(saveModel.SQL, saveModel.Parameters.ToArray());
                    }
                }
                else result = _helper.ExecNoneQueryWithSQL(saveModel.SQL, saveModel.Parameters.ToArray());
            }
            catch (Exception ex)
            {
                _logger.Write(saveModel.SQL);
                throw ex;
            }
            return result;
        }

    }
}
