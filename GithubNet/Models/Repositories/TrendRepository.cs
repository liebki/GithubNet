using System;
using System.Linq;

namespace GithubNet
{
    public class TrendRepository : RepositoryBase
    {
        public TrendRepository(int starsToday, string url, string mainLanguage, int totalStars, int totalForks, string username, string repositoryName, string description) : base(url, mainLanguage, totalStars, totalForks, username, repositoryName, description)
        {
            StarsToday = starsToday;
        }

        public int StarsToday { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(StarsToday)}={StarsToday.ToString()}, {nameof(Url)}={Url}, {nameof(MainLanguage)}={MainLanguage}, {nameof(TotalStars)}={TotalStars.ToString()}, {nameof(TotalForks)}={TotalForks.ToString()}, {nameof(Username)}={Username}, {nameof(RepositoryName)}={RepositoryName}, {nameof(Description)}={Description}}}";
        }
    }
}
