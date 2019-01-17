using CK.DB.Actor;
using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System.Threading.Tasks;

namespace ITI.Human.Data
{
    [SqlTable("tStudent", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class StudentTable : SqlTable
    {
        void StObjConstruct(UserTable uTable, SemesterTable sTable)
        {
        }

        [SqlProcedure("sStudentCreate")]
        public abstract Task<int> Create(ISqlCallContext ctx, int actorId, int userId, int semesterId);
    }
}
