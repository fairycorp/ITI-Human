using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System.Threading.Tasks;

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
        public abstract Task<int> Create(ISqlCallContext ctx, int actorId, string name, string desc, string url = null);

        [SqlProcedure("sProductUpdate")]
        public abstract Task<bool> Update(ISqlCallContext ctx, int actorId, int productId, string name, string desc, string url);
    }
}
