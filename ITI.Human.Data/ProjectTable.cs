using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;

namespace ITI.Human.Data
{
    [SqlTable("tProject", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class ProjectTable : SqlTable
    {
        void StObjConstruct(ProjectTypeTable pTypeTable, SemesterTable sTable)
        {
        }

        [SqlProcedure("sProjectCreate")]
        public abstract int Create(ISqlCallContext ctx, int actorId, int typeId, int semesterId, string name, string headline, string pitch);

        [SqlProcedure("sProjectDelete")]
        public abstract bool Delete(ISqlCallContext ctx, int actorId, int projectId);
    }
}
