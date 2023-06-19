using System.Net;
using System.Text;
using HtmlAgilityPack;
using GithubNet.Models;

namespace GithubNet
{
    internal static class GithubNetManager
    {
        internal static async Task<List<TrendItem>> GetAllTrendEntries(bool loadTrendItemDetails = false, string customQuery = "https://github.com/trending")
        {
            List<TrendItem> TrendingRepositoriesList = new();
            HtmlWeb Github = new();

            HtmlDocument GithubDoc = Github.Load(customQuery);
            IEnumerable<HtmlNode> nodes = GithubDoc.QuerySelectorAll("article.Box-row");

            foreach (HtmlNode productElement in nodes)
            {
                try
                {
                    HtmlNode Description = productElement.QuerySelector("article.Box-row > p");
                    HtmlNode Username = productElement.QuerySelector("article.Box-row span.text-normal");

                    HtmlNode RepositoryLink = productElement.QuerySelector("article.Box-row > h2 > a");
                    IList<HtmlNode> TotalForksAndStars = productElement.QuerySelectorAll("article.Box-row > div > a.Link--muted");

                    string DescriptionFiltered = string.Empty;
                    if (Description != null)
                    {
                        DescriptionFiltered = FilterLineBreaks(Description.InnerText);
                        DescriptionFiltered = WebUtility.HtmlDecode(DescriptionFiltered);
                    }
                    string UsernameFiltered = FilterLineBreaks(Username.InnerText.Replace(" /", string.Empty));

                    string RepositoryLinkFiltered = $"https://github.com{RepositoryLink.GetAttributeValue("href", string.Empty)}";
                    string[] RepositoryNameParse = RepositoryLinkFiltered.Split('/');

                    string RepositoryName = "An error occured";

                    if (RepositoryNameParse?.Length >= 2)
                    {
                        RepositoryName = RepositoryNameParse[4];
                    }

                    string TotalStars = FilterLineBreaks(TotalForksAndStars[0].InnerText);
                    int.TryParse(TotalStars.Replace(",", string.Empty), out int TotalStarsFiltered);

                    string TotalForks = FilterLineBreaks(TotalForksAndStars[1].InnerText);
                    int.TryParse(TotalForks.Replace(",", string.Empty), out int TotalForksFiltered);

                    string ProgrammingLanguage = "None";
                    if (productElement.QuerySelectorAll("article.Box-row > div > span > span") != null)
                    {
                        IList<HtmlNode> ProgrammingLanguageElementCount = productElement.QuerySelectorAll("article.Box-row > div > span > span");
                        if (ProgrammingLanguageElementCount.Count > 0)
                        {
                            ProgrammingLanguage = ProgrammingLanguageElementCount[1].InnerText;
                        }
                    }

                    if (loadTrendItemDetails)
                    {
                        TrendItem trendItem = new(ProgrammingLanguage, false, false, string.Empty, string.Empty, UsernameFiltered, RepositoryLinkFiltered, RepositoryName, DescriptionFiltered, TotalStarsFiltered, TotalForksFiltered, false, string.Empty, false, Array.Empty<string>());
                        TrendingRepositoriesList.Add(await GetTrendDetails(trendItem));
                    }
                    else
                    {
                        TrendingRepositoriesList.Add(new(ProgrammingLanguage, false, false, string.Empty, string.Empty, UsernameFiltered, RepositoryLinkFiltered, RepositoryName, DescriptionFiltered, TotalStarsFiltered, TotalForksFiltered, false, string.Empty, false, Array.Empty<string>()));
                    }
                }
                catch (Exception)
                {
                    //Skip TrendItem if corrupt
                }
            }

            return TrendingRepositoriesList;
        }

