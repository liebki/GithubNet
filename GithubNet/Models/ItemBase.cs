namespace GithubNet.Models
{
    public class ItemBase
    {
        public ItemBase(string user, string respositoryLink, string respositoryName, string description, int totalStars, int totalForks, bool hasProjectUrl, string projectUrl, bool hasTopics, string[] topics)
        {
            User = user;
            RespositoryLink = respositoryLink;
            RespositoryName = respositoryName;
            Description = description;
            TotalStars = totalStars;
            TotalForks = totalForks;
            HasProjectUrl = hasProjectUrl;
            ProjectUrl = projectUrl;
            HasTopics = hasTopics;
            Topics = topics;
        }

        public string User { get; set; }

        public string RespositoryLink { get; set; }

        public string RespositoryName { get; set; }

        public string Description { get; set; }

        public int TotalStars { get; set; }

        public int TotalForks { get; set; }

        public bool HasProjectUrl { get; set; }

        public string ProjectUrl { get; set; }

        public bool HasTopics { get; set; }

        public string[] Topics { get; set; }


        public string GetLastCommitUrl(string branch)
        {
            return $"https://github.com/{this.User}/{this.RespositoryName}/commit/{branch}";
        }

        public string ParseCreatedByText()
        {
            return $"{this.RespositoryName} created by {this.User}";
        }

        public string GetReadMeUrl(string branch)
        {
            return $"https://raw.githubusercontent.com/{this.User}/{this.RespositoryName}/{branch}/README.md";
        }

        public string GetForksUrl()
        {
            return $"https://github.com/{this.User}/{this.RespositoryName}/forks";
        }

        public string GetStarsUrl()
        {
            return $"https://github.com/{this.User}/{this.RespositoryName}/stargazers";
        }
    }
}