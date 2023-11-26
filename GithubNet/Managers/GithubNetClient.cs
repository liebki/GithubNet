using GithubNet.Models.Extra;
using GithubNet.Models.Repositories;
using GithubNet.Models.Userprofiles;

namespace GithubNet.Managers
{
    public class GithubNetClient
    {
        #region Trends
        public IEnumerable<TrendRepository> GetAllTrendingRepositories(string customQuery = "https://github.com/trending")
        {
            if (!string.IsNullOrEmpty(customQuery) && customQuery.Contains("https://github.com/trending", StringComparison.InvariantCultureIgnoreCase))
            {
                return GithubNetManager.GetAllTrendRepositories(customQuery);
            }
            else
            {
                throw new ArgumentException("The parameter customquery, can't be whitespace, null or empty also it has to contain some kind of https://github.com/trending string!");
            }
        }

        public IEnumerable<string> GetUrlsOfTrendingRepositories(string customQuery = "https://github.com/trending")
        {
            if (!string.IsNullOrEmpty(customQuery) && customQuery.Contains("https://github.com/trending", StringComparison.InvariantCultureIgnoreCase))
            {
                return GithubNetManager.GetAllTrendRepositoryUrls(customQuery);
            }
            else
            {
                throw new ArgumentException("The parameter customquery, can't be whitespace, null or empty also it has to contain some kind of https://github.com/trending string!");
            }
        }

        public IEnumerable<FullRepository> GetFullTrendingRepositories(string customQuery = "https://github.com/trending")
        {
            if (!string.IsNullOrEmpty(customQuery) && customQuery.Contains("https://github.com/trending", StringComparison.InvariantCultureIgnoreCase))
            {
                return GithubNetManager.GetAllFullRepositoriesFromTrends(customQuery);
            }
            else
            {
                throw new ArgumentException("The parameter customquery, can't be whitespace, null or empty also it has to contain some kind of https://github.com/trending string!");
            }
        }

        #endregion Trends

        #region Userprofile

        public FullUserprofile GetFullUserprofile(string Username)
        {
            if (!string.IsNullOrEmpty(Username))
            {
                return GithubNetManager.GetFullUserprofile(Username);
            }
            else
            {
                throw new ArgumentException("The parameter Username, can't be whitespace, null or empty!");
            }
        }

        public LightUserprofile GetLightUserprofile(string Username)
        {
            if (!string.IsNullOrEmpty(Username))
            {
                return GithubNetManager.GetLightUserprofile(Username);
            }
            else
            {
                throw new ArgumentException("The parameter Username, can't be whitespace, null or empty!");
            }
        }

        #endregion Userprofile

        #region FullRepository

        public IEnumerable<FullRepository> GetFullRepositories(string Username)
        {
            if (!string.IsNullOrEmpty(Username))
            {
                return GithubNetManager.GetFullRepositories(Username);
            }
            else
            {
                throw new ArgumentException($"The parameter {nameof(Username)}, can't be whitespace, null or empty!");
            }
        }

        public FullRepository GetFullRepository(string RepositoryUrl)
        {
            if (!string.IsNullOrEmpty(RepositoryUrl) && RepositoryUrl.Contains("https://github.com", StringComparison.InvariantCultureIgnoreCase))
            {
                return GithubNetManager.GetFullRepository(RepositoryUrl);
            }
            else
            {
                throw new ArgumentException($"The parameter {nameof(RepositoryUrl)}, can't be whitespace, null or empty also it has to contain some kind of https://github.com string!");
            }
        }

        #endregion FullRepository

        #region LightUserprofile & UserRepository

        public (LightUserprofile Userprofile, IEnumerable<UserRepository> UserRepositories, TopValues topValues) GetLightUserprofileWithRepositories(string Username)
        {
            if (!string.IsNullOrEmpty(Username))
            {
                return GithubNetManager.GetLightUserprofileWithRepositories(Username);
            }
            else
            {
                throw new ArgumentException("The parameter Username, can't be whitespace, null or empty!");
            }
        }

        #endregion LightUserprofile & UserRepository

        #region Utils

        public string GetTopicUrlFromTopicName(string topicName)
        {
            if (!string.IsNullOrEmpty(topicName) && !string.IsNullOrWhiteSpace(topicName))
            {
                return UtilManager.ParseTopicNameToUrl(topicName);
            }
            else
            {
                throw new ArgumentException("The parameter topicName, can't be whitespace, null or empty, it has to contain some kind of value!");
            }
        }

        #endregion Utils
    }
}