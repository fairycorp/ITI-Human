using CK.DB.Actor;
using CK.Setup;
using CK.SqlServer.Setup;

namespace ITI.Human.Data
{
    [SqlTable("tSchoolMember", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class SchoolMemberTable : SqlTable
    {
        void StObjConstruct(SchoolStatusTable sStable, UserTable uTable)
        {
        }
    }
}
