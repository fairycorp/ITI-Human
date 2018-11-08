using CK.SqlServer;
using FluentAssertions;
using NUnit.Framework;
using System;

using static CK.Testing.DBSetupTestHelper;

namespace ITI.Human.Data.Tests.ProjectTests
{
    [TestFixture]
    public class ProjectBasicTests
    {
        // This test also covers deletion process.
        [Test]
        public void GivenAdmissibleArguments_whenCreatingProject_shouldNotThrowException()
        {
            var p = CK.Core.StObjModelExtension.Obtain<ProjectTable>(TestHelper.StObjMap.StObjs);

            using (var ctx = new SqlStandardCallContext())
            {
                // Creation process.
                int created = 0;
                p.Invoking(
                    async sut => 
                    {
                        created = await p.Create(ctx, 0, 0, 0, "N", "N", "N");
                    }
                ).Should().NotThrow();

                // Deletion process.
                if (created != 0) p.Delete(ctx, 0, created); else throw new InvalidOperationException();
            }
        }
    }
}
