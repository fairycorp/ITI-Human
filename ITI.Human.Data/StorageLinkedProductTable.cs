using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System;
using System.Threading.Tasks;

namespace ITI.Human.Data
{
    [SqlTable("tStorageLinkedProduct", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class StorageLinkedProductTable : SqlTable
    {
        void StObjConstruct(StorageTable sTable, ProductTable pTable)
        {
        }

        [SqlProcedure("sStorageLinkedProductCreate")]
        public abstract Task<int> Create(ISqlCallContext ctx, int actorId, int storageId, int productId, int unitPrice, int stock);

        [SqlProcedure("sStorageLinkedProductUnitPriceUpdate")]
        public abstract Task<bool> UpdateUnitPrice(ISqlCallContext ctx, int actorId, DateTime updateDate, int storageLinkedProductId, int unitPrice);

        [SqlProcedure("sStorageLinkedProductStockUpdate")]
        public abstract Task<bool> UpdateStock(ISqlCallContext ctx, int actorId, DateTime updateDate, int storageLinkedProductId, int stock);

        [SqlProcedure("sStorageLinkedProductCreditStateUpdate")]
        public abstract Task<bool> UpdateCreditState(ISqlCallContext ctx, int actorId, DateTime updateDate, int storageLinkedProductId, bool creditState);
    }
}
