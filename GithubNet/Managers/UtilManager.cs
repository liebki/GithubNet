﻿using GithubNet.Models.Extra;
using GithubNet.Models.Repositories;
using HtmlAgilityPack;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GithubNet.Managers
{
    internal static class UtilManager
    {

        private static readonly RegexOptions regexOptions = RegexOptions.Compiled;
        private static readonly Regex TooMuchWhitespaceRegex = new("[ ]{2,}", regexOptions, TimeSpan.FromMilliseconds(50));

        public static TopValues GetTopValues(IEnumerable<UserRepository> userRepositories)
        {
            Dictionary<string, (int Stars, int Forks, int RepoCount)> repositoryStats = new Dictionary<string, (int, int, int)>();

            int totalStars = 0;
            int totalForks = 0;

            foreach (UserRepository repo in userRepositories)
            {
                totalStars += repo.TotalStars;
                totalForks += repo.TotalForks;

                if (!repositoryStats.TryGetValue(repo.MainLanguage, out (int Stars, int Forks, int RepoCount) value))
                {
                    repositoryStats[repo.MainLanguage] = (repo.TotalStars, repo.TotalForks, 1);
                }
                else
                {
                    (int Stars, int Forks, int RepoCount) stats = value;
                    repositoryStats[repo.MainLanguage] = (stats.Stars + repo.TotalStars, stats.Forks + repo.TotalForks, stats.RepoCount + 1);
                }
            }

            string topLanguage = repositoryStats.OrderByDescending(kv => kv.Value.RepoCount).FirstOrDefault().Key;
            TopValues topValues = new(topLanguage, totalStars, totalForks);

            return topValues;
        }

        internal static string GetUsernameFromGitHubUrl(string url)
        {
            if (IsGitHubRepositoryUrl(url))
            {
                string[] parts = url.Split('/');
                return parts[3];
            }
            else
            {
                return "None";
            }
        }

        private static bool IsGitHubRepositoryUrl(string url)
        {
            return url.StartsWith("https://github.com/") && url.Count(c => c == '/') >= 4;
        }

        internal static HtmlDocument GetHtmlDoc(string Url, bool IsGithubPage = false)
        {
            string RequestUrl = Url;
            if (!Url.StartsWith("https://github.com") && IsGithubPage)
            {
                RequestUrl = $"https://github.com{RequestUrl}";
            }

            HtmlWeb Page = new()
            {
                OverrideEncoding = Encoding.UTF8
            };

            HtmlDocument PageDocument = Page.Load(RequestUrl);
            return PageDocument;
        }

        internal static int ParseNumberValue(string value)
        {
            value = value.Replace(",", string.Empty);

            if (value.EndsWith("k", StringComparison.OrdinalIgnoreCase))
            {
                if (double.TryParse(value.Substring(0, value.Length - 1), NumberStyles.Any, CultureInfo.InvariantCulture, out double numericValue))
                {
                    return (int)(numericValue * 1000);
                }
                else
                {
                    throw new ArgumentException("Invalid numeric format");
                }
            }
            else
            {
                if (int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out int numericValue))
                {
                    return numericValue;
                }
                else
                {
                    throw new ArgumentException("Invalid numeric format");
                }
            }
        }

        internal static string ClearStrings(string textIn, bool NormalizeWhitespace = false, string RemoveThis = "")
        {
            StringBuilder sb = new(textIn.Length);
            foreach (char i in textIn)
            {
                if (i != '\n' && i != '\r' && i != '\t')
                {
                    sb.Append(i);
                }
            }

            string ReturnText = sb.ToString().Trim();
            if (NormalizeWhitespace)
            {
                ReturnText = TooMuchWhitespaceRegex.Replace(ReturnText, @" ");
            }

            if (!string.IsNullOrEmpty(RemoveThis))
            {
                ReturnText = ReturnText.Replace(RemoveThis, string.Empty);
            }

            return ReturnText;
        }

        internal static string ParseTopicNameToUrl(string topicNameIn)
        {
            string LanguageName = NormalizeLanguageNameIdentifier(topicNameIn);
            return $"https://github.com/topics/{LanguageName}";
        }

        internal static string NormalizeLanguageNameIdentifier(string topicNameIn)
        {
            string normalized = topicNameIn.Replace(" ", "-").ToLowerInvariant();
            return normalized switch
            {
                "c#" => "csharp",
                "c++" => "cpp",
                _ => normalized,
            };
        }

    }
}