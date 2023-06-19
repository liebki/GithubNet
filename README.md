# GithubNet

## Introduction

GithubNet is a C# library that allows you to retrieve trending GitHub repositories and their information. Currently, it provides functionality to fetch the trending repositories and their details. The library is built using .NET Core 7 and will be available as a NuGet package.


## Technologies

- C# 
- .NET Core 7
- [HtmlAgilityPack](https://www.nuget.org/packages/HtmlAgilityPack)
- [CssSelectors.Core.HtmlAgilityPack](https://www.nuget.org/packages/CssSelectors.Core.HtmlAgilityPack)


## Features

Both the `Repository` and `TrendItem` classes inherit from a base class named `ItemBase`. The `ItemBase` class includes the following common properties:

- `User` (string): The username or owner of the repository.
- `RespositoryLink` (string): The URL or link to the repository.
- `RespositoryName` (string): The name of the repository.
- `Description` (string): A brief description or summary of the repository.
- `TotalStars` (integer): Indicates the total number of stars or favorites received by the repository.
- `TotalForks` (integer): Represents the total number of forks or copies of the repository.
- `HasProjectUrl` (boolean): Indicates whether the repository has a project URL.
- `ProjectUrl` (string): The URL or link to the project associated with the repository, if available.
- `HasTopics` (boolean): Indicates whether the repository has topics associated with it.
- `Topics` (string[]): An array of strings representing the topics or tags associated with the repository.

The `Repository` class includes additional properties specific to repositories:

- `OpenIssuesNumber` (integer): Represents the number of open issues in the repository.
- `OpenPullRequestsNumber` (integer): Indicates the number of open pull requests in the repository.
- `TotalCommitsNumber` (integer): Represents the total number of commits made in the repository.
- `TotalContributorsNumber` (integer): Indicates the total number of contributors to the repository.

The `TrendItem` class includes additional properties specific to trend items:

- `Programminglanguage` (string): The main programming language associated with the trend item.
- `HasDetails` (boolean): Indicates whether it's an intense crawl, where each repository is individually crawled.
- `IsArchived` (boolean): Indicates whether the repository associated with the trend item is archived.
- `LastCommitTime` (string): The timestamp or time of the last commit made to the repository.
- `LastCommitUrl` (string): The URL or link to the last commit made in the repository.


## Usage

The following methods can be used:

- `GetTrendItemsAsync`: This method retrieves a list of `TrendItem` objects representing trending repositories. It returns the list asynchronously and can optionally load all available details if the `loadTrendItemDetails` parameter is set to `true`.

- `GetTrendItemDetailsAsync`: This method fetches additional details for a specific `TrendItem` object. It is used when you need more information about a particular repository in the list of trending repositories obtained from `GetTrendItemsAsync`.

- `GetTopicUrlFromTopicName`: This method allows you to obtain the GitHub URL for a given topic. It takes the topic name as input and returns the corresponding URL.

- `GetRepositoryInfoAsync`: This method retrieves detailed information about a specific GitHub repository. It provides details such as the number of contributors, the number of open issues, and more. The method returns the repository information asynchronously.


## Example

For a demonstration of the library's functionality, refer to the included `GithubNetDemo` project. It showcases the console output of the crawled data using the `GetTrendItemsAsync` method.


## License

GithubNet is licensed under the GNU General Public License v3.0.

You can read the full license details of the GNU General Public License v3.0 [here](https://choosealicense.com/licenses/gpl-3.0/).


## Roadmap

The roadmap for future development includes the following planned features:

- Reduce the redundant code in the `GithubNetManager` class!

- `GetUserInfoAsync`: Develop a method to fetch information about a GitHub user. This method will allow users to retrieve details such as the user's name, bio, location, profile picture, number of followers, number of repositories, and other relevant information.

- `GetAllTopicsAsync`: Create a method to fetch all available topics from the GitHub Topics page (https://github.com/topics). This method will provide a list of topics along with their corresponding URLs and other relevant information.

- `GetAllCollectionsAsync`: Implement a method to retrieve information about the GitHub Collections page (https://github.com/collections). This method will allow users to obtain a list of collections, including their titles, descriptions, URLs, and other relevant details.


## Disclaimer

Please read the full disclaimer in the DISCLAIMER.md file before using this project. The author (liebki) of the project and the project itself are not endorsed by Microsoft and do not reflect the views or opinions of Microsoft or any individuals officially involved with the project.