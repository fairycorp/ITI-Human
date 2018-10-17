using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;

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
        public abstract int Create(ISqlCallContext ctx, int actorId, int orderId, int productId);
    }
}
