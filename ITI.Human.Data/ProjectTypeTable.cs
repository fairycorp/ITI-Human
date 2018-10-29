using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;

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
        public abstract int Create(ISqlCallContext ctx, int actorId, string name);
    }
}
