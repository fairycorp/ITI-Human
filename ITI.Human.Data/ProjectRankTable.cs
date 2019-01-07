using CK.Setup;
using CK.SqlServer.Setup;

namespace ITI.Human.Data
{
    [SqlTable("tProjectRank", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class ProjectRankTable : SqlTable
    {
        void StObjConstruct()
        {
        }
    }
}
