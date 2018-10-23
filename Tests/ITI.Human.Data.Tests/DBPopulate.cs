using CK.DB.Actor;
using CK.SqlServer;
using NUnit.Framework;
using System;
using static CK.Testing.DBSetupTestHelper;

namespace ITI.Human.Data.Tests
{
    [TestFixture]
    [Explicit]
    public class DBPopulate
    {
        [Test]
        public void Populate()
        {
            var productTable = CK.Core.StObjModelExtension.Obtain<ProductTable>(TestHelper.StObjMap.StObjs);
            var userTable = CK.Core.StObjModelExtension.Obtain<UserTable>(TestHelper.StObjMap.StObjs);
            var orderTable = CK.Core.StObjModelExtension.Obtain<OrderTable>(TestHelper.StObjMap.StObjs);
            var orderedProductTable = CK.Core.StObjModelExtension.Obtain<OrderedProductTable>(TestHelper.StObjMap.StObjs);

            using (var ctx = new SqlStandardCallContext())
            {
                productTable.Create(ctx, 0, "Kinder Bueno", "", 2);
                productTable.Create(ctx, 0, "Kinder Maxi", "", 1);
                productTable.Create(ctx, 0, "Kinder Country", "", 1);
                productTable.Create(ctx, 0, "Coca-Cola (33cl)", "", 1);
                productTable.Create(ctx, 0, "Coca-Cola (50cl)", "", 2);
                productTable.Create(ctx, 0, "Redbull (33cl)", "", 1);
                productTable.Create(ctx, 0, "Redbull (50cl)", "", 2);
                productTable.Create(ctx, 0, "Fanta (33cl)", "", 1);
                productTable.Create(ctx, 0, "Fanta (50cl)", "", 2);

                userTable.CreateUser(ctx, 1, "Geralt");
                userTable.CreateUser(ctx, 1, "Jasquier");

                orderTable.Create(ctx, 0, 1, DateTime.Now);
                orderTable.Create(ctx, 0, 2, DateTime.Now);

                orderedProductTable.Create(ctx, 0, 1, 8);
                orderedProductTable.Create(ctx, 0, 1, 2);
                orderedProductTable.Create(ctx, 0, 2, 3);
                orderedProductTable.Create(ctx, 0, 2, 3);
                orderedProductTable.Create(ctx, 0, 2, 7);
            }
        }
    }
}
