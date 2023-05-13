namespace GithubNet
{
    public class GithubNetClient
    {
        public async Task<List<TrendItem>> GetTrendItemsAsync(bool loadTrendItemDetails = false, string customQuery = "https://github.com/trending")
        {
            if (!string.IsNullOrEmpty(customQuery) && customQuery.Contains("https://github.com/trending", StringComparison.InvariantCultureIgnoreCase))
            {
                return await GithubNetManager.GetAllTrendEntries(loadTrendItemDetails, customQuery);
            }
            else
            {
                throw new ArgumentException("The parameter customquery, can't be whitespace, null or empty also it has to contain some kind of https://github.com/trending-url string!");
            }
        }

        public async Task<TrendItem> GetTrendItemDetailsAsync(TrendItem entryItem)
        {
            if (entryItem != null && !string.IsNullOrEmpty(entryItem.RespositoryLink) && !string.IsNullOrWhiteSpace(entryItem.RespositoryLink))
            {
                return await GithubNetManager.GetTrendDetails(entryItem);
            }
            else
            {
                throw new ArgumentException("The parameter entryItem, can't be null also the RespositoryLink inside entryItem, has to contain a value and may not be null or empty!");
            }
        }

        public string GetTopicUrlFromTopicName(string topicName)
        {
            if (!string.IsNullOrEmpty(topicName) && !string.IsNullOrWhiteSpace(topicName))
            {
                return GithubNetManager.ParseTopicNameToUrl(topicName);
            }
            else
            {
                throw new ArgumentException("The parameter topicName, can't be whitespace, null or empty, it has to contain some kind of value!");
            }
        }
    }
}