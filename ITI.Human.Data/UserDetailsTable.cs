using CK.DB.Actor;
using CK.Setup;
using CK.SqlServer.Setup;

namespace ITI.Human.Data
{
    [SqlTable("tUserDetails", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class UserDetailsTable : SqlTable
    {
        void StObjConstruct(UserTable uTable)
        {
        }
    }
}
