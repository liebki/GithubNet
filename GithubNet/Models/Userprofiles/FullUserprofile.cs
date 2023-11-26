using System;
using System.Linq;

namespace GithubNet.Models.Userprofiles
{
    public class FullUserprofile : LightUserprofile
    {
        public FullUserprofile(LightUserprofile lightUserProfile, int lastYearContributionsCount, bool hasSpecialUserReadmeEnabled) : base(lightUserProfile.Username, lightUserProfile.Name, lightUserProfile.Description, lightUserProfile.Organization, lightUserProfile.Location, lightUserProfile.ProfileImageUrl, lightUserProfile.RepositoryCount, lightUserProfile.FollowerCount, lightUserProfile.FollowingCount, lightUserProfile.PrimaryProfileUrl, lightUserProfile.StarsGivenCount)
        {
            LastYearContributionsCount = lastYearContributionsCount;
            HasSpecialUserReadmeEnabled = hasSpecialUserReadmeEnabled;
        }

        public FullUserprofile(int lastYearContributionsCount, bool hasSpecialUserReadmeEnabled, string username, string name, string description, string organization, string location, string profileImageUrl, int repositoryCount, int followerCount, int followingCount, string primaryProfileUrl, int starsGivenCount) : base(username, name, description, organization, location, profileImageUrl, repositoryCount, followerCount, followingCount, primaryProfileUrl, starsGivenCount)
        {
            LastYearContributionsCount = lastYearContributionsCount;
            HasSpecialUserReadmeEnabled = hasSpecialUserReadmeEnabled;
        }

        public int LastYearContributionsCount { get; set; }

        public bool HasSpecialUserReadmeEnabled { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(LastYearContributionsCount)}={LastYearContributionsCount.ToString()}, {nameof(HasSpecialUserReadmeEnabled)}={HasSpecialUserReadmeEnabled.ToString()}, {nameof(Username)}={Username}, {nameof(Name)}={Name}, {nameof(Description)}={Description}, {nameof(Organization)}={Organization}, {nameof(Location)}={Location}, {nameof(ProfileImageUrl)}={ProfileImageUrl}, {nameof(RepositoryCount)}={RepositoryCount.ToString()}, {nameof(FollowerCount)}={FollowerCount.ToString()}, {nameof(FollowingCount)}={FollowingCount.ToString()}, {nameof(PrimaryProfileUrl)}={PrimaryProfileUrl}, {nameof(StarsGivenCount)}={StarsGivenCount.ToString()}}}";
        }
    }
}
