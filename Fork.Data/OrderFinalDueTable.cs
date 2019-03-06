using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System;
using System.Threading.Tasks;

namespace Fork.Data
{
    [SqlTable("tOrderFinalDue", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class OrderFinalDueTable : SqlTable
    {
        void StObjConstruct(OrderTable oTable)
        {
        }

        [SqlProcedure("sOrderFinalDueCreate")]
        public abstract Task<int> Create(ISqlCallContext ctx, int actorId, int orderId, int total, int paid);

        [SqlProcedure("sOrderFinalDueUpdate")]
        public abstract Task<bool> Update(ISqlCallContext ctx, int actorId, int orderFinalDueId, int paid);
    }
}
