using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BLL
{
    public class Dotnet : Interfaces.IDotnet
    {
        private readonly Interfaces.IProcess _processBusiness;
        private readonly string _workingDirectory;

        public Dotnet(Interfaces.IProcess processBusiness,
                      IOptions<BO.AppSettings> options)
        {
            this._processBusiness = processBusiness;
            this._workingDirectory = options.Value.WorkingDirectory;
        }

        public async Task<IEnumerable<string>> GetVersions()
        {
            var process = new BO.Process
            {
                Name = "dotnet",
                WorkingDirectory = "C:\\",
                Arguments = "sdk check",
            };

            BO.ProcessResponse processResponse = await this._processBusiness.ExecuteAsync(process, redirectOutput: true);

            if (processResponse.Sucess == false)
                throw this.UnexpectedException(processResponse.ErrorLines, "dotnet sdk check command execution error");

            IEnumerable<string> versions = this.GetSdkVersions(processResponse.DataLines);

            return versions;
        }

        public async Task Build(BO.Project project)
        {
            string projectFullName = $"{this._workingDirectory}\\{project.Name}\\{project.RelativePath}";
            string framework = this.GetFramework(project.DotnetVersion);

            var process = new BO.Process
            {
                Name = "dotnet",
                WorkingDirectory = "C:\\",
                Arguments = $"build \"{projectFullName}\" -f {framework}  -c Release",
            };

            BO.ProcessResponse processResponse = await this._processBusiness.ExecuteAsync(process, redirectOutput: true);

            if (processResponse.Sucess == false)
                throw this.UnexpectedException(processResponse.DataLines.Union(processResponse.ErrorLines), "dotnet build command execution error");
        }

        public async Task Test(BO.Project project)
        {
            string solutionDirectory = $"{this._workingDirectory}\\{project.Name}";
            string framework = this.GetFramework(project.DotnetVersion);

            var process = new BO.Process
            {
                Name = "dotnet",
                WorkingDirectory = solutionDirectory,
                Arguments = $"test -f {framework}",
            };

            BO.ProcessResponse processResponse = await this._processBusiness.ExecuteAsync(process, redirectOutput: true);

            if (processResponse.Sucess == false)
                throw this.UnexpectedException(processResponse.DataLines.Union(processResponse.ErrorLines), "dotnet test command execution error");
        }

        #region Private Methods

        private IEnumerable<string> GetSdkVersions(IEnumerable<string> dataLines)
        {
            var sdkVersions = new List<string>();

            foreach (var dataLine in dataLines)
            {
                if (string.IsNullOrEmpty(dataLine))
                    continue;

                int firstDotIndex = dataLine.IndexOf('.');

                if (firstDotIndex > 0 && int.TryParse(dataLine.Substring(0, firstDotIndex), out int firstNumber))
                {
                    int secondDotIndex = dataLine.IndexOf('.', firstDotIndex + 1);

                    if (secondDotIndex > firstDotIndex && int.TryParse(dataLine.Substring(firstDotIndex + 1, secondDotIndex - (firstDotIndex + 1)), out int secondNumber))
                    {
                        string sdkVersion = $"{firstNumber}.{secondNumber}";

                        if (!sdkVersions.Any(x => x == sdkVersion))
                            sdkVersions.Add(sdkVersion);
                    }
                }
            }

            return sdkVersions;
        }

        private string GetFramework(string dotnetVersion)
        {
            string framework = $"net{dotnetVersion}";

            decimal version = decimal.Parse(dotnetVersion);
            if (version < 4.0M)
                framework = $"netcoreapp{dotnetVersion}";

            return framework;
        }

        private BO.CustomExceptions.UnexpectedException UnexpectedException(IEnumerable<string> lines, string errorMessage)
        {
            var errorData = new BO.ErrorData
            {
                Message = errorMessage,
                Data = lines,
            };

            return new BO.CustomExceptions.UnexpectedException(errorData);
        }

        #endregion
    }
}
