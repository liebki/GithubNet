using GithubNet.Models.Extra;
using GithubNet.Models.Repositories;
using GithubNet.Models.Userprofiles;
using HtmlAgilityPack;
using System;
using System.Net;

namespace GithubNet.Managers
{
    public static class GithubNetManager
    {

        #region GetTrends

        internal static List<string> GetAllTrendRepositoryUrls(string CustomTrendingQuery)
        {
            List<string> TrendRepositoryUrlList = new();
            HtmlDocument GithubPageDocument = UtilManager.GetHtmlDoc(CustomTrendingQuery, true);

            IEnumerable<HtmlNode> DocumentNodes = GithubPageDocument.QuerySelectorAll("article.Box-row");
            foreach (HtmlNode docNode in DocumentNodes)
            {
                HtmlNode RepositoryLink = docNode.QuerySelector("article.Box-row > h2 > a");
                string RepositoryLinkFiltered = $"https://github.com{RepositoryLink.GetAttributeValue("href", string.Empty)}";

                TrendRepositoryUrlList.Add(RepositoryLinkFiltered);
            }

            return TrendRepositoryUrlList;
        }

        internal static List<FullRepository> GetAllFullRepositoriesFromTrends(string CustomTrendingQuery)
        {
            List<string> TrendingRepositoriesList = GetAllTrendRepositoryUrls(CustomTrendingQuery);
            List<FullRepository> FullTrendingRepositoriesList = new();

            foreach (string repoUrl in TrendingRepositoriesList)
            {
                FullRepository fullRepository = GetFullRepository(repoUrl);
                FullTrendingRepositoriesList.Add(fullRepository);
            }

            return FullTrendingRepositoriesList;
        }

        internal static List<TrendRepository> GetAllTrendRepositories(string CustomTrendingQuery = "https://github.com/trending")
        {
            List<TrendRepository> TrendingRepositoriesList = new();
            HtmlDocument GithubPageDocument = UtilManager.GetHtmlDoc(CustomTrendingQuery, true);

            IEnumerable<HtmlNode> DocumentNodes = GithubPageDocument.QuerySelectorAll("article.Box-row");
            foreach (HtmlNode docNode in DocumentNodes)
            {
                try
                {
                    HtmlNode Description = docNode.QuerySelector("article.Box-row > p");
                    HtmlNode Username = docNode.QuerySelector("article.Box-row span.text-normal");

                    HtmlNode RepositoryLink = docNode.QuerySelector("article.Box-row > h2 > a");
                    IList<HtmlNode> TotalForksAndStars = docNode.QuerySelectorAll("article.Box-row > div > a.Link--muted");

                    HtmlNode StarsTodayRaw = docNode.SelectSingleNode(".//span[@class='d-inline-block float-sm-right']");
                    string StarsTodayRawValue = UtilManager.ClearStrings(StarsTodayRaw.InnerText.Replace(" ", string.Empty).Replace(",", string.Empty).Replace("starstoday", string.Empty));

                    _ = int.TryParse(StarsTodayRawValue, out int StarsTodayFiltered);

                    string DescriptionFiltered = string.Empty;

                    if (Description != null)
                    {
                        DescriptionFiltered = UtilManager.ClearStrings(Description.InnerText);
                        DescriptionFiltered = WebUtility.HtmlDecode(DescriptionFiltered);
                    }
                    string UsernameFiltered = UtilManager.ClearStrings(Username.InnerText.Replace(" /", string.Empty));

                    string RepositoryLinkFiltered = $"https://github.com{RepositoryLink.GetAttributeValue("href", string.Empty)}";
                    string[] RepositoryNameParse = RepositoryLinkFiltered.Split('/');

                    string RepositoryName = "An error occured";

                    if (RepositoryNameParse?.Length >= 2)
                    {
                        RepositoryName = RepositoryNameParse[4];
                    }

                    string TotalStars = UtilManager.ClearStrings(TotalForksAndStars[0].InnerText);
                    _ = int.TryParse(TotalStars.Replace(",", string.Empty), out int TotalStarsFiltered);

                    string TotalForks = UtilManager.ClearStrings(TotalForksAndStars[1].InnerText);
                    _ = int.TryParse(TotalForks.Replace(",", string.Empty), out int TotalForksFiltered);

                    string ProgrammingLanguage = "None";
                    if (docNode.QuerySelectorAll("article.Box-row > div > span > span") != null)
                    {
                        IList<HtmlNode> ProgrammingLanguageElementCount = docNode.QuerySelectorAll("article.Box-row > div > span > span");
                        if (ProgrammingLanguageElementCount.Count > 0)
                        {
                            ProgrammingLanguage = ProgrammingLanguageElementCount[1].InnerText;
                        }
                    }

                    TrendRepository repository = new(TotalStarsFiltered, ProgrammingLanguage, TotalForksFiltered, StarsTodayFiltered, UsernameFiltered, RepositoryName, DescriptionFiltered);
                    TrendingRepositoriesList.Add(repository);
                }
                catch (Exception e)
                {
                    // Skip item if corrupted
                }
            }

            return TrendingRepositoriesList;
        }

