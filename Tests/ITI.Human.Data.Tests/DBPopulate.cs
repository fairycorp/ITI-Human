using API.ViewModels.Product;
using CK.DB.Actor;
using CK.SqlServer;
using Dapper;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using static CK.Testing.DBSetupTestHelper;

namespace ITI.Human.Data.Tests
{
    [TestFixture]
    public class DBPopulate
    {
        // Consts.
        const string productName1 = "Kinder Bueno";
        const string productName2 = "Kinder Maxi";
        const string productName3 = "Kinder Country";
        const string productName4 = "Coca-Cola (33cl)";
        const string productName5 = "Coca-Cola (50cl)";
        const string productName6 = "Redbull (33cl)";
        const string productName7 = "Redbull (50cl)";
        const string productName8 = "Fanta (33cl)";
        const string productName9 = "Fanta (50cl)";
        const string userName1 = "Geralt";
        const string userName2 = "Jasquier";

        [Test]
        public async Task Populate()
        {
            var productTable = CK.Core.StObjModelExtension.Obtain<ProductTable>(TestHelper.StObjMap.StObjs);
            var userTable = CK.Core.StObjModelExtension.Obtain<UserTable>(TestHelper.StObjMap.StObjs);
            var orderTable = CK.Core.StObjModelExtension.Obtain<OrderTable>(TestHelper.StObjMap.StObjs);
            var orderedProductTable = CK.Core.StObjModelExtension.Obtain<OrderedProductTable>(TestHelper.StObjMap.StObjs);

            using (var ctx = new SqlStandardCallContext())
            {
                // CHECKS :
                // On Products.
                string[] productNames = new [] 
                {
                    productName1, productName2, productName3,
                    productName4, productName5, productName6,
                    productName7, productName8, productName9
                };

                foreach (var name in productNames)
                {
                    var currentObj = await ctx[productTable].Connection
                        .QueryFirstOrDefaultAsync<BasicDataProduct>(
                            "SELECT ProductId FROM ITIH.tProduct WHERE [Name] = @nm;",
                            new { nm = name }
                    );
                    if (currentObj != null && !string.IsNullOrWhiteSpace(currentObj.Name)) return;
                }

                // On Users.
                string[] userNames = new[] { userName1, userName2 };
                foreach (var name in userNames)
                {
                    var currentName = await ctx[userTable].Connection
                        .QueryFirstOrDefaultAsync<string>(
                            "SELECT UserName FROM CK.tUser WHERE UserName = @nm;",
                            new { nm = name }
                        );
                    if (currentName != null && !string.IsNullOrWhiteSpace(currentName)) return;
                }
                // END OF CHECKS.


                // Insertion.
                var product1 = await productTable.Create(ctx, 0, productName1, "", 2);
                var product2 = await productTable.Create(ctx, 0, productName2, "", 1);
                var product3 = await productTable.Create(ctx, 0, productName3, "", 1);
                var product4 = await productTable.Create(ctx, 0, productName4, "", 1);
                var product5 = await productTable.Create(ctx, 0, productName5, "", 2);
                var product6 = await productTable.Create(ctx, 0, productName6, "", 1);
                var product7 = await productTable.Create(ctx, 0, productName7, "", 2);
                var product8 = await productTable.Create(ctx, 0, productName8, "", 1);
                var product9 = await productTable.Create(ctx, 0, productName9, "", 2);

                var user1 = userTable.CreateUser(ctx, 1, "Geralt");
                var user2 = userTable.CreateUser(ctx, 1, "Jasquier");

                var order1 = await orderTable.Create(ctx, 0, user1, DateTime.Now);
                var order2 = await orderTable.Create(ctx, 0, user2, DateTime.Now);

                await orderedProductTable.Create(ctx, 0, order1, product2, 1);
                await orderedProductTable.Create(ctx, 0, order1, product9, 1);
                await orderedProductTable.Create(ctx, 0, order2, product3, 2);
                await orderedProductTable.Create(ctx, 0, order2, product4, 1);
                await orderedProductTable.Create(ctx, 0, order2, product1, 1);
            }
        }
    }
}
