using Dapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DAL
{
    public class Project : Interfaces.IProject
    {
        private readonly Model.CICDContext _cicdContext;
        private readonly Mappers.IProject _projectMapper;

        public Project(Model.CICDContext cicdContext,
                       Mappers.IProject projectMapper)
        {
            this._cicdContext = cicdContext;
            this._projectMapper = projectMapper;
        }

        public IEnumerable<BO.Project> GetByUserId(int userId)
        {
            var queryBuilder = new QueryBuilder(new CultureInfo("en-us"), "yyyy/MM/dd HH:mm:ss");
            var equalToOperator = QueryBuilder.Operators.EqualTo;

            string query = queryBuilder.Select<Model.Project>(x => new { x.Id, x.UserId, x.Name, x.RelativePath, x.DotnetVersion, x.Test, x.Deploy, x.DeployPort })
                                       .Where<Model.Project>(x => x.UserId, equalToOperator, userId)
                           .Build();

            var dbConnection = this._cicdContext.Connection;

            IEnumerable<Model.Project> projectsDB = dbConnection.Query<Model.Project>(query);
            IEnumerable<BO.Project> projects = this._projectMapper.DbsToBos(projectsDB);

            return projects;
        }

        public BO.Project GetById(int projectId)
        {
            var queryBuilder = new QueryBuilder(new CultureInfo("en-us"), "yyyy/MM/dd HH:mm:ss");
            var equalToOperator = QueryBuilder.Operators.EqualTo;

            string query = queryBuilder.Select<Model.Project>(x => new { x.Id, x.UserId, x.Name, x.RelativePath, x.DotnetVersion, x.Test, x.Deploy, x.DeployPort })
                                       .Where<Model.Project>(x => x.Id, equalToOperator, projectId)
                           .Build();

            var dbConnection = this._cicdContext.Connection;

            Model.Project? projectDB = dbConnection.Query<Model.Project>(query).FirstOrDefault();
            BO.Project? project = this._projectMapper.DbToBo(projectDB);

            if (project == null)
                throw new BO.CustomExceptions.NotFoundException(new BO.ErrorData(projectId));

            return project;
        }

        public BO.Project GetByName(string name)
        {
            var queryBuilder = new QueryBuilder(new CultureInfo("en-us"), "yyyy/MM/dd HH:mm:ss");
            var equalToOperator = QueryBuilder.Operators.EqualTo;

            string query = queryBuilder.Select<Model.Project>(x => new { x.Id, x.Name })
                                       .Where<Model.Project>(x => x.Name, equalToOperator, name)
                           .Build();

            var dbConnection = this._cicdContext.Connection;

            Model.Project? projectDB = dbConnection.Query<Model.Project>(query).FirstOrDefault();
            BO.Project? project = this._projectMapper.DbToBo(projectDB);

            if (project == null)
                throw new BO.CustomExceptions.NotFoundException(new BO.ErrorData(name));

            return project;
        }

        public void Insert(BO.Project project)
        {
            var queryBuilder = new QueryBuilder(new CultureInfo("en-us"), "yyyy/MM/dd HH:mm:ss");

            var dbConnection = this._cicdContext.Connection;

            string query = queryBuilder
                           .Insert<Model.Project>(dbConnection, x => new
                                { x.UserId, x.Name, x.RelativePath, x.DotnetVersion, x.Test, x.Deploy, x.DeployPort },
                                project.User.Id, project.Name, project.RelativePath, project.DotnetVersion, project.Test, project.Deploy, project.DeployPort)
                           .Build();

            object projectId = dbConnection.ExecuteScalar(query);

            project.Id = (int)(long)projectId;
        }

        public void Update(BO.Project project)
        {
            var queryBuilder = new QueryBuilder(new CultureInfo("en-us"), "yyyy/MM/dd HH:mm:ss");
            var equalToOperator = QueryBuilder.Operators.EqualTo;

            string query = queryBuilder.Update<Model.Project>(x => new 
                                            { x.DotnetVersion, x.Test, x.Deploy, x.DeployPort },
                                            project.DotnetVersion, project.Test, project.Deploy, project.DeployPort)
                                       .Where<Model.Project>(x => x.Id, equalToOperator, project.Id)
                           .Build();

            var dbConnection = this._cicdContext.Connection;

            int rowNumber = dbConnection.Execute(query);

            if (rowNumber == 0)
                throw new BO.CustomExceptions.NotFoundException(new BO.ErrorData(project));
        }

        public void Delete(BO.Project project)
        {
            var queryBuilder = new QueryBuilder(new CultureInfo("en-us"), "yyyy/MM/dd HH:mm:ss");
            var equalToOperator = QueryBuilder.Operators.EqualTo;

            string query = queryBuilder.Delete(typeof(Model.Project))
                                       .Where<Model.Project>(x => x.Id, equalToOperator, project.Id)
                           .Build();

            var dbConnection = this._cicdContext.Connection;
            var dbTransaction = this._cicdContext.GetTransaction();

            int rowNumber = dbConnection.Execute(query, transaction: dbTransaction);

            if (rowNumber == 0)
                throw new BO.CustomExceptions.NotFoundException(new BO.ErrorData(project));
        }
    }
}
