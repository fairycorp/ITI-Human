using CK.Setup;
using CK.SqlServer.Setup;

namespace ITI.Human.Data
{
    [SqlPackage(
    ResourcePath = "Res",
    Schema = "ITIH",
    Database = typeof(SqlDefaultDatabase),
    ResourceType = typeof(Package)),
    Versions("1.0.0")]
    public abstract class Package : SqlPackage
    {
        void StObjConstruct(CK.DB.User.UserGitHub.Package githubUserPackage)
        {

        }
    }
}
