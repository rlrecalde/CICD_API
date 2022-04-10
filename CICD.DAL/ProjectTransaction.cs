using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DAL
{
    public class ProjectTransaction : Interfaces.IProjectTransaction
    {
        private readonly Model.CICDContext _cicdContext;
        private readonly Interfaces.ICommit _commitData;
        private readonly Interfaces.IBranch _branchData;
        private readonly Interfaces.IProject _projectData;

        public ProjectTransaction(Model.CICDContext cicdContext,
                                  Interfaces.ICommit commitData,
                                  Interfaces.IBranch branchData,
                                  Interfaces.IProject projectData)
        {
            this._cicdContext = cicdContext;
            this._commitData = commitData;
            this._branchData = branchData;
            this._projectData = projectData;
        }

        public void Delete(BO.Project project)
        {
            DbTransaction? dbTransaction = null;

            try
            {
                dbTransaction = this._cicdContext.Connection.BeginTransaction();
                this._cicdContext.SetTransaction(dbTransaction);

                foreach (var branch in project.Branches)
                {
                    this._commitData.DeleteByBranchId(branch.Id);
                    this._branchData.Delete(branch);
                }

                this._projectData.Delete(project);

                dbTransaction.Commit();
            }
            catch (Exception)
            {
                if (dbTransaction != null)
                    dbTransaction.Rollback();

                throw;
            }
            finally
            {
                if (dbTransaction != null)
                    dbTransaction.Dispose();
            }
        }
    }
}
