using CK.DB.Actor;
using CK.Setup;
using CK.SqlServer.Setup;

namespace Fork.Data
{
    [SqlTable("tUpdateTrack", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class UpdateTrackTable : SqlTable
    {
        void StObjConstruct(ActorTable aTable)
        {
        }
    }
}
