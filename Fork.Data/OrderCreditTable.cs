using CK.DB.Actor;
using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System;
using System.Threading.Tasks;

namespace Fork.Data
{
    [SqlTable("tOrderCredit", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class OrderCreditTable : SqlTable
    {
        void StObjConstruct(OrderedProductTable oPTable, UserTable uTable)
        {
        }

        [SqlProcedure("sOrderCreditCreate")]
        public abstract Task<int> Create(ISqlCallContext ctx, int actorId, int projectId, int userId, int amount, DateTime creditTime);
    }
}
