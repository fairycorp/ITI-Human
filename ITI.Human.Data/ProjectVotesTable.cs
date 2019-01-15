using CK.DB.Actor;
using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System.Threading.Tasks;

namespace ITI.Human.Data
{
    [SqlTable("tProjectVotes", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class ProjectVotesTable : SqlTable
    {
        void StObjConstruct(ProjectTable pTable, UserTable uTable)
        {
        }

        [SqlProcedure("sProjectVoteCreate")]
        public abstract Task<int> Create(ISqlCallContext ctx, int actorId, int projectId, int userId, int note);
    }
}
