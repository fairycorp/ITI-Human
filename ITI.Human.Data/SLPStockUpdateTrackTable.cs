using CK.Setup;
using CK.SqlServer.Setup;

namespace ITI.Human.Data
{
    [SqlTable("tSLPStockUpdateTrack", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class SLPStockUpdateTrackTable : SqlTable
    {
        void StObjConstruct(StorageLinkedProductUpdateTrackTable slpUpdateTrackTable)
        {
        }
    }
}
