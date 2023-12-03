using System;
using System.Linq;

namespace GithubNet
{
    public class RepositoryBase
    {
        public RepositoryBase(string url, string mainLanguage, int totalStars, int totalForks, string username, string repositoryName, string description)
        {
            Url = url;
            MainLanguage = mainLanguage;
            TotalStars = totalStars;
            TotalForks = totalForks;
            Username = username;
            RepositoryName = repositoryName;
            Description = description;
        }

        public string Url { get; set; }

        public string MainLanguage { get; set; }

        public int TotalStars { get; set; }

        public int TotalForks { get; set; }

        public string Username { get; set; }

        public string RepositoryName { get; set; }

        public string Description { get; set; }

        public virtual string GetReadMeUrl(string branch, string readmeFile = "README.md")
        {
            return $"https://raw.githubusercontent.com/{this.Username}/{this.RepositoryName}/{branch}/{readmeFile}";
        }

        public async Task<(string ReadmeContent, string Url)> GetReadmeAuto()
        {
            (string Content, string Url) readme = await UtilManager.FindReadme(this.Username, this.RepositoryName);
            return readme;
        }

        public string GetStarsUrl()
        {
            return $"https://github.com/{this.Username}/{this.RepositoryName}/stargazers";
        }

        public string ParseCreatedByText()
        {
            return $"{this.RepositoryName} created by {this.Username}";
        }

        public string GetForksUrl()
        {
            return $"https://github.com/{this.Username}/{this.RepositoryName}/forks";
        }

        public override string ToString()
        {
            return $"{{{nameof(Url)}={Url}, {nameof(MainLanguage)}={MainLanguage}, {nameof(TotalStars)}={TotalStars.ToString()}, {nameof(TotalForks)}={TotalForks.ToString()}, {nameof(Username)}={Username}, {nameof(RepositoryName)}={RepositoryName}, {nameof(Description)}={Description}}}";
        }
    }
}