        #endregion GetTrends

        #region Userprofile

        internal static LightUserprofile GetLightUserprofile(string Username)
        {
            (LightUserprofile profileObj, _) = GetUserprofileData(Username);
            return profileObj;
        }

        internal static FullUserprofile GetFullUserprofile(string Username)
        {
            (LightUserprofile profileObj, HtmlDocument rawProfileData) = GetUserprofileData(Username);

            HtmlNode LastYearContributionsCount = rawProfileData.QuerySelector(".js-yearly-contributions > div:nth-child(1) > h2");
            int LastYearContributionsCountValue = 0;

            if (LastYearContributionsCount != null)
            {
                string LastYearContributionsCountRaw = UtilManager.ClearStrings(LastYearContributionsCount.InnerHtml, true, " contributions in the last year");
                LastYearContributionsCountValue = UtilManager.ParseNumberValue(LastYearContributionsCountRaw);
            }

            HtmlNode HasSpecialUserReadmeEnabled = rawProfileData.QuerySelector(".text-mono");
            bool HasSpecialUserReadmeEnabledValue = false;

            if (HasSpecialUserReadmeEnabled != null)
            {
                HasSpecialUserReadmeEnabledValue = true;
            }

            FullUserprofile fullUserprofile = new(profileObj, LastYearContributionsCountValue, HasSpecialUserReadmeEnabledValue);
            return fullUserprofile;

        }

        #endregion Userprofile

        #region LightUserprofile & UserRepository

        internal static (LightUserprofile Userprofile, IEnumerable<UserRepository> UserRepositories, TopValues topValues) GetLightUserprofileWithRepositories(string Username)
        {
            (LightUserprofile profileObj, _) = GetUserprofileData(Username);
            LightUserprofile lightUserprofile = profileObj;

            HtmlDocument UserRepositoriesDocument = UtilManager.GetHtmlDoc($"https://github.com/{Username}?tab=repositories", true);
            HtmlNode UserRepositoriesNode = UserRepositoriesDocument.DocumentNode;

            IEnumerable<HtmlNode> RawUserRepositoriesList = UserRepositoriesNode.QuerySelectorAll("li.col-12");
            List<UserRepository> UserRepositoriesList = new();

            foreach (HtmlNode repo in RawUserRepositoriesList)
            {
                HtmlNode nameNode = repo.SelectSingleNode(".//h3/a");
                string name = (nameNode != null && !string.IsNullOrEmpty(nameNode.InnerText.Trim())) ? nameNode.InnerText.Trim() : "None";

                HtmlNode descriptionNode = repo.SelectSingleNode(".//p[@itemprop='description']");
                string description = (descriptionNode != null && !string.IsNullOrEmpty(descriptionNode.InnerText.Trim())) ? descriptionNode.InnerText.Trim() : "None";

                HtmlNode languageNode = repo.SelectSingleNode(".//span[contains(@class, 'repo-language-color')]/following-sibling::span");
                string language = (languageNode != null && !string.IsNullOrEmpty(languageNode.InnerText.Trim())) ? languageNode.InnerText.Trim() : "None";

                HtmlNode forkedTextNode = repo.SelectSingleNode(".//span[contains(@class, 'color-fg-muted') and contains(., 'Forked from')]");
                bool isFork = (forkedTextNode != null);

                HtmlNode starCountNode = repo.SelectSingleNode(".//a[contains(@href, '/stargazers')]");
                int starCountValue = 0;
                if (starCountNode != null && !string.IsNullOrEmpty(starCountNode.InnerText.Trim()))
                {
                    starCountValue = UtilManager.ParseNumberValue(UtilManager.ClearStrings(starCountNode.InnerText.Trim()));
                }

                HtmlNode forkCountNode = repo.SelectSingleNode(".//a[contains(@href, '/forks')]");
                int forkCountValue = 0;
                if (forkCountNode != null && !string.IsNullOrEmpty(forkCountNode.InnerText.Trim()))
                {
                    forkCountValue = UtilManager.ParseNumberValue(UtilManager.ClearStrings(forkCountNode.InnerText.Trim()));
                }

                HtmlNode licenseNode = repo.SelectSingleNode("//span[@class='mr-3']/text()[normalize-space()]");
                string licenseValue = (licenseNode != null && !string.IsNullOrEmpty(licenseNode.InnerText.Trim())) ? UtilManager.ClearStrings(licenseNode.InnerText) : "None";

                HtmlNode lastUpdateNode = repo.SelectSingleNode(".//relative-time/@datetime");
                string lastUpdate = (lastUpdateNode != null && !string.IsNullOrEmpty(lastUpdateNode.GetAttributeValue("datetime", string.Empty).Trim())) ? lastUpdateNode.GetAttributeValue("datetime", string.Empty).Trim() : "None";

                UserRepository userRepository = new(isFork, licenseValue, lastUpdate, language, starCountValue, forkCountValue, Username, name, description);
                UserRepositoriesList.Add(userRepository);
            }

            TopValues topValues = UtilManager.GetTopValues(UserRepositoriesList);
            return (lightUserprofile, UserRepositoriesList, topValues);
        }

