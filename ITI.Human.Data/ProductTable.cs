using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;

namespace ITI.Human.Data
{
    [SqlTable("tProduct", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class ProductTable : SqlTable
    {
        void StObjConstruct()
        {
        }

        [SqlProcedure("sProductCreate")]
        public abstract int Create(ISqlCallContext ctx, int actorId, string name, string desc, double price);
    }
}
