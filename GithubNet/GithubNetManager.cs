using HtmlAgilityPack;
using System.Net;
using System.Text;

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

                    string RepositoryLinkFiltered = $"https://github.com{RepositoryLink.Attributes["href"].Value}";
                    string[] RepositoryNameParse = RepositoryLinkFiltered.Split('/');

                    string RepositoryName = "An error occured";

                    if (RepositoryNameParse?.Length >= 2)
                    {
                        RepositoryName = RepositoryNameParse[4];
                    }

                    string TotalStarsFiltered = FilterLineBreaks(TotalForksAndStars[0].InnerText);
                    string TotalForksFiltered = FilterLineBreaks(TotalForksAndStars[1].InnerText);

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
                        TrendItem trendItem = new(UsernameFiltered, RepositoryLinkFiltered, RepositoryName, DescriptionFiltered, TotalStarsFiltered, TotalForksFiltered, ProgrammingLanguage);
                        TrendingRepositoriesList.Add(await GetTrendDetails(trendItem));
                    }
                    else
                    {
                        TrendingRepositoriesList.Add(new(UsernameFiltered, RepositoryLinkFiltered, RepositoryName, DescriptionFiltered, TotalStarsFiltered, TotalForksFiltered, ProgrammingLanguage));
                    }
                }
                catch (Exception)
                {
                    //Skip TrendItem if corrupt
                }
            }

            return TrendingRepositoriesList;
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
                entryItem.ProjectUrl = ProjectUrl.Attributes["href"].Value;
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