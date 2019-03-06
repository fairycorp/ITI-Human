using CK.DB.Actor;
using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System.Threading.Tasks;

namespace Fork.Data
{
    [SqlTable("tUserBalance", Package = typeof(Package))]
    [Versions("1.0.0")]
    [SqlObjectItem("vUserBalance")]
    public abstract class UserBalanceTable : SqlTable
    {
        void StObjConstruct(UserTable uTable, ProjectTable pTable)
        {
        }

        [SqlProcedure("sUserBalanceCreate")]
        public abstract Task<int> Create(ISqlCallContext ctx, int actorId, int userId, int projectId);

        [SqlProcedure("sUserBalanceUpdate")]
        public abstract Task<bool> Update(ISqlCallContext ctx, int actorId, int userBalanceId, int amount);
    }
}
