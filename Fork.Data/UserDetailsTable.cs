using CK.DB.Actor;
using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System;
using System.Threading.Tasks;

namespace Fork.Data
{
    [SqlTable("tUserDetails", Package = typeof(Package))]
    [Versions("1.0.0")]
    [SqlObjectItem("vUserProfile")]
    public abstract class UserDetailsTable : SqlTable
    {
        void StObjConstruct(UserTable uTable)
        {
        }

        [SqlProcedure("sUserDetailsCreate")]
        public abstract Task<int> Create(ISqlCallContext ctx, int actorId, int userId, string firstName, string lastName, DateTime birthDate);

        [SqlProcedure("sUserDetailsUpdate")]
        public abstract Task<bool> Update(ISqlCallContext ctx, int actorId, int userId, string firstName, string lastName, DateTime birthDate);
    }
}
