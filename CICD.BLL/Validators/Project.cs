using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BLL.Validators
{
    public class Project : Interfaces.IValidator<BO.Project>
    {
        private readonly DAL.Interfaces.IGitHub _gitHubApi;
        private readonly DAL.Interfaces.IProject _projectData;

        public Project(DAL.Interfaces.IGitHub gitHubApi,
                       DAL.Interfaces.IProject projectData)
        {
            this._gitHubApi = gitHubApi;
            this._projectData = projectData;
        }

        public void Validate(BO.Project project)
        {
            this.ValidateExistingProject(project);
            this.ValidateProjectOnGitHub(project);
            this._gitHubApi.FileExists(project);
        }

        #region Private Methods

        private void ValidateExistingProject(BO.Project project)
        {
            BO.Project? existingProject = null;

            try
            {
                existingProject = this._projectData.GetByName(project.Name);
            }
            catch (BO.CustomExceptions.NotFoundException) { }

            if (existingProject != null)
                throw new BO.CustomExceptions.ConflictException(new BO.ErrorData(project));
        }

        private void ValidateProjectOnGitHub(BO.Project project)
        {
            IEnumerable<BO.Project> projects = this._gitHubApi.GetProjects(project.User);

            if (!projects.Any(x => x.Name == project.Name))
            {
                var errorData = new BO.ErrorData
                {
                    Message = "Project does not exist",
                    StackTrace = Environment.StackTrace,
                    Data = project,
                };

                throw new BO.CustomExceptions.ConflictException(errorData);
            }
        }

        #endregion
    }
}
