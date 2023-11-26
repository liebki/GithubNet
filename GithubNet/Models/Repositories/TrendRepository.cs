using System;
using System.Linq;

namespace GithubNet.Models.Repositories
{
    public class TrendRepository : RepositoryBase
    {
        public TrendRepository(int starsToday, string mainLanguage, int totalStars, int totalForks, string username, string repositoryName, string description) : base(mainLanguage, totalStars, totalForks, username, repositoryName, description)
        {
            StarsToday = starsToday;
        }

        public int StarsToday { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(StarsToday)}={StarsToday.ToString()}, {nameof(MainLanguage)}={MainLanguage}, {nameof(TotalStars)}={TotalStars.ToString()}, {nameof(TotalForks)}={TotalForks.ToString()}, {nameof(Username)}={Username}, {nameof(RepositoryName)}={RepositoryName}, {nameof(Description)}={Description}}}";
        }
    }
}
