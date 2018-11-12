using CK.Setup;
using CK.SqlServer.Setup;

namespace ITI.Human.Data
{
    [SqlTable("tStorageLinkedProductUpdateTrack", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class StorageLinkedProductUpdateTrackTable : SqlTable
    {
        void StObjConstruct(UpdateTrackTable uTrackTable, StorageLinkedProductTable slpTable)
        {
        }
    }
}
