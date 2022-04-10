using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BLL
{
    public class Commit : Interfaces.ICommit
    {
        private readonly DAL.Interfaces.ICommit _commitData;

        public Commit(DAL.Interfaces.ICommit commitData)
        {
            this._commitData = commitData;
        }

        public BO.Commit GetLastCommitByBranchId(int branchId)
        {
            try
            {
                BO.Commit commit = this._commitData.GetLastCommitByBranchId(branchId);

                return commit;
            }
            catch (BO.CustomExceptions.CustomExceptionBase)
            {
                throw;
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(branchId, ex);
                throw new BO.CustomExceptions.UnexpectedException(errorData);
            }
        }

        public void Insert(BO.Commit commit)
        {
            try
            {
                this._commitData.Insert(commit);
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(commit, ex);
                throw new BO.CustomExceptions.UnexpectedException(errorData);
            }
        }
    }
}
