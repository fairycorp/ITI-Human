using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System.Threading.Tasks;

namespace Fork.Data
{
    [SqlTable("tStorage", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class StorageTable : SqlTable
    {
        void StObjConstruct(ProjectTable pTable)
        {
        }

        [SqlProcedure("sStorageCreate")]
        public abstract Task<int> Create(ISqlCallContext ctx, int actorId, int projectId);

        [SqlProcedure("sStallUpdate")]
        public abstract Task<bool> UpdateStall(ISqlCallContext ctx, int actorId, int storageId, bool openedStall);
    }
}
