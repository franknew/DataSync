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
        int Insert(Dictionary<string, object> o);
        int Update(Dictionary<string, object> o, List<WhereClause> where);
        int Delete(Dictionary<string, object> o, List<WhereClause> where);
        int InsertOrUpdate(Dictionary<string, object> o);
        DataTable SelectTopN(int TopN, Dictionary<string, object> o, List<WhereClause> where, OrderByClause orderby);

    }
}
