using CK.DB.User.UserGitHub;

namespace ITI.Human.Data
{
    public interface IExtendedUserGitHubInfo : IUserGitHubInfo
    {
        string Name { get; set; }

        string AvatarUrl { get; set; }
    }
}