        #endregion LightUserprofile & UserRepository

        #region FullRepository

        internal static FullRepository GetFullRepository(string RepoUrl)
        {
            HtmlDocument RepositoryDocument = UtilManager.GetHtmlDoc(RepoUrl, true);
            HtmlNode RepositoryNode = RepositoryDocument.DocumentNode;

            string UsernameValue = UtilManager.GetUsernameFromGitHubUrl(RepoUrl);

            HtmlNode RepoName = RepositoryNode.SelectSingleNode("/html/body/div[1]/div[4]/div/main/div/div[1]/div[1]/div/strong/a");
            string RepoNameValue = "None";

            if (RepoName != null)
            {
                RepoNameValue = UtilManager.ClearStrings(RepoName.InnerHtml);
            }

            HtmlNode Description = RepositoryNode.SelectSingleNode("/html/body/div[1]/div[4]/div/main/turbo-frame/div/div/div/div[2]/div[2]/div/div[1]/div/div/p");
            string DescriptionValue = "None";

            if (Description != null)
            {
                DescriptionValue = UtilManager.ClearStrings(Description.InnerHtml);
            }

            HtmlNode OpenIssueCount = RepositoryNode.SelectSingleNode("//*[@id=\"issues-repo-tab-count\"]");
            int OpenIssueCountValue = 0;

            if (OpenIssueCount != null)
            {
                OpenIssueCountValue = UtilManager.ParseNumberValue(OpenIssueCount.Attributes["title"].Value);
            }

            HtmlNode OpenPullRequestsCount = RepositoryNode.SelectSingleNode("//*[@id=\"pull-requests-repo-tab-count\"]");
            int OpenPullRequestsCountValue = 0;

            if (OpenPullRequestsCount != null)
            {
                OpenPullRequestsCountValue = UtilManager.ParseNumberValue(OpenPullRequestsCount.Attributes["title"].Value);
            }

            HtmlNode TotalCommitsCount = RepositoryNode.SelectSingleNode("/html/body/div[1]/div[4]/div/main/turbo-frame/div/div/div/div[2]/div[1]/div[2]/div[1]/div/div[4]/ul/li/a/span/strong");
            int TotalCommitsCountValue = 0;

            if (TotalCommitsCount != null)
            {
                TotalCommitsCountValue = UtilManager.ParseNumberValue(UtilManager.ClearStrings(TotalCommitsCount.InnerHtml));
            }

            HtmlNode LastCommitText = RepositoryNode.SelectSingleNode("/html/body/div[1]/div[4]/div/main/turbo-frame/div/div/div/div[2]/div[1]/div[2]/div[1]/div/div[2]/div[1]/span/a[1]");
            string LastCommitTextValue = "None";

            if (LastCommitText != null)
            {
                LastCommitTextValue = UtilManager.ClearStrings(LastCommitText.InnerHtml.Replace(" (", string.Empty));
            }

            int WatcherCountValue = GetWatcherCount(RepositoryNode);

            HtmlNode ContributorCount = RepositoryNode.SelectSingleNode("/html/body/div[1]/div[4]/div/main/turbo-frame/div/div/div/div[2]/div[2]/div/div[5]/div/h2/a/span");
            int ContributorCountValue = 0;

            if (ContributorCount != null)
            {
                ContributorCountValue = UtilManager.ParseNumberValue(ContributorCount.Attributes["title"].Value);
            }

            IEnumerable<HtmlNode> Topics = RepositoryNode.QuerySelectorAll("a.topic-tag");
            IEnumerable<string> TopicValues = new List<string>();

            if (Topics != null && Topics.Any())
            {
                TopicValues = Topics.Select(t => UtilManager.ClearStrings(t.InnerHtml));
            }

            HtmlNode ReleaseCount = RepositoryNode.SelectSingleNode("/html/body/div[1]/div[4]/div/main/turbo-frame/div/div/div/div[2]/div[2]/div/div[2]/div/h2/a/span");
            int ReleaseCountValue = 0;

            if (ReleaseCount != null)
            {
                ReleaseCountValue = UtilManager.ParseNumberValue(ReleaseCount.Attributes["title"].Value);
            }

            HtmlNode LatestReleaseText = RepositoryNode.SelectSingleNode("/html/body/div[1]/div[4]/div/main/turbo-frame/div/div/div/div[2]/div[2]/div/div[2]/div/a/div/div[1]/span[1]");
            string LatestReleaseTextValue = "None";

            if (LatestReleaseText != null)
            {
                LatestReleaseTextValue = UtilManager.ClearStrings(LatestReleaseText.InnerHtml);
            }

            HtmlNode TagCount = RepositoryNode.SelectSingleNode("/html/body/div[1]/div[4]/div/main/turbo-frame/div/div/div/div[2]/div[1]/div[1]/div[3]/a[2]/strong");
            int TagCountValue = 0;

            if (TagCount != null)
            {
                TagCountValue = UtilManager.ParseNumberValue(UtilManager.ClearStrings(TagCount.InnerHtml));
            }

            HtmlNode BranchCount = RepositoryNode.SelectSingleNode("/html/body/div[1]/div[4]/div/main/turbo-frame/div/div/div/div[2]/div[1]/div[1]/div[3]/a[1]/strong");
            int BranchCountValue = 0;

            if (BranchCount != null)
            {
                BranchCountValue = UtilManager.ParseNumberValue(UtilManager.ClearStrings(BranchCount.InnerHtml));
            }

            HtmlNode DefaultBranchName = RepositoryNode.SelectSingleNode("/html/body/div[1]/div[4]/div/main/turbo-frame/div/div/div/div[2]/div[1]/div[1]/div[1]/details/summary/span[1]");
            string DefaultBranchNameValue = "None";

            if (DefaultBranchName != null)
            {
                DefaultBranchNameValue = UtilManager.ClearStrings(DefaultBranchName.InnerHtml);
            }

            IEnumerable<HtmlNode> MainLanguage = RepositoryNode.QuerySelectorAll("li.d-inline:nth-child(1) > a:nth-child(1) > span");
            string MainLanguageValue = "None";

            if (MainLanguage != null && MainLanguage.Any())
            {
                MainLanguageValue = UtilManager.ClearStrings(MainLanguage.ElementAt(0).InnerHtml);
            }

            HtmlNode Stars = RepositoryNode.SelectSingleNode("//*[@id=\"repo-stars-counter-star\"]");
            int StarsValue = 0;

            if (Stars != null)
            {
                StarsValue = UtilManager.ParseNumberValue(UtilManager.ClearStrings(Stars.InnerHtml));
            }

            HtmlNode Forks = RepositoryNode.SelectSingleNode("//*[@id=\"repo-network-counter\"]");
            int ForksValue = 0;

            if (Forks != null)
            {
                ForksValue = UtilManager.ParseNumberValue(UtilManager.ClearStrings(Forks.InnerHtml));
            }

            FullRepository fullRepository = new($"https://github.com{RepoUrl}", OpenIssueCountValue, OpenPullRequestsCountValue, TotalCommitsCountValue, LastCommitTextValue, WatcherCountValue, ContributorCountValue, TopicValues.ToArray(), ReleaseCountValue, LatestReleaseTextValue, TagCountValue, BranchCountValue, DefaultBranchNameValue, MainLanguageValue, StarsValue, ForksValue, UsernameValue, RepoNameValue, DescriptionValue);
            return fullRepository;
        }

