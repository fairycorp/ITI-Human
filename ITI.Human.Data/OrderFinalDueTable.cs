using CK.Setup;
using CK.SqlServer.Setup;

namespace ITI.Human.Data
{
    [SqlTable("tOrderFinalDue", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class OrderFinalDueTable : SqlTable
    {
        void StObjConstruct(OrderTable oTable)
        {
        }
    }
}