        private static async Task<ItemBase> FillBaseData(HtmlNode node, string repositoryUrl)
        {
            HtmlNode Username = node.SelectSingleNode("/html/body/div[1]/div[4]/div/main/div/div[1]/div[1]/div/span[1]/a");
            HtmlNode RepositoryName = node.SelectSingleNode("/html/body/div[1]/div[4]/div/main/div/div[1]/div[1]/div/strong/a");

            HtmlNode Description = node.SelectSingleNode("/html/body/div[1]/div[4]/div/main/turbo-frame/div/div/div/div[2]/div[2]/div/div[1]/div/p");
            HtmlNode TotalForks = node.SelectSingleNode("//*[@id=\"repo-network-counter\"]");

            HtmlNode TotalStars = node.SelectSingleNode("//*[@id=\"repo-stars-counter-star\"]");
            HtmlNode ProjectUrl = node.SelectSingleNode("/html/body/div[1]/div[4]/div/main/turbo-frame/div/div/div/div[2]/div[2]/div/div[1]/div/div[1]/span/a");

            HtmlNode Topics = node.SelectSingleNode("/html/body/div[1]/div[4]/div/main/turbo-frame/div/div/div/div[2]/div[2]/div/div[1]/div/div[2]/div");

            string DescriptionFiltered = FilterLineBreaks(Description.InnerText);
            if (!string.IsNullOrWhiteSpace(DescriptionFiltered))
            {
                DescriptionFiltered = WebUtility.HtmlDecode(DescriptionFiltered);
            }

            List<string> TopicsFiltered = new();
            bool HasTopics = false;

            if (Topics != null)
            {
                HasTopics = true;
                IList<HtmlNode> TopicsValues = Topics.QuerySelectorAll("a.topic-tag");

                foreach (HtmlNode t in TopicsValues)
                {
                    TopicsFiltered.Add(FilterLineBreaks(t.InnerHtml));
                }
            }

            string ProjectUrlFiltered = string.Empty;
            bool HasProjectUrl = false;

            if (ProjectUrl != null)
            {
                HasProjectUrl = true;
                ProjectUrlFiltered = ProjectUrl.GetAttributeValue("href", string.Empty);
            }

            string ForksValue = TotalForks.GetAttributeValue("title", string.Empty);
            int.TryParse(ForksValue.Replace(",", string.Empty), out int ForksFiltered);

            string StarsValue = TotalStars.GetAttributeValue("title", string.Empty);
            int.TryParse(StarsValue.Replace(",", string.Empty), out int StarsFiltered);

            string UsernameFiltered = FilterLineBreaks(Username.InnerText.Replace(" /", string.Empty));
            string RepositoryNameFiltered = FilterLineBreaks(RepositoryName.InnerText.Replace(" /", string.Empty));

            return new(UsernameFiltered, repositoryUrl, RepositoryNameFiltered, DescriptionFiltered, StarsFiltered, ForksFiltered, HasProjectUrl, ProjectUrlFiltered, HasTopics, TopicsFiltered.ToArray());
        }

        internal static async Task<Repository> GetRepositoryInfo(string repositoryLink)
        {
            HtmlWeb CommitUrl = new();
            HtmlDocument TrendingRep = CommitUrl.Load(repositoryLink);

            ItemBase baseData = await FillBaseData(TrendingRep.DocumentNode, repositoryLink);
            HtmlNode OpenIssuesNumber = TrendingRep.DocumentNode.SelectSingleNode("//*[@id=\"issues-repo-tab-count\"]");

            HtmlNode OpenPullRequestsNumber = TrendingRep.DocumentNode.SelectSingleNode("//*[@id=\"pull-requests-repo-tab-count\"]");
            HtmlNode TotalCommitsNumber = TrendingRep.DocumentNode.SelectSingleNode("/html/body/div[1]/div[4]/div/main/turbo-frame/div/div/div/div[2]/div[1]/div[2]/div[1]/div/div[4]/ul/li/a/span/strong");

            HtmlNode TotalContributorsNumber = TrendingRep.DocumentNode.SelectSingleNode("/html/body/div[1]/div[4]/div/main/turbo-frame/div/div/div/div[2]/div[2]/div/div[5]/div/h2/a/span");

            string OpenIssuesNumberFiltered = FilterLineBreaks(OpenIssuesNumber.InnerHtml);
            int.TryParse(OpenIssuesNumberFiltered.Replace(",", string.Empty), out int OpenIssuesNumberValue);

            string OpenPullRequestsNumberFiltered = FilterLineBreaks(OpenPullRequestsNumber.InnerHtml);
            int.TryParse(OpenPullRequestsNumberFiltered.Replace(",", string.Empty), out int OpenPullRequestsNumberValue);

            string TotalCommitsNumberFiltered = FilterLineBreaks(TotalCommitsNumber.InnerText);
            int.TryParse(TotalCommitsNumberFiltered.Replace(",", string.Empty), out int TotalCommitsNumberValue);

            int TotalContributorsNumberValue = 0;
            if (TotalContributorsNumber != null)
            {
                string TotalContributorsNumberFiltered = FilterLineBreaks(TotalContributorsNumber.InnerHtml);
                int.TryParse(TotalContributorsNumberFiltered.Replace(",", string.Empty), out TotalContributorsNumberValue);
            }

            return new Repository(OpenIssuesNumberValue, OpenPullRequestsNumberValue, TotalCommitsNumberValue, TotalContributorsNumberValue, baseData.User, baseData.RespositoryLink, baseData.RespositoryName, baseData.Description, baseData.TotalStars, baseData.TotalForks, baseData.HasProjectUrl, baseData.ProjectUrl, baseData.HasTopics, baseData.Topics);
        }

