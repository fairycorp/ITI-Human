using CK.SqlServer;
using FluentAssertions;
using NUnit.Framework;
using System;

using static CK.Testing.DBSetupTestHelper;

namespace ITI.Human.Data.Tests.OrderSystemTests
{
    [TestFixture]
    public class OrderTests
    {
        [Test]
        public void givenAdmissibleArguments_whenCreateOrder_shouldReturnCreatedOrderId()
        {
            var oTable = CK.Core.StObjModelExtension.Obtain<OrderTable>(TestHelper.StObjMap.StObjs);

            using (var ctx = new SqlStandardCallContext())
            {
                var createdOrder = oTable.Create(ctx, 0, 0, DateTime.Now);
                Assert.That(createdOrder, Is.GreaterThan(0));
            }
        }
    }
}
