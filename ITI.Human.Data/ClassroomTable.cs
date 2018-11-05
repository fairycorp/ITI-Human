using CK.Setup;
using CK.SqlServer.Setup;

namespace ITI.Human.Data
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
