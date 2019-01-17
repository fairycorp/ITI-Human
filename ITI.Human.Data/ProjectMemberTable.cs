using CK.DB.Actor;
using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System.Threading.Tasks;

namespace ITI.Human.Data
{
    [SqlTable("tProjectMember", Package = typeof(Package))]
    [Versions("1.0.0")]
    [SqlObjectItem("vProjectMembers")]
    public abstract class ProjectMemberTable : SqlTable
    {
        void StObjConstruct(ProjectTable pTable, ProjectRankTable pRTable, UserTable uTable)
        {
        }

        [SqlProcedure("sProjectMemberCreate")]
        public abstract Task<int> Create(ISqlCallContext ctx, int actorId, int projectId, int projectRankId, int userId);

        [SqlProcedure("sProjectMemberDelete")]
        public abstract Task<bool> Delete(ISqlCallContext ctx, int actorId, int projectMemberId);
    }
}
