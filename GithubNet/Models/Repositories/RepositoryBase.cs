using System;
using System.Linq;

namespace GithubNet.Models.Repositories
{
    public class RepositoryBase
    {
        public RepositoryBase(string mainLanguage, int totalStars, int totalForks, string username, string repositoryName, string description)
        {
            MainLanguage = mainLanguage;
            TotalStars = totalStars;
            TotalForks = totalForks;
            Username = username;
            RepositoryName = repositoryName;
            Description = description;
        }

        public string MainLanguage { get; set; }

        public int TotalStars { get; set; }

        public int TotalForks { get; set; }

        public string Username { get; set; }

        public string RepositoryName { get; set; }

        public string Description { get; set; }


    }
}