        internal static async Task<TrendItem> GetTrendDetails(TrendItem entryItem)
        {
            entryItem.HasDetails = true;
            HtmlWeb TrendingRepository = new();

            HtmlDocument TrendingRep = TrendingRepository.Load(entryItem.RespositoryLink);
            HtmlNode ArchiveStatus = TrendingRep.DocumentNode.SelectSingleNode("/html/body/div[1]/div[4]/div/main/div[1]");

            HtmlNode ProjectUrl = TrendingRep.QuerySelector("div.my-3:nth-child(3) > span:nth-child(2) > a:nth-child(1)");
            IList<HtmlNode> Topics = TrendingRep.QuerySelectorAll("a.topic-tag");

            if (ArchiveStatus?.InnerHtml.Contains("This repository has been archived by the owner on", StringComparison.InvariantCultureIgnoreCase) == true)
            {
                entryItem.IsArchived = true;
            }

            if (ProjectUrl != null)
            {
                entryItem.HasProjectUrl = true;
                entryItem.ProjectUrl = ProjectUrl.GetAttributeValue("href", string.Empty);
            }

            if (Topics.Count > 0)
            {
                List<string> TopicNames = new();
                foreach (HtmlNode TopicItem in Topics)
                {
                    TopicNames.Add(FilterLineBreaks(TopicItem.InnerHtml));
                }
                entryItem.Topics = TopicNames.ToArray();
                entryItem.HasTopics = true;
            }
            else
            {
                entryItem.Topics = Array.Empty<string>();
            }

            entryItem.LastCommitTime = $"Last commit: {GetLastCommitTime(entryItem)}";
            return entryItem;
        }

        private static string GetLastCommitTime(TrendItem entry)
        {
            string LastCommitTime = "Error";
            string CommitTime = FetchLastCommitTime(entry, "master");

            if (string.IsNullOrEmpty(CommitTime))
            {
                CommitTime = FetchLastCommitTime(entry, "main");
                if (!string.IsNullOrEmpty(CommitTime))
                {
                    entry.LastCommitUrl = entry.GetLastCommitUrl("main");
                    LastCommitTime = CommitTime;
                }
            }
            else
            {
                entry.LastCommitUrl = entry.GetLastCommitUrl("master");
                LastCommitTime = CommitTime;
            }

            return LastCommitTime;
        }

        private static string FetchLastCommitTime(TrendItem entry, string codeBranch)
        {
            HtmlWeb CommitUrl = new();
            HtmlDocument TrendingRep = CommitUrl.Load(entry.GetLastCommitUrl(codeBranch));

            HtmlNode LastCommitTimeRaw = TrendingRep.QuerySelector("relative-time.no-wrap");

            if (LastCommitTimeRaw?.InnerHtml.Length > 0)
            {
                return FilterLineBreaks(LastCommitTimeRaw.InnerHtml);
            }
            return null;
        }

        private static string FilterLineBreaks(string textIn)
        {
            StringBuilder sb = new(textIn.Length);
            foreach (char i in textIn)
            {
                if (i != '\n' && i != '\r' && i != '\t')
                {
                    sb.Append(i);
                }
            }
            return sb.ToString().Trim();
        }

        public static string ParseTopicNameToUrl(string topicNameIn)
        {
            string LanguageName = NormalizeLanguageNameIdentifier(topicNameIn);
            return $"https://github.com/topics/{LanguageName}";
        }

        private static string NormalizeLanguageNameIdentifier(string topicNameIn)
        {
            string normalized = topicNameIn.Replace(" ", "-").ToLowerInvariant();
            return normalized switch
            {
                "c#" => "csharp",
                "c++" => "cpp",
                _ => normalized,
            };
        }
    }
}