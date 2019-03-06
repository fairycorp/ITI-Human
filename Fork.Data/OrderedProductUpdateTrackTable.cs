using CK.Setup;
using CK.SqlServer.Setup;

namespace Fork.Data
{
    [SqlTable("tOrderedProductUpdateTrack", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class OrderedProductUpdateTrackTable : SqlTable
    {
        void StObjConstruct(UpdateTrackTable uTrackTable, OrderedProductTable opTable)
        {
        }
    }
}
