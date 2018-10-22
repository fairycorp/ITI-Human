using CK.SqlServer;
using FluentAssertions;
using NUnit.Framework;

using static CK.Testing.DBSetupTestHelper;

namespace ITI.Human.Data.Tests.OrderSystemTests
{
    [TestFixture]
    public class ProductTests
    {
        [Test]
        public void givenNotAdmissibleArguments_whenCreateProduct_shouldThrowSqlDetailedException()
        {
            var pTable = CK.Core.StObjModelExtension.Obtain<ProductTable>(TestHelper.StObjMap.StObjs);

            using (var ctx = new SqlStandardCallContext())
            {
                pTable.Invoking(
                    sut => pTable.Create(ctx, 0, null, null, 0)
                )
                .Should()
                .Throw<SqlDetailedException>();
            }
        }

        [Test]
        public void givenAdmissibleArguments_whenCreateProduct_shouldReturnCreatedProductId()
        {
            var pTable = CK.Core.StObjModelExtension.Obtain<ProductTable>(TestHelper.StObjMap.StObjs);

            using (var ctx = new SqlStandardCallContext())
            {
                var createdProduct = pTable.Create(ctx, 0, "Kinder Maxi", "A delicious chocolate bar.", 1);
                Assert.That(createdProduct, Is.GreaterThan(0));
            }
        }
    }
}
