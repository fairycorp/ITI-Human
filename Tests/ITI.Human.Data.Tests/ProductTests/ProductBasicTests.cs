using CK.SqlServer;
using FluentAssertions;
using NUnit.Framework;

using static CK.Testing.DBSetupTestHelper;


namespace ITI.Human.Data.Tests.ProductTests
{
    [TestFixture]
    public class ProductBasicTests
    {
        [Test]
        public void GivenAdmissibleArguments_whenCreatingProduct_shouldNotThrowException()
        {
            var p = CK.Core.StObjModelExtension.Obtain<ProductTable>(TestHelper.StObjMap.StObjs);

            using (var ctx = new SqlStandardCallContext())
            {
                // Creation process.
                int created = 0;
                p.Invoking(
                    sut =>
                    {
                        created = p.Create(ctx, 0, string.Empty, string.Empty, 0.3);
                    }
                ).Should().NotThrow();
            }
        }
    }
}
