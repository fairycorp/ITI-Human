using CK.Setup;
using CK.SqlServer.Setup;

namespace ITI.Human.Data
{
    [SqlTable("tSLPCreditStateUpdateTrack", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class SLPCreditStateUpdateTrackTable : SqlTable
    {
        void StObjConstruct(StorageLinkedProductUpdateTrackTable slpUpdateTrackTable)
        {
        }
    }
}
