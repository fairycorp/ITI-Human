using CK.DB.Actor;
using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System;
using System.Threading.Tasks;

namespace ITI.Human.Data
{
    [SqlTable("tUserDetails", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class UserDetailsTable : SqlTable
    {
        void StObjConstruct(UserTable uTable)
        {
        }

        [SqlProcedure("sUserDetailsCreate")]
        public abstract Task<int> Create(ISqlCallContext ctx, int actorId, int userId, string firstName, string lastName, DateTime birthDate);
    }
}
