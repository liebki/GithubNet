using System;
using System.Linq;

namespace GithubNet.Models.Extra
{
    public class TopValues
    {
        public TopValues(string topLanguage, int allStarsCount, int allForksCount)
        {
            TopLanguage = topLanguage;
            AllStarsCount = allStarsCount;
            AllForksCount = allForksCount;
        }

        public string TopLanguage { get; set; }
        public int AllStarsCount { get; set; }
        public int AllForksCount { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(TopLanguage)}={TopLanguage}, {nameof(AllStarsCount)}={AllStarsCount.ToString()}, {nameof(AllForksCount)}={AllForksCount.ToString()}}}";
        }
    }
}
