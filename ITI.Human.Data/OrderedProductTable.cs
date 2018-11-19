using ITI.Human.ViewModels.Order;
using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System.Threading.Tasks;
using System;
using ITI.Human.ViewModels.Product.Ordered;

namespace ITI.Human.Data
{
    [SqlTable("tOrderedProduct", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class OrderedProductTable : SqlTable
    {
        void StObjConstruct(OrderTable oTable, StorageLinkedProductTable sLPTable)
        {
        }

        [SqlProcedure("sOrderedProductCreate")]
        public abstract Task<int> Create(ISqlCallContext ctx, int actorId, int orderId, int storageLinkedProductId, int quantity);

        [SqlProcedure("sOrderedProductCurrentStateUpdate")]
        public abstract Task<bool> UpdateCurrentState(ISqlCallContext ctx, int actorId, DateTime updateDate, int orderedProductId, State currentState);

        [SqlProcedure("sOrderedProductPaymentStateUpdate")]
        public abstract Task<bool> UpdatePaymentState(ISqlCallContext ctx, int actorId, DateTime updateDate, int orderedProductId, int orderFinalDueId,
            Payment paymentState, int amount);

        [SqlProcedure("sOrderedProductDelete")]
        public abstract Task<bool> Delete(ISqlCallContext ctx, int actorId, int orderedProductId);
    }
}
