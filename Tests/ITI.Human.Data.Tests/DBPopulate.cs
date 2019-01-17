using CK.DB.Actor;
using CK.SqlServer;
using Dapper;
using ITI.Human.ViewModels.Order;
using ITI.Human.ViewModels.Product.Ordered;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
        const string userName4 = "BobbyCarotte";
        const string userName5 = "Azsher";
        const string userName6 = "Legann";
        const string userName7 = "Fixesuarez";
        const string userName8 = "TheAmazing";
        const string userName9 = "Luthys";
        const string userName10 = "Surface";
        const string userName11 = "TakiTaki";
        const string projectName1 = "Stall";
        const string projectName2 = "Atome";

        [Test]
        public async Task Populate()
        {
            var oTable = (OrderTable)
                Initialize(Element.Order);

            var oFDTable = (OrderFinalDueTable)
                Initialize(Element.OrderFinalDue);

            var oPTable = (OrderedProductTable)
                Initialize(Element.OrderedProduct);

            var pTable = (ProductTable)
                Initialize(Element.Product);

            var pJTable = (ProjectTable)
                Initialize(Element.Project);

            var pMTable = (ProjectMemberTable)
                Initialize(Element.ProjectMember);

            var sMTable = (SchoolMemberTable)
                Initialize(Element.SchoolMember);

            var sTable = (StorageTable)
                Initialize(Element.Storage);

            var sLTable = (StorageLinkedProductTable)
                Initialize(Element.StorageLinkedProduct);

            var uTable = (UserTable)
                Initialize(Element.User);

            var uATable = (UserAvatarsTable)
                Initialize(Element.UserAvatars);

            var uBTable = (UserBalanceTable)
                Initialize(Element.UserBalance);

            var uDTable = (UserDetailsTable)
                Initialize(Element.UserDetails);

            using (var ctx = new SqlStandardCallContext())
            {
                object doesExist;

                // Checks on products.
                var productNames = new string[] {
                    productName1, productName2, productName3,
                    productName4, productName5, productName6,
                    productName7, productName8, productName9
                };

                foreach (var productName in productNames)
                {
                    doesExist = await GetElement(Element.Product, strIdentifier: productName);
                    if (doesExist != null) return;
                }

                // Creates products.
                var productId1 = await pTable.Create(ctx, 0, productName1, "Deux délicieuses barres chocolatées pralinées.",
                    "https://image.noelshack.com/fichiers/2019/03/1/1547463367-kbueno.png");
                var productId2 = await pTable.Create(ctx, 0, productName2, "L'original Kinder, prêt à être dévoré.",
                    "https://image.noelshack.com/fichiers/2019/03/1/1547463387-kmaxi.png");
                var productId3 = await pTable.Create(ctx, 0, productName3, "Certain(e)s disent qu'il est le meilleur en bouche.",
                    "https://image.noelshack.com/fichiers/2019/03/1/1547463367-kcountry.png");
                var productId4 = await pTable.Create(ctx, 0, productName4, "Cannette au format standard.",
                    "https://image.noelshack.com/fichiers/2019/03/1/1547463367-coca-33.png");
                var productId5 = await pTable.Create(ctx, 0, productName5, "Bouteille au format standard.",
                    "https://image.noelshack.com/fichiers/2019/03/1/1547463367-coca-50.png");
                var productId6 = await pTable.Create(ctx, 0, productName6, "Canette au format standard.",
                    "https://image.noelshack.com/fichiers/2019/03/1/1547463472-redbull-33.png");
                var productId7 = await pTable.Create(ctx, 0, productName7, "Canette au grand format.",
                    "https://image.noelshack.com/fichiers/2019/03/1/1547463387-redbull-50.png");
                var productId8 = await pTable.Create(ctx, 0, productName8, "Canette au format standard.",
                    "https://image.noelshack.com/fichiers/2019/03/1/1547463367-fanta-33.png");
                var productId9 = await pTable.Create(ctx, 0, productName9, "Bouteille au format standard.",
                    "https://image.noelshack.com/fichiers/2019/03/1/1547463367-fanta-50.png");


                // Checks on users.
                doesExist = await GetElement(Element.User, strIdentifier: userName1);
                if (doesExist != null) return;

                // Creates users (considered as school members) and their details.
                var userId1 = await uTable.CreateUserAsync(ctx, 1, userName1);
                var userId2 = await uTable.CreateUserAsync(ctx, 1, userName2);
                var userId3 = await uTable.CreateUserAsync(ctx, 1, userName3);
                var userId4 = await uTable.CreateUserAsync(ctx, 1, userName4);
                var userId5 = await uTable.CreateUserAsync(ctx, 1, userName5);
                var userId6 = await uTable.CreateUserAsync(ctx, 1, userName6);
                var userId7 = await uTable.CreateUserAsync(ctx, 1, userName7);
                var userId8 = await uTable.CreateUserAsync(ctx, 1, userName8);
                var userId9 = await uTable.CreateUserAsync(ctx, 1, userName9);
                var userId10 = await uTable.CreateUserAsync(ctx, 1, userName10);
                var userId11 = await uTable.CreateUserAsync(ctx, 1, userName11);

                var schoolMember1 = await sMTable.Create(ctx, 0, userId1, 3);
                var schoolMember2 = await sMTable.Create(ctx, 0, userId2, 3);
                var schoolMember3 = await sMTable.Create(ctx, 0, userId3, 3);
                var schoolMember4 = await sMTable.Create(ctx, 0, userId4, 3);
                var schoolMember5 = await sMTable.Create(ctx, 0, userId5, 3);
                var schoolMember6 = await sMTable.Create(ctx, 0, userId6, 3);
                var schoolMember7 = await sMTable.Create(ctx, 0, userId7, 3);
                var schoolMember8 = await sMTable.Create(ctx, 0, userId8, 3);
                var schoolMember9 = await sMTable.Create(ctx, 0, userId9, 3);
                var schoolMember10 = await sMTable.Create(ctx, 0, userId10, 3);
                var schoolMember11 = await sMTable.Create(ctx, 0, userId11, 3);

                var userDetails1 = await uDTable.Create(ctx, 0, userId1, "Charles", "Resini", new DateTime(1997, 10, 12));
                var userDetails2 = await uDTable.Create(ctx, 0, userId2, "Pierre", "Loderin", new DateTime(1995, 04, 19));
                var userDetails3 = await uDTable.Create(ctx, 0, userId3, "Emma", "Ruvol", new DateTime(1996, 07, 02));
                var userDetails4 = await uDTable.Create(ctx, 0, userId4, "Loïc", "Monard", new DateTime(1996, 07, 02));
                var userDetails5 = await uDTable.Create(ctx, 0, userId5, "Damien", "Gidon", new DateTime(1996, 07, 02));
                var userDetails6 = await uDTable.Create(ctx, 0, userId6, "Thibault", "Cam", new DateTime(1996, 07, 02));
                var userDetails7 = await uDTable.Create(ctx, 0, userId7, "François-Xavier", "Suarez", new DateTime(1996, 07, 02));
                var userDetails8 = await uDTable.Create(ctx, 0, userId8, "Hugo", "Thomas", new DateTime(1996, 07, 02));
                var userDetails9 = await uDTable.Create(ctx, 0, userId9, "Sébastien", "Martins", new DateTime(1996, 07, 02));
                var userDetails10 = await uDTable.Create(ctx, 0, userId10, "Hugo", "Loiseau", new DateTime(1996, 07, 02));
                var userDetails11 = await uDTable.Create(ctx, 0, userId11, "Abdelmadjid", "Sahki", new DateTime(1996, 07, 02));

                var userAvatar1 = await uATable.Create(ctx, 0, userId1, "https://image.noelshack.com/fichiers/2019/02/4/1547118763-geralt.png");
                var userAvatar2 = await uATable.Create(ctx, 0, userId2, "https://image.noelshack.com/fichiers/2019/02/4/1547118763-jaskier.png");
                var userAvatar3 = await uATable.Create(ctx, 0, userId3, "https://image.noelshack.com/fichiers/2019/02/4/1547118763-priscilla.png");
                var userAvatar4 = await uATable.Create(ctx, 0, userId4, "https://image.noelshack.com/fichiers/2019/02/4/1547126824-loic.jpg");
                var userAvatar5 = await uATable.Create(ctx, 0, userId5, "https://image.noelshack.com/fichiers/2019/02/4/1547126824-damien.jpg");
                var userAvatar6 = await uATable.Create(ctx, 0, userId6, "https://image.noelshack.com/fichiers/2019/02/4/1547126900-thibault.jpg");
                var userAvatar7 = await uATable.Create(ctx, 0, userId7, "https://image.noelshack.com/fichiers/2019/02/4/1547126910-suarez.jpg");
                var userAvatar8 = await uATable.Create(ctx, 0, userId8, "https://image.noelshack.com/fichiers/2019/02/4/1547126824-hugot.jpg");
                var userAvatar9 = await uATable.Create(ctx, 0, userId9, "https://image.noelshack.com/fichiers/2019/02/4/1547126945-seb.jpg");
                var userAvatar10 = await uATable.Create(ctx, 0, userId10, "https://image.noelshack.com/fichiers/2019/02/4/1547126824-hugo.jpg");
                var userAvatar11 = await uATable.Create(ctx, 0, userId11, "https://image.noelshack.com/fichiers/2019/02/4/1547126824-madjid.jpg");


                // Checks on projects.
                doesExist = await GetElement(Element.Project, strIdentifier: projectName1);
                if (doesExist != null) return;

                doesExist = await GetElement(Element.Project, strIdentifier: projectName2);
                if (doesExist != null) return;

                // Creates projects.
                var projectId1 = await pJTable.Create(ctx, 0, 1, 4, projectName1, "Project about something.", "Sample pitch.");
                var projectId2 = await pJTable.Create(ctx, 0, 1, 4, projectName2, "Loving great projecters.", "Greats pitch.");


                // Creates storages.
                var storageId1 = await sTable.Create(ctx, 0, projectId1);
                var storageId2 = await sTable.Create(ctx, 0, projectId2);

     
                // Creates storage linked products.
                var storageLinkedProductId1 = await sLTable.Create(ctx, 0, storageId1, productId1, 1, 20);
                var storageLinkedProductId2 = await sLTable.Create(ctx, 0, storageId1, productId2, 80, 40);
                var storageLinkedProductId4 = await sLTable.Create(ctx, 0, storageId1, productId4, 90, 40);
                var storageLinkedProductId5 = await sLTable.Create(ctx, 0, storageId1, productId8, 90, 30);
                var storageLinkedProductId6 = await sLTable.Create(ctx, 0, storageId1, productId9, 130, 20);

                var storageLinkedProductId7 = await sLTable.Create(ctx, 0, storageId2, productId3, 90, 40);
                var storageLinkedProductId8 = await sLTable.Create(ctx, 0, storageId2, productId1, 100, 40);
                var storageLinkedProductId9 = await sLTable.Create(ctx, 0, storageId2, productId5, 140, 30);
                var storageLinkedProductId10 = await sLTable.Create(ctx, 0, storageId2, productId6, 150, 20);
                var storageLinkedProductId11 = await sLTable.Create(ctx, 0, storageId2, productId7, 180, 40);


                // Creates project members.
                var projectMember1 = await pMTable.Create(ctx, 0, projectId1, 2, userId1);
                var projectMember2 = await pMTable.Create(ctx, 0, projectId1, 1, userId2);
                var projectMember3 = await pMTable.Create(ctx, 0, projectId1, 2, userId3);


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

                // -----------------------------------------------------------------

                var basicData = await ctx[oTable].Connection
                    .QueryAsync<BasicDataOrder>(
                        @"SELECT
                            *
                        FROM
                            ITIH.vOrders;"
                    );

                List<DetailedDataOrder> ordersList = new List<DetailedDataOrder>();
                foreach (var data in basicData)
                {
                    var detailedData = new DetailedDataOrder
                    {
                        Info = data,
                        Products = await ctx[oTable].Connection
                        .QueryAsync<DetailedDataOrderedProduct>(
                            @"SELECT
                                *
                            FROM
                                ITIH.vOrderedProducts v
                            WHERE
                                v.OrderId = @id;",
                            new { id = data.OrderId }
                        )
                    };
                    detailedData.Info.Total = CalculateOrderTotal(detailedData.Products);
                    ordersList.Add(detailedData);
                }


                // -----------------------------------------------------------------

                foreach (var order in ordersList)
                {
                    var total = 0;
                    foreach (var product in order.Products)
                    {
                        total += product.UnitPrice;
                    }
                    await oFDTable.Create(ctx, 0, order.Info.OrderId, total, 0);
                }
            }
        }

        [Test]
        public async Task SetupPoneyProject()
        {
            var oCTable = (OrderCreditTable)
                Initialize(Element.OrderCredit);

            var uTable = (UserTable)
                Initialize(Element.User);

            var uBTable = (UserBalanceTable)
                Initialize(Element.UserBalance);

            var pVTable = (ProjectVotesTable)
                Initialize(Element.ProjectVotes);

            async Task<int> GetUser(ISqlCallContext ctx, string userName)
            {
                return await ctx[uTable].Connection
                .QueryFirstOrDefaultAsync<int>(
                    "SELECT UserId FROM CK.tUser WHERE UserName = @nm;",
                    new { nm = userName }
                );
            };

            async Task<int> GetProject(ISqlCallContext ctx, string projectName)
            {
                return await ctx[uTable].Connection
                .QueryFirstOrDefaultAsync<int>(
                    "SELECT ProjectId FROM ITIH.tProject WHERE [Name] = @nm;",
                    new { nm = projectName }
                );
            };

            async Task<int> GetBalance(ISqlCallContext ctx, int projectId)
            {
                return await ctx[uBTable].Connection
                .QueryFirstOrDefaultAsync<int>(
                    "SELECT UserBalanceId FROM ITIH.tUserBalance WHERE ProjectId = @id;",
                    new { id = projectId }
                );
            }

            using (var ctx = new SqlStandardCallContext())
            {
                var doesProjectExist = await GetProject(ctx, "Poney");
                if (doesProjectExist == 0) return;

                var hasCreationAlreadyBeenMade =
                    await GetBalance(ctx, doesProjectExist);
                if (hasCreationAlreadyBeenMade > 0) return;

                var userId1 = await GetUser(ctx, "fairyfingers");
                var userId2 = await GetUser(ctx, "BobbyCarotte");
                var userId3 = await GetUser(ctx, "Azsher");
                var userId4 = await GetUser(ctx, "Legann");
                var projectId = await GetProject(ctx, "Poney");

                var projectNoteId1 = await pVTable.Create(ctx, 0, projectId, userId1, 4);
                var projectNoteId2 = await pVTable.Create(ctx, 0, projectId, userId2, 3);
                var projectNoteId3 = await pVTable.Create(ctx, 0, projectId, userId3, 3);
                var projectNoteId4 = await pVTable.Create(ctx, 0, projectId, userId4, 5);

                var uBalanceId1 = await uBTable.Create(ctx, 0, userId1, projectId);
                var uBalanceId2 = await uBTable.Create(ctx, 0, userId2, projectId);
                var uBalanceId3 = await uBTable.Create(ctx, 0, userId3, projectId);

                await oCTable.Create(ctx, 0, projectId, userId1, 40, new DateTime(2018, 12, 12));
                await oCTable.Create(ctx, 0, projectId, userId1, 120, new DateTime(2019, 01, 4));
                await oCTable.Create(ctx, 0, projectId, userId1, 100, new DateTime(2018, 01, 7));
                await oCTable.Create(ctx, 0, projectId, userId1, 90, new DateTime(2018, 01, 14));

                await oCTable.Create(ctx, 0, projectId, userId2, 160, new DateTime(2018, 11, 20));
                await oCTable.Create(ctx, 0, projectId, userId2, 70, new DateTime(2018, 12, 19));

                await uBTable.Update(ctx, 0, uBalanceId1, -350);
                await uBTable.Update(ctx, 0, uBalanceId2, -230);
                await uBTable.Update(ctx, 0, uBalanceId3, 410);
            }
        }

        static int CalculateOrderTotal(IEnumerable<DetailedDataOrderedProduct> products)
        {
            int total = 0;
            foreach (var product in products)
            {
                total += product.UnitPrice;
            }
            return total;
        }

        /// <summary>
        /// Defines what element type one's using.
        /// </summary>
        private enum Element
        {
            Order,
            OrderCredit,
            OrderFinalDue,
            OrderedProduct,
            Product,
            Project,
            ProjectMember,
            ProjectVotes,
            SchoolMember,
            Storage,
            StorageLinkedProduct,
            User,
            UserAvatars,
            UserBalance,
            UserDetails
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

                    case (Element.OrderCredit):
                        table = (OrderTable)Initialize(Element.OrderCredit);
                        tableName = "ITIH.tOrderCredit"; fieldName = "OrderCreditId";
                        break;

                    case (Element.OrderFinalDue):
                        table = (OrderFinalDueTable)Initialize(Element.OrderFinalDue);
                        tableName = "ITIH.tOrderFinalDue"; fieldName = "OrderFinalDueId";
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

                    case (Element.ProjectMember):
                        table = (ProjectMemberTable)Initialize(Element.ProjectMember);
                        tableName = "ITIH.tProjectMember"; fieldName = "ProjectMemberId";
                        break;

                    case (Element.ProjectVotes):
                        table = (ProjectVotesTable)Initialize(Element.ProjectVotes);
                        tableName = "ITIH.tProjectVotes"; fieldName = "ProjectVoteId";
                        break;

                    case (Element.SchoolMember):
                        table = (SchoolMemberTable)Initialize(Element.SchoolMember);
                        tableName = "ITIH.tSchoolMember"; fieldName = "SchoolMemberId";
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

                    case (Element.UserBalance):
                        table = (UserBalanceTable)Initialize(Element.UserBalance);
                        tableName = "ITIH.tUserBalance"; fieldName = "UserBalanceId";
                        break;

                    case (Element.UserAvatars):
                        table = (UserAvatarsTable)Initialize(Element.UserAvatars);
                        tableName = "ITIH.tUserAvatars"; fieldName = "UserAvatarId";
                        break;

                    case (Element.UserDetails):
                        table = (UserDetailsTable)Initialize(Element.UserDetails);
                        tableName = "ITIH.tUserDetails"; fieldName = "UserDetailsId";
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

                case (Element.OrderCredit):
                    return CK.Core.StObjModelExtension.Obtain<OrderCreditTable>(TestHelper.StObjMap.StObjs);

                case (Element.OrderFinalDue):
                    return CK.Core.StObjModelExtension.Obtain<OrderFinalDueTable>(TestHelper.StObjMap.StObjs);

                case (Element.OrderedProduct):
                    return CK.Core.StObjModelExtension.Obtain<OrderedProductTable>(TestHelper.StObjMap.StObjs);

                case (Element.Product):
                    return CK.Core.StObjModelExtension.Obtain<ProductTable>(TestHelper.StObjMap.StObjs);

                case (Element.Project):
                    return CK.Core.StObjModelExtension.Obtain<ProjectTable>(TestHelper.StObjMap.StObjs);

                case (Element.ProjectMember):
                    return CK.Core.StObjModelExtension.Obtain<ProjectMemberTable>(TestHelper.StObjMap.StObjs);

                case (Element.ProjectVotes):
                    return CK.Core.StObjModelExtension.Obtain<ProjectVotesTable>(TestHelper.StObjMap.StObjs);

                case (Element.SchoolMember):
                    return CK.Core.StObjModelExtension.Obtain<SchoolMemberTable>(TestHelper.StObjMap.StObjs);

                case (Element.Storage):
                    return CK.Core.StObjModelExtension.Obtain<StorageTable>(TestHelper.StObjMap.StObjs);

                case (Element.StorageLinkedProduct):
                    return CK.Core.StObjModelExtension.Obtain<StorageLinkedProductTable>(TestHelper.StObjMap.StObjs);

                case (Element.User):
                    return CK.Core.StObjModelExtension.Obtain<UserTable>(TestHelper.StObjMap.StObjs);

                case (Element.UserBalance):
                    return CK.Core.StObjModelExtension.Obtain<UserBalanceTable>(TestHelper.StObjMap.StObjs);

                case (Element.UserAvatars):
                    return CK.Core.StObjModelExtension.Obtain<UserAvatarsTable>(TestHelper.StObjMap.StObjs);

                case (Element.UserDetails):
                    return CK.Core.StObjModelExtension.Obtain<UserDetailsTable>(TestHelper.StObjMap.StObjs);

                default:
                    return new object();
            }
        }
    }
}
