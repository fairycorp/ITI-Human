using CK.SqlServer;
using FluentAssertions;
using NUnit.Framework;
using System;

using static CK.Testing.DBSetupTestHelper;

namespace ITI.Human.Data.Tests.OrderSystemTests
{
    [TestFixture]
    public class OrderedProductTests
    {
        [Test]
        public void givenNotAdmissibleArguments_whenCreateOrdoredProduct_shouldReturnSqlDetailedException()
        {
            var oProductTable = CK.Core.StObjModelExtension.Obtain<OrderedProductTable>(TestHelper.StObjMap.StObjs);

            using (var ctx = new SqlStandardCallContext())
            {
                oProductTable.Invoking(
                    sut => oProductTable.Create(ctx, 0, -1, -1)
                )
                .Should()
                .Throw<SqlDetailedException>();
            }
        }

        [Test]
        public void givenAdmissibleArguments_whenCreateOrderedProduct_shouldReturnCreatedOrderId()
        {
            var oProductTable = CK.Core.StObjModelExtension.Obtain<OrderedProductTable>(TestHelper.StObjMap.StObjs);

            using (var ctx = new SqlStandardCallContext())
            {
                var createdOrderedProduct = oProductTable.Create(ctx, 0, 0, 0);
                Assert.That(createdOrderedProduct, Is.GreaterThan(0));
            }
        }
    }
}
