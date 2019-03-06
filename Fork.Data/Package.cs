using CK.Setup;
using CK.SqlServer.Setup;

namespace Fork.Data
{
    [SqlPackage(
    ResourcePath = "Res",
    Schema = "FRK",
    Database = typeof(SqlDefaultDatabase),
    ResourceType = typeof(Package)),
    Versions("1.0.0")]
    public abstract class Package : SqlPackage
    {
        void StObjConstruct(
            CK.DB.User.UserPassword.Package userPasswordPackage,
            CK.DB.User.UserGitHub.Package githubUserPackage
        )
        {

        }
    }
}
