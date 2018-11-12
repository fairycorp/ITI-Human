using ITI.Human.ViewModels.Order;
using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System.Threading.Tasks;

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

        [SqlProcedure("sOrderedProductUpdate")]
        public abstract Task<State> Update(ISqlCallContext ctx, int actorId, int orderedProductId, int currentState);

        [SqlProcedure("sOrderedProductDelete")]
        public abstract Task<bool> Delete(ISqlCallContext ctx, int actorId, int orderedProductId);
    }
}
