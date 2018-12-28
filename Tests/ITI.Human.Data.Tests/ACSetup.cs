using CK.DB.Actor;
using CK.DB.User.UserPassword;
using CK.SqlServer;
using Dapper;
using NUnit.Framework;

using static CK.Testing.DBSetupTestHelper;

namespace ITI.Human.Data.Tests
{
    [TestFixture]
    public class ACSetup
    {
        [Test]
        public async System.Threading.Tasks.Task CreateAdminAsync()
        {
            var uTable = CK.Core.StObjModelExtension.Obtain<UserTable>(TestHelper.StObjMap.StObjs);
            var uPTable = CK.Core.StObjModelExtension.Obtain<UserPasswordTable>(TestHelper.StObjMap.StObjs);

            using (var ctx = new SqlStandardCallContext())
            {
                var doesUserExist = await ctx[uTable].Connection
                    .QueryFirstOrDefaultAsync<int>(
                        "SELECT UserId FROM CK.tUser WHERE UserName = @name",
                        new { name = "fairyfingers" }
                    );
                if (doesUserExist == 0)
                {
                    var userId = await uTable.CreateUserAsync(ctx, 1, "fairyfingers");
                    Assert.Greater(userId, 0);

                    var passwordResponse = await uPTable.CreateOrUpdatePasswordUserAsync(ctx, 1, userId, "access");
                }
            }
        }
    }
}