        internal static IEnumerable<FullRepository> GetFullRepositories(string Username)
        {
            IEnumerable<string> RepositoryUrlsList = GetAllRepositorsUrls(Username);
            List<FullRepository> FullRepositoriesList = new();

            foreach (string repoUrl in RepositoryUrlsList)
            {
                FullRepository fullRepository = GetFullRepository(repoUrl);
                FullRepositoriesList.Add(fullRepository);
            }

            return FullRepositoriesList;
        }

        #endregion FullRepository

        #region Utils

        private static int GetWatcherCount(HtmlNode DocumentNode)
        {
            int watchersCount = 0;
            IEnumerable<HtmlNode> octiconEyeElements = DocumentNode.QuerySelectorAll(".octicon-eye");

            if (octiconEyeElements != null)
            {
                foreach (HtmlNode octiconEyeElement in octiconEyeElements)
                {
                    HtmlNode strongElement = octiconEyeElement.SelectSingleNode("following-sibling::strong");

                    if (strongElement != null)
                    {
                        _ = int.TryParse(strongElement.InnerText.Trim(), out watchersCount);
                    }
                }
            }

            return watchersCount;
        }

        private static IEnumerable<string> GetAllRepositorsUrls(string Username)
        {
            HtmlDocument UserRepositoriesDocument = UtilManager.GetHtmlDoc($"https://github.com/{Username}?tab=repositories", true);
            HtmlNode UserRepositoriesNode = UserRepositoriesDocument.DocumentNode;

            IEnumerable<HtmlNode> UserRepositorieList = UserRepositoriesNode.QuerySelectorAll("li.col-12");
            List<string> RepositoryUrlList = new();

            foreach (HtmlNode repository in UserRepositorieList)
            {
                HtmlNode RepoName = repository.SelectSingleNode(".//h3/a");
                string RepoUrl = "None";

                if (RepoName != null)
                {
                    RepoUrl = RepoName.Attributes["href"].Value;
                }

                RepositoryUrlList.Add(RepoUrl);
            }

            return RepositoryUrlList;
        }

