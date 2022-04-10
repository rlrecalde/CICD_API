using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BLL
{
    public class Shell : Interfaces.IShell
    {
        private readonly Interfaces.IProcess _processBusiness;
        private readonly string _workingDirectory;
        private readonly string _dockerizerFullFileName;
        private readonly bool _useWsl;

        public Shell(Interfaces.IProcess processBusiness,
                     IOptions<BO.AppSettings> options)
        {
            this._processBusiness = processBusiness;

            var appSettings = options.Value;
            this._workingDirectory = appSettings.WorkingDirectory;
            this._dockerizerFullFileName = appSettings.DockerizerFullFileName;
            this._useWsl = appSettings.UseWsl;
        }

        public async Task Dockerize(BO.Project project)
        {
            string arguments = this.GetProcessArguments(project);

            var process = new BO.Process
            {
                Name = "powershell",
                WorkingDirectory = "C:\\",
                Arguments = arguments,
            };

            BO.ProcessResponse processResponse = await this._processBusiness.ExecuteAsync(process, redirectOutput: true);

            if (processResponse.Sucess == false)
                throw this.UnexpectedException(processResponse.DataLines.Union(processResponse.ErrorLines), "powershell command execution error");
        }

        #region Private Methods

        private string GetProcessArguments(BO.Project project)
        {
            var snakeCaseResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() };

            string projectDirectory = project.RelativePath.Split('.')[0];
            string projectPath = $"{this._workingDirectory}\\{projectDirectory}";
            string imageName = snakeCaseResolver.GetResolvedPropertyName(project.Name);
            string port = project.DeployPort.ToString();
            string extension = "." + project.RelativePath.Split('.').Last();
            string framework = project.DotnetVersion;

            string arguments = $"\"{this._dockerizerFullFileName}";
            arguments += $" -path {projectPath}";
            arguments += $" -imageName {imageName}";
            arguments += $" -port {port}";
            arguments += $" -extension {extension}";
            arguments += $" -framework {framework}";

            if (decimal.TryParse(framework, out var version) && version >= 6)
                arguments += " -loggingConsole 1";

            if (this._useWsl)
                arguments += " -wsl 1";

            arguments += $"\"";

            return arguments;
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
