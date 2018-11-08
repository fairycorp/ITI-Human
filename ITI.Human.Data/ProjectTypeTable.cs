using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System.Threading.Tasks;

namespace ITI.Human.Data
{
    [SqlTable("tProjectType", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class ProjectTypeTable : SqlTable
    {
        void StObjConstruct()
        {
        }

        [SqlProcedure("sProjectTypeCreate")]
        public abstract Task<int> Create(ISqlCallContext ctx, int actorId, string name);
    }
}
