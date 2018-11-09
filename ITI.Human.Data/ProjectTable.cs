using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System.Threading.Tasks;

namespace ITI.Human.Data
{
    [SqlTable("tProject", Package = typeof(Package))]
    [Versions("1.0.0")]
    [SqlObjectItem("vProjects")]
    public abstract class ProjectTable : SqlTable
    {
        void StObjConstruct(ProjectTypeTable pTypeTable, SemesterTable sTable)
        {
        }

        [SqlProcedure("sProjectCreate")]
        public abstract Task<int> Create(ISqlCallContext ctx, int actorId, int typeId, int semesterId, string name, string headline, string pitch);

        [SqlProcedure("sProjectDelete")]
        public abstract Task<bool> Delete(ISqlCallContext ctx, int actorId, int projectId);
    }
}
