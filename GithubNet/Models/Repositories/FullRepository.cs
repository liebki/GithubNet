using System;
using System.Linq;

namespace GithubNet
{
    public class FullRepository : RepositoryBase
    {
        public FullRepository(string projectUrl, int openIssueCount, int openPullRequestsCount, int totalCommitsCount, string lastCommitText, int watcherCount, int contributorCount, string[] topics, int releaseCount, string lastReleaseText, int tagCount, int branchCount, string defaultBranchName, string url, string mainLanguage, int totalStars, int totalForks, string username, string repositoryName, string description) : base(url, mainLanguage, totalStars, totalForks, username, repositoryName, description)
        {
            ProjectUrl = projectUrl;
            OpenIssueCount = openIssueCount;
            OpenPullRequestsCount = openPullRequestsCount;
            TotalCommitsCount = totalCommitsCount;
            LastCommitText = lastCommitText;
            WatcherCount = watcherCount;
            ContributorCount = contributorCount;
            Topics = topics;
            ReleaseCount = releaseCount;
            LastReleaseText = lastReleaseText;
            TagCount = tagCount;
            BranchCount = branchCount;
            DefaultBranchName = defaultBranchName;
        }

        public string ProjectUrl { get; set; }

        public int OpenIssueCount { get; set; }

        public int OpenPullRequestsCount { get; set; }

        public int TotalCommitsCount { get; set; }

        public string LastCommitText { get; set; }

        public int WatcherCount { get; set; }

        public int ContributorCount { get; set; }

        public string[] Topics { get; set; }

        public int ReleaseCount { get; set; }

        public string LastReleaseText { get; set; }

        public int TagCount { get; set; }

        public int BranchCount { get; set; }

        public string DefaultBranchName { get; set; }

        public string GetLastCommitUrl(string branch = "")
        {
            if (string.IsNullOrEmpty(branch))
            {
                return $"https://github.com/{this.Username}/{this.RepositoryName}/commit/{this.DefaultBranchName}";
            }

            return $"https://github.com/{this.Username}/{this.RepositoryName}/commit/{branch}";
        }

        public override string ToString()
        {
            return $"{{{nameof(ProjectUrl)}={ProjectUrl}, {nameof(OpenIssueCount)}={OpenIssueCount.ToString()}, {nameof(OpenPullRequestsCount)}={OpenPullRequestsCount.ToString()}, {nameof(TotalCommitsCount)}={TotalCommitsCount.ToString()}, {nameof(LastCommitText)}={LastCommitText}, {nameof(WatcherCount)}={WatcherCount.ToString()}, {nameof(ContributorCount)}={ContributorCount.ToString()}, {nameof(Topics)}={Topics}, {nameof(ReleaseCount)}={ReleaseCount.ToString()}, {nameof(LastReleaseText)}={LastReleaseText}, {nameof(TagCount)}={TagCount.ToString()}, {nameof(BranchCount)}={BranchCount.ToString()}, {nameof(DefaultBranchName)}={DefaultBranchName}, {nameof(Url)}={Url}, {nameof(MainLanguage)}={MainLanguage}, {nameof(TotalStars)}={TotalStars.ToString()}, {nameof(TotalForks)}={TotalForks.ToString()}, {nameof(Username)}={Username}, {nameof(RepositoryName)}={RepositoryName}, {nameof(Description)}={Description}}}";
        }
    }
}
