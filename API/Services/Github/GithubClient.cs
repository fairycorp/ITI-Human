using System;
using System.IO;
using System.Net;

namespace API.Services.Github
{
    public class GithubClient
    {
        public byte[] DownloadUserAvatar(string url)
        {
            using (WebClient wc = new WebClient())
            {
                byte[] downloadedData;
                downloadedData = wc.DownloadData(new Uri(url));

                using (MemoryStream stream = new MemoryStream(downloadedData))
                    return stream.ToArray();
            }
        }
    }
}
