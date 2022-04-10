using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BLL
{
    public class Git : Interfaces.IGit
    {
        private readonly Interfaces.IProcess _processBusiness;
        private readonly string _workingDirectory;

        public Git(Interfaces.IProcess processBusiness,
                   IOptions<BO.AppSettings> options)
        {
            this._processBusiness = processBusiness;
            this._workingDirectory = options.Value.WorkingDirectory;
        }

        public async Task Clone(BO.Project project)
        {
            var process = new BO.Process
            {
                Name = "git",
                WorkingDirectory = this._workingDirectory,
                Arguments = $"clone https://{project.User.Token}@github.com/{project.User.Name}/{project.Name}.git",
            };

            BO.ProcessResponse processResponse = await this._processBusiness.ExecuteAsync(process);

            if (processResponse.Sucess == false)
                throw this.UnexpectedException(processResponse, "git clone command execution error");
        }

        public async Task Fetch(BO.Project project)
        {
            var process = new BO.Process
            {
                Name = "git",
                WorkingDirectory = $"{this._workingDirectory}\\{project.Name}",
                Arguments = "fetch",
            };

            BO.ProcessResponse processResponse = await this._processBusiness.ExecuteAsync(process);

            if (processResponse.Sucess == false)
                throw this.UnexpectedException(processResponse, "git fetch command execution error");
        }

        public async Task Switch(BO.Branch branch)
        {
            var process = new BO.Process
            {
                Name = "git",
                WorkingDirectory = $"{this._workingDirectory}\\{branch.Project.Name}",
                Arguments = $"switch {branch.Name}",
            };

            BO.ProcessResponse processResponse = await this._processBusiness.ExecuteAsync(process);

            if (processResponse.Sucess == false)
                throw this.UnexpectedException(processResponse, "git switch command execution error");
        }

        public async Task Pull(BO.Project project)
        {
            var process = new BO.Process
            {
                Name = "git",
                WorkingDirectory = $"{this._workingDirectory}\\{project.Name}",
                Arguments = "pull",
            };

            BO.ProcessResponse processResponse = await this._processBusiness.ExecuteAsync(process);

            if (processResponse.Sucess == false)
                throw this.UnexpectedException(processResponse, "git pull command execution error");
        }

        public async Task<BO.Branch> GetHeadBranch(BO.Project project)
        {
            var process = new BO.Process
            {
                Name = "git",
                WorkingDirectory = $"{this._workingDirectory}\\{project.Name}",
                Arguments = "branch --remotes",
            };

            BO.ProcessResponse processResponse = await this._processBusiness.ExecuteAsync(process, redirectOutput: true);

            if (processResponse.Sucess == false)
                throw this.UnexpectedException(processResponse, "git branch --remotes command execution error");

            string branchName = this.GetHeadBranchName(processResponse.DataLines);
            var branch = new BO.Branch
            {
                Name = branchName,
                LastCommit = new BO.Commit(),
                Project = new BO.Project { User = new BO.User() },
            };

            return branch;
        }

        #region Private Methods

        private BO.CustomExceptions.UnexpectedException UnexpectedException(BO.ProcessResponse processResponse, string errorMessage)
        {
            var errorData = new BO.ErrorData
            {
                Message = errorMessage,
                Data = processResponse.ErrorLines,
            };

            return new BO.CustomExceptions.UnexpectedException(errorData);
        }

        private string GetHeadBranchName(IEnumerable<string> dataLines)
        {
            foreach (var dataLine in dataLines)
            {
                if (string.IsNullOrEmpty(dataLine))
                    continue;

                if (dataLine.Contains("HEAD"))
                {
                    string[] data = dataLine.Split("->");
                    string[] branch = data[1].Split("/");

                    return branch[1];
                }
            }

            return string.Empty;
        }

        #endregion
    }
}
