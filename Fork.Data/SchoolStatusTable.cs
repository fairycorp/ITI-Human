using CK.Setup;
using CK.SqlServer.Setup;

namespace Fork.Data
{
    [SqlTable("tSchoolStatus", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class SchoolStatusTable : SqlTable
    {
        void StObjConstruct()
        {
        }
    }
}
