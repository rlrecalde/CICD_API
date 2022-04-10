using Dapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DAL
{
    public class Commit : Interfaces.ICommit
    {
        private readonly Model.CICDContext _cicdContext;
        private readonly Mappers.ICommit _commitMapper;

        public Commit(Model.CICDContext cicdContext,
                      Mappers.ICommit commitMapper)
        {
            this._cicdContext = cicdContext;
            this._commitMapper = commitMapper;
        }

        public BO.Commit GetLastCommitByBranchId(int branchId)
        {
            var queryBuilder = new QueryBuilder(new CultureInfo("en-us"), "yyyy/MM/dd HH:mm:ss");
            var equalToOperator = QueryBuilder.Operators.EqualTo;
            var descendingOrder = QueryBuilder.Orders.Descending;

            var dbConnection = this._cicdContext.Connection;

            string query = queryBuilder.Select<Model.Commit>(x => new { x.Sha, x.BranchId, x.CommitterLogin, x.CommitterName, x.Date, x.Message }, top: 1)
                                       .Where<Model.Commit>(x => x.BranchId, equalToOperator, branchId)
                                       .OrderBy<Model.Commit>(x => x.Id, descendingOrder)
                           .Build();

            Model.Commit? commitDb = dbConnection.Query<Model.Commit>(query).FirstOrDefault();
            BO.Commit? commit = this._commitMapper.DbToBo(commitDb);

            if (commit == null)
                throw new BO.CustomExceptions.NotFoundException(new BO.ErrorData(branchId));

            return commit;
        }

        public void Insert(BO.Commit commit)
        {
            var queryBuilder = new QueryBuilder(new CultureInfo("en-us"), "yyyy/MM/dd HH:mm:ss");

            var dbConnection = this._cicdContext.Connection;

            string query = queryBuilder
                           .Insert<Model.Commit>(dbConnection, x => new
                                { x.BranchId, x.Sha, x.CommitterLogin, x.CommitterName, x.Date, x.Message },
                                commit.BranchId, commit.Id, commit.CommitterLogin, commit.CommitterName, commit.Date, commit.Message)
                           .Build();

            object commitId = dbConnection.ExecuteScalar(query);
        }

        public void DeleteByBranchId(int branchId)
        {
            var queryBuilder = new QueryBuilder(new CultureInfo("en-us"), "yyyy/MM/dd HH:mm:ss");
            var equalToOperator = QueryBuilder.Operators.EqualTo;

            string query = queryBuilder.Delete(typeof(Model.Commit))
                                       .Where<Model.Commit>(x => x.BranchId, equalToOperator, branchId)
                           .Build();

            var dbConnection = this._cicdContext.Connection;
            var dbTransaction = this._cicdContext.GetTransaction();

            int rowNumber = dbConnection.Execute(query, transaction: dbTransaction);

            if (rowNumber == 0)
                throw new BO.CustomExceptions.NotFoundException(new BO.ErrorData(branchId));
        }
    }
}
