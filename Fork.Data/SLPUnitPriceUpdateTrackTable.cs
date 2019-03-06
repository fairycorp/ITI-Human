using CK.Setup;
using CK.SqlServer.Setup;

namespace Fork.Data
{
    [SqlTable("tSLPUnitPriceUpdateTrack", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class SLPUnitPriceUpdateTrackTable : SqlTable
    {
        void StObjConstruct(StorageLinkedProductUpdateTrackTable slpUpdateTrackTable)
        {
        }
    }
}
