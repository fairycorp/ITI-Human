using CK.DB.Actor;
using CK.Setup;
using CK.SqlServer;
using CK.SqlServer.Setup;
using System;

namespace ITI.Human.Data
{
    [SqlTable("tOrder", Package = typeof(Package))]
    [Versions("1.0.0")]
    [SqlObjectItem("vOrderedProducts")]
    public abstract class OrderTable : SqlTable
    {
        void StObjConstruct(UserTable uTable)
        {
        }

        [SqlProcedure("sOrderCreate")]
        public abstract int Create(ISqlCallContext ctx, int actorId, int userId, DateTime creationDate);

        [SqlProcedure("sOrderUpdate")]
        public abstract bool Update(ISqlCallContext ctx, int actorId, int orderId, bool hasBeenEntirelyDelivered);
    }
}
