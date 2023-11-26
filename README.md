# GithubNet


## Introduction

Github(Data)Net is a simple C# library, using HtmlAgilityPack to retrieve several things from GitHub, things like trending repositories, profiles of users, the repositories of users and related information.


## Features ⭐

- **Trending Repositories:** Retrieve trending repositories.
- **Repository Details:** Obtain detailed information about repositories.
- **User Profiles:** Access full and light GitHub user profiles.
- **User Repositories:** Retrieve repositories associated with a GitHub user.
- **Topic URL:** Obtain the GitHub URL for a specific topic.


## Usage 🔧


### General

The GithubNetClient needs to be used to access all functionalities:

```csharp
GithubNetClient client = new();
```


### Trending Repositories

```csharp
IEnumerable<TrendRepository> GetAllTrendingRepositories(string customQuery = "https://github.com/trending");
IEnumerable<string> GetUrlsOfTrendingRepositories(string customQuery = "https://github.com/trending");
IEnumerable<FullRepository> GetFullTrendingRepositories(string customQuery = "https://github.com/trending");
```


### User Profiles

```csharp
FullUserprofile GetFullUserprofile(string Username);
LightUserprofile GetLightUserprofile(string Username);
```


### Full Repositories

```csharp
IEnumerable<FullRepository> GetFullRepositories(string Username);
FullRepository GetFullRepository(string RepositoryUrl);
```


### LightUserprofile, UserRepositories and TopValues of User

```csharp
(LightUserprofile Userprofile, IEnumerable<UserRepository> UserRepositories, TopValues topValues) GetLightUserprofileWithRepositories(string Username);
```


### Utils

```csharp
string GetTopicUrlFromTopicName(string topicName);
```


## Types 🔖

- **FullUserprofile:** Represents a comprehensive user profile with additional information such as last year's contributions count and whether the user has a special readme enabled. This type is based on `LightUserprofile`.

- **LightUserprofile:** Represents a lightweight user profile with essential information like username, name, description, etc.

- **UserRepository:** Represents a user's repository with additional details like whether it's a fork, license text, and last update information. This type is based on `RepositoryBase`.

- **TrendRepository:** Represents a trending repository with information about stars received today, main language, total stars, total forks, etc. This type is based on `RepositoryBase`.

- **RepositoryBase:** Base class for repositories, containing shared information like main language, total stars, total forks, username, repository name, and description.

- **FullRepository:** Represents a comprehensive repository with additional details such as project URL, open issues count, open pull requests count, total commits count, etc. This type is based on `RepositoryBase`.

- **TopValues:** Contains top values such as the top language, total stars count, and total forks count across user repositories.


## Example ✍🏻

For a demonstration of the library's functionality, refer to the included `GithubNetDemo` project.


## License 📜

GithubNet is licensed under the GNU General Public License v3.0.

You can read the full license details of the GNU General Public License v3.0 [here](https://choosealicense.com/licenses/gpl-3.0/).


## Disclaimer ⚠️

Please read the full disclaimer in the DISCLAIMER.md file before using this project. 
The author (liebki) of the project and the project itself are not endorsed by Microsoft and do not reflect the views or opinions of Microsoft or any individuals officially involved with the project.
The author of this library is not responsible for any incorrect or inappropriate usage. Please ensure that you use this library in accordance with its intended purpose and guidelines.