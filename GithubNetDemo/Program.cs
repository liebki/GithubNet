using GithubNet.Managers;
using GithubNet.Models.Extra;
using GithubNet.Models.Repositories;
using GithubNet.Models.Userprofiles;

namespace GithubNetDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            GithubNetClient client = new();

            IEnumerable<FullRepository> repos = client.GetFullRepositories("fffaraz");
            foreach (FullRepository it in repos)
            {
                Console.WriteLine(it);
                Console.WriteLine("\n");
            }


            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("\n");



            IEnumerable<string> TrendRepositoriesUrlList = client.GetUrlsOfTrendingRepositories();
            foreach (string it in TrendRepositoriesUrlList)
            {
                Console.WriteLine(it);
                Console.WriteLine("\n");
            }


            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("\n");



            IEnumerable<TrendRepository> TrendRepositoriesList = client.GetAllTrendingRepositories();
            foreach (TrendRepository it in TrendRepositoriesList)
            {
                Console.WriteLine(it);
                Console.WriteLine("\n");
            }


            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("\n");



            (LightUserprofile Userprofile, IEnumerable<UserRepository> UserRepositories, TopValues topValues) UserWithRepositories = client.GetLightUserprofileWithRepositories("liebki");

            Console.WriteLine(UserWithRepositories.Userprofile.ToString());
            Console.WriteLine("\n");

            Console.WriteLine(UserWithRepositories.topValues.ToString());
            Console.WriteLine("\n");

            foreach (UserRepository it in UserWithRepositories.UserRepositories)
            {
                Console.WriteLine(it);
                Console.WriteLine("\n");
            }


            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("\n");



            FullUserprofile fullUserprofile = client.GetFullUserprofile("liebki");
            Console.Write(fullUserprofile.ToString());


            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("\n");



            FullRepository fullRepository = client.GetFullRepository("https://github.com/bgstaal/multipleWindow3dScene");
            Console.Write(fullRepository.ToString());


            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("\n");



            LightUserprofile lightUserprofile = client.GetLightUserprofile("bgstaal");
            Console.WriteLine(lightUserprofile.ToString());
        }
    }
}