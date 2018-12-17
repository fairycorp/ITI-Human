using Stall.Guard.System;

using static API.Services.Helper.ResultFactory;

namespace API.Services.Github
{
    public class GithubService
    {
        private GithubClient Client { get; set; }

        public GithubService(GithubClient client)
        {
            Client = client;
        }

        public GuardResult GuardedGetUserGithubAvatar(string url)
        {
            var result = Client.DownloadUserAvatar(url);
            if (result == null) return Failure("Error in download process.");

            return Success(result);
        }
    }
}
