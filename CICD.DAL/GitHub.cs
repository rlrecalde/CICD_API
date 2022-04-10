using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CICD.DAL
{
    public class GitHub : Interfaces.IGitHub
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        private readonly string _gitHubUrl;
        private readonly string _userEndpoint;
        private readonly string _contentsRoute;
        private readonly string _reposRoute;
        private readonly string _branchesRoute;
        private readonly string _commitsRoute;
        private readonly string _commentsRoute;

        private record class GitHubUser(long id, string login);
        private record class GitHubRepo(long id, string name);
        private record class GitHubCommiter(string name, DateTime date);
        private record class GitHubSubCommit(GitHubCommiter committer, string message);
        private record class GitHubCommit(string sha, GitHubSubCommit commit, GitHubUser committer);
        private record class GitHubBranch(string name, GitHubCommit commit);
        private record class GitHubContent(string name, string type);

        public GitHub(IHttpClientFactory httpClientFactory,
                      IOptions<BO.AppSettings> options)
        {
            this._httpClientFactory = httpClientFactory;

            this._httpClient = this._httpClientFactory.CreateClient();
            this._httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this._httpClient.DefaultRequestHeaders.Add("User-Agent", @"Mozilla/5.0 (Windows NT 10; Win64; x64; rv:60.0) Gecko/20100101 Firefox/60.0");

            this._gitHubUrl = options.Value.GitHubUrl;
            this._userEndpoint = options.Value.UserEndpoint;
            this._contentsRoute = options.Value.ContentsRoute;
            this._reposRoute = options.Value.ReposRoute;
            this._branchesRoute = options.Value.BranchesRoute;
            this._commitsRoute = options.Value.CommitsRoute;
            this._commentsRoute = options.Value.CommentsRoute;
        }

        public bool UserAuthentication(BO.User user)
        {
            try
            {
                string url = $"{this._gitHubUrl}/{this._userEndpoint}";
                this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
                HttpResponseMessage httpResponseMessage = this._httpClient.GetAsync(url).Result;

                if (!httpResponseMessage.IsSuccessStatusCode)
                    throw Exception<BO.CustomExceptions.ApiCallerException>($"{httpResponseMessage.ReasonPhrase} ({(int)httpResponseMessage.StatusCode})", url);

                string responseResult = httpResponseMessage.Content.ReadAsStringAsync().Result;
                var gitHubUser = JsonSerializer.Deserialize<GitHubUser>(responseResult);

                if (gitHubUser == null || gitHubUser.login != user.Name)
                    throw Exception<BO.CustomExceptions.ConflictException>("Wrong authentication", user);

                return true;
            }
            catch (BO.CustomExceptions.CustomExceptionBase)
            {
                throw;
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(user, ex);
                throw new BO.CustomExceptions.ApiCallerException(errorData);
            }
        }

        public bool FileExists(BO.Project project)
        {
            try
            {
                string url = $"{this._gitHubUrl}/{this._reposRoute}/{project.User.Name}/{project.Name}/{this._contentsRoute}/{project.RelativePath}";
                this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", project.User.Token);
                HttpResponseMessage httpResponseMessage = this._httpClient.GetAsync(url).Result;

                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                        throw Exception<BO.CustomExceptions.NotFoundException>($"File '{project.RelativePath}' does not exist", url);

                    throw Exception<BO.CustomExceptions.ApiCallerException>($"{httpResponseMessage.ReasonPhrase} ({(int)httpResponseMessage.StatusCode})", url);
                }

                string responseResult = httpResponseMessage.Content.ReadAsStringAsync().Result;
                var gitHubContent = JsonSerializer.Deserialize<GitHubContent>(responseResult);

                if (gitHubContent == null || gitHubContent.type != "file")
                    throw Exception<BO.CustomExceptions.UnexpectedException>("Unexpected response", project);

                return true;
            }
            catch (BO.CustomExceptions.CustomExceptionBase)
            {
                throw;
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(project, ex);
                throw new BO.CustomExceptions.ApiCallerException(errorData);
            }
        }

        public IEnumerable<BO.Project> GetProjects(BO.User user)
        {
            try
            {
                var projects = new List<BO.Project>();

                string url = $"{this._gitHubUrl}/{this._userEndpoint}/{this._reposRoute}";
                this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
                HttpResponseMessage httpResponseMessage = this._httpClient.GetAsync(url).Result;

                if (!httpResponseMessage.IsSuccessStatusCode)
                    throw Exception<BO.CustomExceptions.ApiCallerException>($"{httpResponseMessage.ReasonPhrase} ({(int)httpResponseMessage.StatusCode})", url);

                string responseResult = httpResponseMessage.Content.ReadAsStringAsync().Result;
                var gitHubRepos = JsonSerializer.Deserialize<IEnumerable<GitHubRepo>>(responseResult);

                if (gitHubRepos == null)
                    throw Exception<BO.CustomExceptions.UnexpectedException>("Response issues", new { user, responseResult });

                foreach (var gitHubRepo in gitHubRepos)
                    projects.Add(new BO.Project { Name = gitHubRepo.name });

                return projects;
            }
            catch (BO.CustomExceptions.CustomExceptionBase)
            {
                throw;
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(user, ex);
                throw new BO.CustomExceptions.ApiCallerException(errorData);
            }
        }

        public IEnumerable<BO.Branch> GetBranches(BO.Project project)
        {
            var branches = new List<BO.Branch>();

            string url = $"{this._gitHubUrl}/{this._reposRoute}/{project.User.Name}/{project.Name}/{this._branchesRoute}";
            this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", project.User.Token);
            HttpResponseMessage httpResponseMessage = this._httpClient.GetAsync(url).Result;

            if (!httpResponseMessage.IsSuccessStatusCode)
                throw Exception<BO.CustomExceptions.ApiCallerException>($"{httpResponseMessage.ReasonPhrase} ({(int)httpResponseMessage.StatusCode})", url);

            string responseResult = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var gitHubBranches = JsonSerializer.Deserialize<IEnumerable<GitHubBranch>>(responseResult);

            if (gitHubBranches == null)
                throw Exception<BO.CustomExceptions.UnexpectedException>("Response issues", new { project, responseResult });

            foreach (var gitHubBranch in gitHubBranches)
                branches.Add(new BO.Branch { Name = gitHubBranch.name, LastCommit = new BO.Commit { Id = gitHubBranch.commit.sha }, Project = project });

            return branches;
        }

        public BO.Commit GetLastCommitByBranch(BO.Branch branch)
        {
            string url = $"{this._gitHubUrl}/{this._reposRoute}/{branch.Project.User.Name}/{branch.Project.Name}/{this._branchesRoute}/{branch.Name}";
            this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", branch.Project.User.Token);
            HttpResponseMessage httpResponseMessage = this._httpClient.GetAsync(url).Result;

            if (!httpResponseMessage.IsSuccessStatusCode)
                throw Exception<BO.CustomExceptions.ApiCallerException>($"{httpResponseMessage.ReasonPhrase} ({(int)httpResponseMessage.StatusCode})", url);

            string responseResult = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var gitHubBranch = JsonSerializer.Deserialize<GitHubBranch>(responseResult);

            if (gitHubBranch == null)
                throw Exception<BO.CustomExceptions.UnexpectedException>("Response issues", new { branch, responseResult });

            var commit = new BO.Commit { Id = gitHubBranch.commit.sha, CommitterLogin = gitHubBranch.commit.committer.login, CommitterName = gitHubBranch.commit.commit.committer.name, Date = gitHubBranch.commit.commit.committer.date, Message = gitHubBranch.commit.commit.message };

            return commit;
        }

        public void SendComment(BO.Comment comment)
        {
            string url = $"{this._gitHubUrl}/{this._reposRoute}/{comment.Branch.Project.User.Name}/{comment.Branch.Project.Name}/{this._commitsRoute}/{comment.Branch.LastCommit.Id}/{this._commentsRoute}";
            var body = new { body = comment.Text };
            this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", comment.Branch.Project.User.Token);

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponseMessage = this._httpClient.PostAsync(url, content).Result;

            if (!httpResponseMessage.IsSuccessStatusCode)
                throw Exception<BO.CustomExceptions.ApiCallerException>($"{httpResponseMessage.ReasonPhrase} ({(int)httpResponseMessage.StatusCode})", url);
        }

        #region Private Methods

        private T Exception<T>(string message, object data) where T : BO.CustomExceptions.CustomExceptionBase, new()
        {
            var errorData = new BO.ErrorData
            {
                Message = message,
                Data = data,
            };

            var exception = new T();
            exception.ErrorData = errorData;

            return exception;
        }

        #endregion
    }
}
