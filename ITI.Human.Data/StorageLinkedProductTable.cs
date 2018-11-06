using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;

namespace ITI.Human.Data
{
    [SqlTable("tStorageLinkedProduct", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class StorageLinkedProduct : SqlTable
    {
        void StObjConstruct(StorageTable sTable, ProductTable pTable)
        {
        }

        [SqlProcedure("sStorageLinkedProductCreate")]
        public abstract int Create(ISqlCallContext ctx, int actorId, int storageId, int productId, double unitPrice, int quantity);
    }
}
