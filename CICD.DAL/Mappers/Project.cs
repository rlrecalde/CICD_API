using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DAL.Mappers
{
    public class Project : IProject
    {
        public BO.Project? DbToBo(Model.Project? projectModel)
        {
            if (projectModel == null)
                return null;

            var projectBo = new BO.Project
            {
                Id = projectModel.Id,
                User = new BO.User { Id = projectModel.UserId },
                Name = projectModel.Name,
                RelativePath = projectModel.RelativePath,
                DotnetVersion = projectModel.DotnetVersion,
                Test = projectModel.Test,
                Deploy = projectModel.Deploy,
                DeployPort = projectModel.DeployPort,
            };

            return projectBo;
        }

        public Model.Project? BoToDb(BO.Project? projectBo)
        {
            if (projectBo == null)
                return null;

            var projectModel = new Model.Project
            {
                Id = projectBo.Id,
                UserId = projectBo.User.Id,
                Name = projectBo.Name,
                RelativePath = projectBo.RelativePath,
                DotnetVersion = projectBo.DotnetVersion,
                Test = projectBo.Test,
                Deploy = projectBo.Deploy,
                DeployPort= projectBo.DeployPort,
            };

            return projectModel;
        }

        public IEnumerable<BO.Project> DbsToBos(IEnumerable<Model.Project> projectModels)
        {
            var projectBos = new List<BO.Project>();

            foreach (var projectModel in projectModels)
            {
                BO.Project? projectBo = this.DbToBo(projectModel);

                if (projectBo != null)
                    projectBos.Add(projectBo);
            }

            return projectBos;
        }

        public IEnumerable<Model.Project> BosToDbs(IEnumerable<BO.Project> projectBos)
        {
            var projectModels = new List<Model.Project>();

            foreach (var projectBo in projectBos)
            {
                Model.Project? projectModel = this.BoToDb(projectBo);

                if (projectModel != null)
                    projectModels.Add(projectModel);
            }

            return projectModels;
        }
    }
}
