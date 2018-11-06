using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;

namespace ITI.Human.Data
{
    [SqlTable("tStorage", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class StorageTable : SqlTable
    {
        void StObjConstruct(ProjectTable pTable)
        {
        }

        [SqlProcedure("sStorageCreate")]
        public abstract int Create(ISqlCallContext ctx, int actorId, int projectId);
    }
}
