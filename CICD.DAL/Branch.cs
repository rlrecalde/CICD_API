using Dapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DAL
{
    public class Branch : Interfaces.IBranch
    {
        private readonly Model.CICDContext _cicdContext;
        private readonly Mappers.IBranch _branchMapper;

        public Branch(Model.CICDContext cicdContext,
                      Mappers.IBranch branchMapper)
        {
            this._cicdContext = cicdContext;
            this._branchMapper = branchMapper;
        }

        public IEnumerable<BO.Branch> GetByProjectId(int projectId)
        {
            var queryBuilder = new QueryBuilder(new CultureInfo("en-us"), "yyyy/MM/dd HH:mm:ss");
            var equalToOperator = QueryBuilder.Operators.EqualTo;

            string query = queryBuilder.Select<Model.Branch>(x => new { x.Id, x.ProjectId, x.Name })
                                       .Where<Model.Branch>(x => x.ProjectId, equalToOperator, projectId)
                                       .And<Model.Branch>(x => x.Deleted, equalToOperator, false, false, false)
                           .Build();

            var dbConnection = this._cicdContext.Connection;

            IEnumerable<Model.Branch> branchesDB = dbConnection.Query<Model.Branch>(query);
            IEnumerable<BO.Branch> branches = this._branchMapper.DbsToBos(branchesDB);

            return branches;
        }

        public void Insert(BO.Branch branch)
        {
            var queryBuilder = new QueryBuilder(new CultureInfo("en-us"), "yyyy/MM/dd HH:mm:ss");

            var dbConnection = this._cicdContext.Connection;

            string query = queryBuilder
                           .Insert<Model.Branch>(dbConnection, x => new
                                { x.ProjectId, x.Name, x.Deleted },
                                branch.Project.Id, branch.Name, false)
                           .Build();

            object branchId = dbConnection.ExecuteScalar(query);

            branch.Id = (int)(long)branchId;
        }

        public void MarkAsDeleted(BO.Branch branch)
        {
            var queryBuilder = new QueryBuilder(new CultureInfo("en-us"), "yyyy/MM/dd HH:mm:ss");
            var equalToOperator = QueryBuilder.Operators.EqualTo;

            string query = queryBuilder.Update<Model.Branch>(x => new { x.Deleted }, true)
                                       .Where<Model.Branch>(x => x.Id, equalToOperator, branch.Id)
                           .Build();

            var dbConnection = this._cicdContext.Connection;

            int rowNumber = dbConnection.Execute(query);

            if (rowNumber == 0)
                throw new BO.CustomExceptions.NotFoundException(new BO.ErrorData(branch));
        }

        public void Delete(BO.Branch branch)
        {
            var queryBuilder = new QueryBuilder(new CultureInfo("en-us"), "yyyy/MM/dd HH:mm:ss");
            var equalToOperator = QueryBuilder.Operators.EqualTo;

            string query = queryBuilder.Delete(typeof(Model.Branch))
                                       .Where<Model.Branch>(x => x.Id, equalToOperator, branch.Id)
                           .Build();

            var dbConnection = this._cicdContext.Connection;
            var dbTransaction = this._cicdContext.GetTransaction();

            int rowNumber = dbConnection.Execute(query, transaction: dbTransaction);

            if (rowNumber == 0)
                throw new BO.CustomExceptions.NotFoundException(new BO.ErrorData(branch));
        }
    }
}
