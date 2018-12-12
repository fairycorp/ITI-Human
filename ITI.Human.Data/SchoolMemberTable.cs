using CK.DB.Actor;
using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System.Threading.Tasks;

namespace ITI.Human.Data
{
    [SqlTable("tSchoolMember", Package = typeof(Package))]
    [Versions("1.0.0")]
    [SqlObjectItem("vSchoolMembers")]
    public abstract class SchoolMemberTable : SqlTable
    {
        void StObjConstruct(SchoolStatusTable sStable, UserTable uTable)
        {
        }

        [SqlProcedure("sSchoolMemberCreate")]
        public abstract Task<int> Create(ISqlCallContext ctx, int actorId, int userId, int schoolStatusId);
    }
}
