namespace GithubNet.Models
{
    public class Repository : ItemBase
    {
        public Repository(int openIssuesNumber, int openPullRequestsNumber, int totalCommitsNumber, int totalContributorsNumber, string user, string respositoryLink, string respositoryName, string description, int totalStars, int totalForks, bool hasProjectUrl, string projectUrl, bool hasTopics, string[] topics) : base(user, respositoryLink, respositoryName, description, totalStars, totalForks, hasProjectUrl, projectUrl, hasTopics, topics)
        {
            OpenIssuesNumber = openIssuesNumber;
            OpenPullRequestsNumber = openPullRequestsNumber;
            TotalCommitsNumber = totalCommitsNumber;
            TotalContributorsNumber = totalContributorsNumber;

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

        public int OpenIssuesNumber { get; set; }

        public int OpenPullRequestsNumber { get; set; }

        public int TotalCommitsNumber { get; set; }

        public int TotalContributorsNumber { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(OpenIssuesNumber)}={OpenIssuesNumber.ToString()}, {nameof(OpenPullRequestsNumber)}={OpenPullRequestsNumber.ToString()}, {nameof(TotalCommitsNumber)}={TotalCommitsNumber.ToString()}, {nameof(TotalContributorsNumber)}={TotalContributorsNumber.ToString()}, {nameof(User)}={User}, {nameof(RespositoryLink)}={RespositoryLink}, {nameof(RespositoryName)}={RespositoryName}, {nameof(Description)}={Description}, {nameof(TotalStars)}={TotalStars.ToString()}, {nameof(TotalForks)}={TotalForks.ToString()}, {nameof(HasProjectUrl)}={HasProjectUrl.ToString()}, {nameof(ProjectUrl)}={ProjectUrl}, {nameof(HasTopics)}={HasTopics.ToString()}, {nameof(Topics)}={Topics}}}";
        }
    }
}