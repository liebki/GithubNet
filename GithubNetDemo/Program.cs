using GithubNet;
using GithubNet.Models;

namespace GithubNetDemo
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            GithubNetClient client = new();

            Repository tinygrad = await client.GetRepositoryInfoAsync("https://github.com/liebki/GithubNet");
            Console.WriteLine(tinygrad.ToString());

            List<TrendItem> testEntries = await client.GetTrendItemsAsync();
            foreach (TrendItem entry in testEntries)
            {
                Console.WriteLine();
                Console.WriteLine("------------------------------------------");
                Console.WriteLine(entry.ToString());
                Console.WriteLine("------------------------------------------");
                Console.WriteLine();
            }

            Console.WriteLine(client.GetTopicUrlFromTopicName(string.Empty));
        }
    }
}