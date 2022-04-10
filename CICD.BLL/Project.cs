using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BLL
{
    public class Project : Interfaces.IProject
    {
        private readonly DAL.Interfaces.IProject _projectData;
        private readonly DAL.Interfaces.IProjectTransaction _projectTransactionData;
        private readonly Interfaces.IBranch _branchBusiness;
        private readonly Interfaces.IProcess _processBusiness;
        private readonly Validators.Project _projectValidator;
        private readonly string _workingDirectory;

        public Project(DAL.Interfaces.IProject projectData,
                       DAL.Interfaces.IProjectTransaction projectTransactionData,
                       Interfaces.IBranch branchBusiness,
                       Interfaces.IProcess processBusiness,
                       Validators.Project projectValidator,
                       IOptions<BO.AppSettings> options)
        {
            this._projectData = projectData;
            this._projectTransactionData = projectTransactionData;
            this._branchBusiness = branchBusiness;
            this._processBusiness = processBusiness;
            this._projectValidator = projectValidator;
            this._workingDirectory = options.Value.WorkingDirectory;
        }

        public IEnumerable<BO.Project> GetByUserId(int userId)
        {
            try
            {
                IEnumerable<BO.Project> projects = this._projectData.GetByUserId(userId);

                return projects;
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(userId, ex);
                throw new BO.CustomExceptions.UnexpectedException(errorData);
            }
        }

        public void Insert(BO.Project project)
        {
            try
            {
                this._projectValidator.Validate(project);
                project.RelativePath = project.RelativePath.Replace("/", "\\");

                this._projectData.Insert(project);
            }
            catch (BO.CustomExceptions.CustomExceptionBase)
            {
                throw;
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(project, ex);
                throw new BO.CustomExceptions.UnexpectedException(errorData);
            }
        }

        public void Update(BO.Project project)
        {
            try
            {
                this._projectData.Update(project);
            }
            catch (BO.CustomExceptions.CustomExceptionBase)
            {
                throw;
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(project, ex);
                throw new BO.CustomExceptions.UnexpectedException(errorData);
            }
        }

        public async Task Delete(BO.Project project)
        {
            try
            {
                project = this._projectData.GetById(project.Id);
                project.Branches = this._branchBusiness.GetByProjectId(project.Id);
                
                this._projectTransactionData.Delete(project);

                await this.RemoveFolder(project);
            }
            catch (BO.CustomExceptions.CustomExceptionBase)
            {
                throw;
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(project, ex);
                throw new BO.CustomExceptions.UnexpectedException(errorData);
            }
        }

        #region Private Methods

        private async Task RemoveFolder(BO.Project project)
        {
            var process = new BO.Process
            {
                Name = "cmd.exe",
                WorkingDirectory = this._workingDirectory,
                Arguments = $"/C rd {project.Name} /s /q",
            };

            BO.ProcessResponse processResponse = await this._processBusiness.ExecuteAsync(process, redirectOutput: true);

            if (processResponse.Sucess == false)
                throw this.UnexpectedException(processResponse, "rd command execution error");
        }

        private BO.CustomExceptions.UnexpectedException UnexpectedException(BO.ProcessResponse processResponse, string errorMessage)
        {
            var errorData = new BO.ErrorData
            {
                Message = errorMessage,
                Data = processResponse.ErrorLines,
            };

            return new BO.CustomExceptions.UnexpectedException(errorData);
        }

        #endregion
    }
}
