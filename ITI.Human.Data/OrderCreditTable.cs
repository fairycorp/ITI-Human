using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System;
using System.Threading.Tasks;

namespace ITI.Human.Data
{
    [SqlTable("tOrderCredit", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class OrderCreditTable : SqlTable
    {
        void StObjConstruct(OrderedProductTable oPTable)
        {
        }

        [SqlProcedure("sOrderCreditCreate")]
        public abstract Task<int> Create(ISqlCallContext ctx, int actorId, int orderedProductId, int amount, DateTime creditTime);
    }
}
