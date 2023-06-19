namespace GithubNet.Models
{
    public class TrendItem : ItemBase
    {

        public TrendItem(string programminglanguage, bool hasDetails, bool isArchived, string lastCommitTime, string lastCommitUrl, string user, string respositoryLink, string respositoryName, string description, int totalStars, int totalForks, bool hasProjectUrl, string projectUrl, bool hasTopics, string[] topics) : base(user, respositoryLink, respositoryName, description, totalStars, totalForks, hasProjectUrl, projectUrl, hasTopics, topics)
        {
            Programminglanguage = programminglanguage;
            HasDetails = hasDetails;
            IsArchived = isArchived;
            LastCommitTime = lastCommitTime;
            LastCommitUrl = lastCommitUrl;
            base.User = user;
            base.RespositoryLink = respositoryLink;
            base.RespositoryName = respositoryName;
            base.Description = description;
            base.TotalStars = totalStars;
            base.TotalForks = totalForks;
            base.HasProjectUrl = hasProjectUrl;
            base.ProjectUrl = projectUrl;
            base.HasTopics = hasTopics;
            base.Topics = topics;
        }

        public string Programminglanguage { get; set; }

        public bool HasDetails { get; set; }

        public bool IsArchived { get; set; }

        public string LastCommitTime { get; set; }

        public string LastCommitUrl { get; set; }

    }
}