using CK.DB.User.UserGitHub;

namespace Fork.Data
{
    public interface IExtendedUserGitHubInfo : IUserGitHubInfo
    {
        string Name { get; set; }

        string AvatarUrl { get; set; }
    }
}