        private static (LightUserprofile profil, HtmlDocument profilepage) GetUserprofileData(string Username)
        {
            HtmlDocument GithubProfilepage = UtilManager.GetHtmlDoc($"https://github.com/{Username}", true);

            HtmlNode ProfileName = GithubProfilepage.QuerySelector(".p-name");
            string ProfileNameValue = ProfileName.InnerHtml.Trim();

            HtmlNode Description = GithubProfilepage.QuerySelector(".p-note > div:nth-child(1)");
            string DescriptionValue = "None";

            if (Description != null)
            {
                DescriptionValue = Description.InnerHtml.Trim();
            }

            HtmlNode FollowerCount = GithubProfilepage.QuerySelector("span.text-bold:nth-child(2)");
            int FollowerCountValue = UtilManager.ParseNumberValue(FollowerCount.InnerHtml);

            HtmlNode FollowingCount = GithubProfilepage.QuerySelector("span.text-bold:nth-child(1)");
            int FollowingCountValue = UtilManager.ParseNumberValue(FollowingCount.InnerHtml);

            HtmlNode Location = GithubProfilepage.QuerySelector(".p-label");
            string LocationValue = "None";

            if (Location != null)
            {
                LocationValue = Location.InnerHtml.Trim();
            }

            HtmlNode ImageUrl = GithubProfilepage.QuerySelector("div.d-inline-block > a");
            string ImageUrlValue = ImageUrl.Attributes["href"].Value;

            HtmlNode Organization = GithubProfilepage.QuerySelector(".p-org > div:nth-child(1)");
            string OrganizationValue = "None";

            if (Organization != null)
            {
                OrganizationValue = Organization.InnerHtml.Trim();
            }

            HtmlNode PrimaryProfileUrl = GithubProfilepage.DocumentNode.SelectSingleNode("//a[@class='Link--primary']");
            string PrimaryProfileUrlValue = "None";

            if (PrimaryProfileUrl != null)
            {
                PrimaryProfileUrlValue = PrimaryProfileUrl.Attributes["href"].Value;
            }

            HtmlNode RepositoryCount = GithubProfilepage.QuerySelector("div.box-shadow-none > nav:nth-child(1) > a:nth-child(2) > span");
            int RepositoryCountValue = 0;

            if (RepositoryCount != null)
            {
                RepositoryCountValue = UtilManager.ParseNumberValue(RepositoryCount.Attributes["title"].Value);
            }

            HtmlNode StarsGiven = GithubProfilepage.QuerySelector("div.box-shadow-none > nav:nth-child(1) > a:nth-child(5) > span:nth-child(2)");
            int StarsGivenValue = 0;

            if (StarsGiven != null)
            {
                StarsGivenValue = UtilManager.ParseNumberValue(StarsGiven.Attributes["title"].Value);
            }

            return (new(Username, ProfileNameValue, DescriptionValue, OrganizationValue, LocationValue, ImageUrlValue, RepositoryCountValue, FollowerCountValue, FollowingCountValue, PrimaryProfileUrlValue, StarsGivenValue), GithubProfilepage);
        }

        #endregion Utils

    }
}