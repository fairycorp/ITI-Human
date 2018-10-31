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
        void StObjConstruct(OrderTable oTable, ProductTable pTable)
        {
        }

        [SqlProcedure("sOrderedProductCreate")]
        public abstract Task<int> Create(ISqlCallContext ctx, int actorId, int orderId, int productId, int amount);

        [SqlProcedure("sOrderedProductUpdate")]
        public abstract Task<bool> Update(ISqlCallContext ctx, int actorId, int orderedProductId, bool hasBeenDelivered);

        [SqlProcedure("sOrderedProductDelete")]
        public abstract Task<bool> Delete(ISqlCallContext ctx, int actorId, int orderedProductId);
    }
}
