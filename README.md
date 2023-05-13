# GithubNet

## Introduction

GithubNet is a C# library that allows you to retrieve trending GitHub repositories and their information. Currently, it provides functionality to fetch the trending repositories and their details. The library is built using .NET Core 7 and will be available as a NuGet package.

## Technologies

- C# 
- .NET Core 7
- [HtmlAgilityPack](https://www.nuget.org/packages/HtmlAgilityPack)
- [CssSelectors.Core.HtmlAgilityPack](https://www.nuget.org/packages/CssSelectors.Core.HtmlAgilityPack)

## Features

- Retrieve trending GitHub repositories
- Access detailed information for each repository, including:
  - User
  - Repository link
  - Repository name
  - Description
  - Total stars
  - Total forks
  - Programming language
  - Availability of details
  - Archival status
  - Availability of project URL
  - Project URL
  - Availability of topics
  - Topics
  - Last commit time
  - Last commit URL

## Logo

The logo, which is used for the nuget-package was created by: https://uxwing.com

## Usage

To use the GithubNet library, follow these steps:

1. Create an instance of the `GithubNetClient` class.
2. Use the `GetTrendItemsAsync` method to retrieve a list of `TrendItem` objects representing trending repositories. You can optionally specify the `loadTrendItemDetails` parameter as `true` to load all the available details, but note that this process can be time-consuming as it crawls through each page.
3. If needed, you can use the `GetTrendItemDetailsAsync` method to fetch additional details for a specific `TrendItem` object.
4. Use the `GetTopicUrlFromTopicName` method to obtain the GitHub URL for a given topic.

The following methods are described in the text:

- `GetTrendItemsAsync`: This method retrieves a list of `TrendItem` objects representing trending repositories. It returns the list asynchronously and can optionally load all available details if the `loadTrendItemDetails` parameter is set to `true`.

- `GetTrendItemDetailsAsync`: This method fetches additional details for a specific `TrendItem` object. It is used when you need more information about a particular repository in the list of trending repositories obtained from `GetTrendItemsAsync`.

- `GetTopicUrlFromTopicName`: This method allows you to obtain the GitHub URL for a given topic. It takes the topic name as input and returns the corresponding URL.

## Example

For a demonstration of the library's functionality, refer to the included `GithubNetDemo` project. It showcases the console output of the crawled data using the `GetTrendItemsAsync` method.

## License

GithubNet is licensed under the GNU General Public License v3.0.

You can read the full license details of the GNU General Public License v3.0 [here](https://choosealicense.com/licenses/gpl-3.0/).

## Roadmap

The roadmap for future development includes the following planned features:

1. `GetRepositoryInfoAsync`: Implement a method to retrieve detailed information about a specific GitHub repository. This method will provide details such as the repository owner, description, stars, forks, programming language, and other relevant information.

2. `GetUserInfoAsync`: Develop a method to fetch information about a GitHub user. This method will allow users to retrieve details such as the user's name, bio, location, profile picture, number of followers, number of repositories, and other relevant information.

3. `GetAllTopicsAsync`: Create a method to fetch all available topics from the GitHub Topics page (https://github.com/topics). This method will provide a list of topics along with their corresponding URLs and other relevant information.

4. `GetAllCollectionsAsync`: Implement a method to retrieve information about the GitHub Collections page (https://github.com/collections). This method will allow users to obtain a list of collections, including their titles, descriptions, URLs, and other relevant details.

## Disclaimer

Please read the full disclaimer in the DISCLAIMER.md file before using this project. The author (liebki) of the project and the project itself are not endorsed by Microsoft and do not reflect the views or opinions of Microsoft or any individuals officially involved with the project.