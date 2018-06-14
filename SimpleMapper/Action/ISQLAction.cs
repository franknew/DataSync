using SOAFramework.Library.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Chainway.Library.SimpleMapper
{
    public interface ISQLAction
    {
        int Insert(IDictionary<string, object> o);
        int Update(IDictionary<string, object> o, IList<WhereClause> where);
        int Delete(IDictionary<string, object> o, IList<WhereClause> where);
        int InsertOrUpdate(IDictionary<string, object> o);
        DataTable SelectTopN(int TopN, IDictionary<string, object> o, IList<WhereClause> where, OrderByClause orderby);

    }
}
