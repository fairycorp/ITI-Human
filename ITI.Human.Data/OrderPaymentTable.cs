using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System;
using System.Threading.Tasks;

namespace ITI.Human.Data
{
    [SqlTable("tOrderPayment", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class OrderPaymentTable : SqlTable
    {
        void StObjConstruct(OrderFinalDueTable oTable, OrderedProductTable oPTable)
        {
        }

        [SqlProcedure("sOrderPaymentCreate")]
        public abstract Task<int> Create(ISqlCallContext ctx, int actorId, int orderFinalDueId, int orderedProductId, int amount, DateTime paymentTime);

        [SqlProcedure("sOrderPaymentDelete")]
        public abstract Task<bool> Delete(ISqlCallContext ctx, int actorId, int orderedProductId);
    }
}
