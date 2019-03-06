using CK.Setup;
using CK.SqlServer.Setup;

namespace Fork.Data
{
    [SqlTable("tOPCurrentStateUpdateTrack", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class OPPaymentStateUpdateTrackTable : SqlTable
    {
        void StObjConstruct(OrderedProductUpdateTrackTable opUTrackTable)
        {
        }
    }
}
