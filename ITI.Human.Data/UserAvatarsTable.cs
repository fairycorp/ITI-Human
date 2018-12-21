using CK.DB.Actor;
using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System.Threading.Tasks;

namespace ITI.Human.Data
{
    [SqlTable("tUserAvatars", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class UserAvatarsTable : SqlTable
    {
        void StObjConstruct(UserTable uTable)
        {
        }

        [SqlProcedure("sUserAvatarCreate")]
        public abstract Task<int> Create(ISqlCallContext ctx, int actorId, int userId, string image);
    }
}
