using System;
using System.Linq;

namespace GithubNet.Models.Userprofiles
{
    public class LightUserprofile
    {
        public LightUserprofile(string username, string name, string description, string organization, string location, string profileImageUrl, int repositoryCount, int followerCount, int followingCount, string primaryProfileUrl, int starsGivenCount)
        {
            Username = username;
            Name = name;
            Description = description;
            Organization = organization;
            Location = location;
            ProfileImageUrl = profileImageUrl;
            RepositoryCount = repositoryCount;
            FollowerCount = followerCount;
            FollowingCount = followingCount;
            PrimaryProfileUrl = primaryProfileUrl;
            StarsGivenCount = starsGivenCount;
        }

        public string Username { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Organization { get; set; }

        public string Location { get; set; }

        public string ProfileImageUrl { get; set; }

        public int RepositoryCount { get; set; }

        public int FollowerCount { get; set; }

        public int FollowingCount { get; set; }

        public string PrimaryProfileUrl { get; set; }

        public int StarsGivenCount { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(Username)}={Username}, {nameof(Name)}={Name}, {nameof(Description)}={Description}, {nameof(Organization)}={Organization}, {nameof(Location)}={Location}, {nameof(ProfileImageUrl)}={ProfileImageUrl}, {nameof(RepositoryCount)}={RepositoryCount.ToString()}, {nameof(FollowerCount)}={FollowerCount.ToString()}, {nameof(FollowingCount)}={FollowingCount.ToString()}, {nameof(PrimaryProfileUrl)}={PrimaryProfileUrl}, {nameof(StarsGivenCount)}={StarsGivenCount.ToString()}}}";
        }
    }
}
