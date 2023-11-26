using System;
using System.Linq;

namespace GithubNet.Models.Repositories
{
    public class UserRepository : RepositoryBase
    {

        public UserRepository(bool isForked, string licenseText, string lastUpdateText, string mainLanguage, int totalStars, int totalForks, string username, string repositoryName, string description) : base(mainLanguage, totalStars, totalForks, username, repositoryName, description)
        {
            IsForked = isForked;
            LicenseText = licenseText;
            LastUpdateText = lastUpdateText;
        }

        public bool IsForked { get; set; }

        public string LicenseText { get; set; }

        public string LastUpdateText { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(IsForked)}={IsForked.ToString()}, {nameof(LicenseText)}={LicenseText}, {nameof(LastUpdateText)}={LastUpdateText}, {nameof(MainLanguage)}={MainLanguage}, {nameof(TotalStars)}={TotalStars.ToString()}, {nameof(TotalForks)}={TotalForks.ToString()}, {nameof(Username)}={Username}, {nameof(RepositoryName)}={RepositoryName}, {nameof(Description)}={Description}}}";
        }
    }
}
