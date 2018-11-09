using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System.Threading.Tasks;

namespace ITI.Human.Data
{
    [SqlTable("tSemester", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class SemesterTable : SqlTable
    {
        void StObjConstruct(ProductTable pTable)
        {
        }

        [SqlProcedure("sSemesterCreate")]
        public abstract Task<int> Create(ISqlCallContext ctx, int actorId, string name);
    }
}
