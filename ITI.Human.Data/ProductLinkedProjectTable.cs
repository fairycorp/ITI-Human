using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;

namespace ITI.Human.Data
{
    [SqlTable("tProductLinkedProject", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class ProductLinkedProjectTable : SqlTable
    {
        void StObjConstruct(ProductTable pTable)
        {
        }

        [SqlProcedure("sProductLinkedProjectCreate")]
        public abstract int Create(ISqlCallContext ctx, int actorId, int productId, int projectId, bool availability);

        [SqlProcedure("sProductLinkedProjectDelete")]
        public abstract bool Delete(ISqlCallContext ctx, int actorId, int productLinkedProjectId);
    }
}
