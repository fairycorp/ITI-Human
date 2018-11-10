﻿using CK.DB.Actor;
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
        const string userName3 = "Priscilla";
        const string projectName1 = "Stall";
        const string projectName2 = "Atome";

        [Test]
        public async Task Populate()
        {
            var oTable = (OrderTable)
                Initialize(Element.Order);

            var oPTable = (OrderedProductTable)
                Initialize(Element.OrderedProduct);

            var pTable = (ProductTable)
                Initialize(Element.Product);

            var pJTable = (ProjectTable)
                Initialize(Element.Project);

            var sTable = (StorageTable)
                Initialize(Element.Storage);

            var sLTable = (StorageLinkedProductTable)
                Initialize(Element.StorageLinkedProduct);

            var uTable = (UserTable)
                Initialize(Element.User);

            using (var ctx = new SqlStandardCallContext())
            {
                object doesExist;

                // Checks on products.
                doesExist = await GetElement(Element.Product, strIdentifier: productName1);
                if (doesExist != null) return;

                doesExist = await GetElement(Element.Product, strIdentifier: productName2);
                if (doesExist != null) return;

                doesExist = await GetElement(Element.Product, strIdentifier: productName3);
                if (doesExist != null) return;

                doesExist = await GetElement(Element.Product, strIdentifier: productName4);
                if (doesExist != null) return;

                doesExist = await GetElement(Element.Product, strIdentifier: productName5);
                if (doesExist != null) return;

                doesExist = await GetElement(Element.Product, strIdentifier: productName6);
                if (doesExist != null) return;

                doesExist = await GetElement(Element.Product, strIdentifier: productName7);
                if (doesExist != null) return;

                doesExist = await GetElement(Element.Product, strIdentifier: productName8);
                if (doesExist != null) return;

                doesExist = await GetElement(Element.Product, strIdentifier: productName9);
                if (doesExist != null) return;


                // Creates products.
                var productId1 = await pTable.Create(ctx, 0, productName1, string.Empty);
                var productId2 = await pTable.Create(ctx, 0, productName2, string.Empty);
                var productId3 = await pTable.Create(ctx, 0, productName3, string.Empty);
                var productId4 = await pTable.Create(ctx, 0, productName4, string.Empty);
                var productId5 = await pTable.Create(ctx, 0, productName5, string.Empty);
                var productId6 = await pTable.Create(ctx, 0, productName6, string.Empty);
                var productId7 = await pTable.Create(ctx, 0, productName7, string.Empty);
                var productId8 = await pTable.Create(ctx, 0, productName8, string.Empty);
                var productId9 = await pTable.Create(ctx, 0, productName9, string.Empty);


                // Checks on users.
                doesExist = await GetElement(Element.User, strIdentifier: userName1);
                if (doesExist != null) return;

                doesExist = await GetElement(Element.User, strIdentifier: userName2);
                if (doesExist != null) return;

                doesExist = await GetElement(Element.User, strIdentifier: userName3);
                if (doesExist != null) return;

                // Creates users.
                var userId1 = await uTable.CreateUserAsync(ctx, 1, userName1);
                var userId2 = await uTable.CreateUserAsync(ctx, 1, userName2);
                var userId3 = await uTable.CreateUserAsync(ctx, 1, userName3);


                // Checks on projects.
                doesExist = await GetElement(Element.Project, strIdentifier: projectName1);
                if (doesExist != null) return;

                doesExist = await GetElement(Element.Project, strIdentifier: projectName2);
                if (doesExist != null) return;

                // Creates projects.
                var projectId1 = await pJTable.Create(ctx, 0, 1, 2, projectName1, "Project about something.", "Sample pitch.");
                var projectId2 = await pJTable.Create(ctx, 0, 1, 3, projectName2, "Loving great projecters.", "Greats pitch.");


                // Creates storages.
                var storageId1 = await sTable.Create(ctx, 0, projectId1);
                var storageId2 = await sTable.Create(ctx, 0, projectId2);

     
                // Creates storage linked products.
                var storageLinkedProductId1 = await sLTable.Create(ctx, 0, storageId1, productId1, 1, 20);
                var storageLinkedProductId2 = await sLTable.Create(ctx, 0, storageId1, productId2, 0.8, 40);
                var storageLinkedProductId4 = await sLTable.Create(ctx, 0, storageId1, productId4, 0.9, 40);
                var storageLinkedProductId5 = await sLTable.Create(ctx, 0, storageId1, productId8, 0.9, 30);
                var storageLinkedProductId6 = await sLTable.Create(ctx, 0, storageId1, productId9, 1.3, 20);

                var storageLinkedProductId7 = await sLTable.Create(ctx, 0, storageId2, productId3, 0.9, 40);
                var storageLinkedProductId8 = await sLTable.Create(ctx, 0, storageId2, productId3, 0.9, 40);
                var storageLinkedProductId9 = await sLTable.Create(ctx, 0, storageId2, productId5, 1.4, 30);
                var storageLinkedProductId10 = await sLTable.Create(ctx, 0, storageId2, productId6, 1.5, 20);
                var storageLinkedProductId11 = await sLTable.Create(ctx, 0, storageId2, productId7, 1.8, 40);


                // Creates orders.
                var orderId1 = await oTable.Create(ctx, 0, storageId1, userId1, 3, DateTime.Now);
                var orderId2 = await oTable.Create(ctx, 0, storageId2, userId2, 5, DateTime.Now);
                var orderId3 = await oTable.Create(ctx, 0, storageId2, userId3, 0, DateTime.Now);


                // Creates ordered products.
                await oPTable.Create(ctx, 0, orderId1, storageLinkedProductId2, 4);
                await oPTable.Create(ctx, 0, orderId1, storageLinkedProductId4, 2);
                await oPTable.Create(ctx, 0, orderId2, storageLinkedProductId11, 1);
                await oPTable.Create(ctx, 0, orderId3, storageLinkedProductId7, 2);
                await oPTable.Create(ctx, 0, orderId3, storageLinkedProductId10, 1);
            }
        }

        /// <summary>
        /// Defines what element type one's using.
        /// </summary>
        private enum Element
        {
            Order,
            OrderedProduct,
            Product,
            Project,
            Storage,
            StorageLinkedProduct,
            User
        }

        /// <summary>
        /// Gets a specific element from database, by its id or its name.
        /// </summary>
        /// <param name="type">Mentionned typed element.</param>
        /// <param name="intIdentifier">Optional int identifier (ex: UserId).</param>
        /// <param name="strIdentifier">Optional str identifier.</param>
        private async static Task<object> GetElement(Element type, int intIdentifier = 0, string strIdentifier = "")
        {
            // Ends the function IF identifiers have both kept their default values.
            if (intIdentifier == 0 && strIdentifier == "") return null;

            string getter = "SELECT {1} FROM {0} WHERE {1} = {2};";

            using (var ctx = new SqlStandardCallContext())
            {
                object table = null;
                string tableName = "";
                string fieldName = "";
                string idName = "@Id";
                string nmName = "@Name";

                switch (type)
                {
                    case (Element.Order):
                        table = (OrderTable)Initialize(Element.Order);
                        tableName = "ITIH.tOrder"; fieldName = "OrderId";
                        break;

                    case (Element.OrderedProduct):
                        table = (OrderedProductTable)Initialize(Element.OrderedProduct);
                        tableName = "ITIH.tOrderedProduct"; fieldName = "OrderedProductId";
                        break;

                    case (Element.Product):
                        table = (ProductTable)Initialize(Element.Product);
                        tableName = "ITIH.tProduct"; fieldName = "[Name]";
                        break;

                    case (Element.Project):
                        table = (ProjectTable)Initialize(Element.Project);
                        tableName = "ITIH.tProject"; fieldName = "[Name]";
                        break;

                    case (Element.Storage):
                        table = (StorageTable)Initialize(Element.Storage);
                        tableName = "ITIH.tStorage"; fieldName = "StorageId";
                        break;

                    case (Element.StorageLinkedProduct):
                        table = (StorageLinkedProductTable)Initialize(Element.StorageLinkedProduct);
                        tableName = "ITIH.tStorageLinkedProduct"; fieldName = "StorageLinkedProductId";
                        break;

                    case (Element.User):
                        table = (UserTable)Initialize(Element.User);
                        tableName = "CK.tUser"; fieldName = "UserName";
                        break;
                }

                if (table != null)
                {
                    string formattedGetter;

                    if (intIdentifier != 0 && strIdentifier == "")
                    {
                        formattedGetter = string.Format(getter, tableName, fieldName, idName);

                        return await ctx[(ISqlConnectionStringProvider)table].Connection.QueryFirstOrDefaultAsync<int>
                        (
                            formattedGetter,
                            new { Id = intIdentifier }
                        );
                    }

                    if (strIdentifier != "" && intIdentifier == 0)
                    {
                        formattedGetter = string.Format(getter, tableName, fieldName, nmName);

                        return await ctx[(ISqlConnectionStringProvider)table].Connection.QueryFirstOrDefaultAsync<string>
                        (
                            formattedGetter,
                            new { Name = strIdentifier }
                        );
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Initializes the matching table according to the given element type.
        /// </summary>
        /// <param name="type">Mentionned typed element.</param>
        /// <returns></returns>
        private static object Initialize(Element type)
        {
            switch (type)
            {
                case (Element.Order):
                    return CK.Core.StObjModelExtension.Obtain<OrderTable>(TestHelper.StObjMap.StObjs);

                case (Element.OrderedProduct):
                    return CK.Core.StObjModelExtension.Obtain<OrderedProductTable>(TestHelper.StObjMap.StObjs);

                case (Element.Product):
                    return CK.Core.StObjModelExtension.Obtain<ProductTable>(TestHelper.StObjMap.StObjs);

                case (Element.Project):
                    return CK.Core.StObjModelExtension.Obtain<ProjectTable>(TestHelper.StObjMap.StObjs);

                case (Element.Storage):
                    return CK.Core.StObjModelExtension.Obtain<StorageTable>(TestHelper.StObjMap.StObjs);

                case (Element.StorageLinkedProduct):
                    return CK.Core.StObjModelExtension.Obtain<StorageLinkedProductTable>(TestHelper.StObjMap.StObjs);

                case (Element.User):
                    return CK.Core.StObjModelExtension.Obtain<UserTable>(TestHelper.StObjMap.StObjs);

                default:
                    return new object();
            }
        }
    }
}
