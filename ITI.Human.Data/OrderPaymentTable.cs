using CK.Setup;
using CK.SqlServer.Setup;

namespace ITI.Human.Data
{
    [SqlTable("tOrderPayment", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class OrderPaymentTable : SqlTable
    {
        void StObjConstruct(OrderFinalDueTable oTable)
        {
        }
    }
}
