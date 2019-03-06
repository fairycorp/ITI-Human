using CK.Setup;
using CK.SqlServer.Setup;

namespace Fork.Data
{
    [SqlTable("tClassroom", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class ClassroomTable : SqlTable
    {
        void StObjConstruct()
        {
        }
    }
}
