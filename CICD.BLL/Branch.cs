using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BLL
{
    public class Branch : Interfaces.IBranch
    {
        private readonly DAL.Interfaces.IBranch _branchData;

        public Branch(DAL.Interfaces.IBranch branchData)
        {
            this._branchData = branchData;
        }

        public IEnumerable<BO.Branch> GetByProjectId(int projectId)
        {
            try
            {
                IEnumerable<BO.Branch> branches = this._branchData.GetByProjectId(projectId);

                return branches;
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(projectId, ex);
                throw new BO.CustomExceptions.UnexpectedException(errorData);
            }
        }

        public void Insert(BO.Branch branch)
        {
            try
            {
                this._branchData.Insert(branch);
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(branch, ex);
                throw new BO.CustomExceptions.UnexpectedException(errorData);
            }
        }

        public void InsertMany(IEnumerable<BO.Branch> branches)
        {
            try
            {
                foreach (var branch in branches)
                    this._branchData.Insert(branch);
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(branches, ex);
                throw new BO.CustomExceptions.UnexpectedException(errorData);
            }
        }

        public void MarkAsDeleted(BO.Branch branch)
        {
            try
            {
                this._branchData.MarkAsDeleted(branch);
            }
            catch (BO.CustomExceptions.CustomExceptionBase)
            {
                throw;
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(branch, ex);
                throw new BO.CustomExceptions.UnexpectedException(errorData);
            }
        }

        public void MarkManyAsDeleted(IEnumerable<BO.Branch> branches)
        {
            try
            {
                foreach (var branch in branches)
                    this._branchData.MarkAsDeleted(branch);
            }
            catch (BO.CustomExceptions.CustomExceptionBase)
            {
                throw;
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(branches, ex);
                throw new BO.CustomExceptions.UnexpectedException(errorData);
            }
        }
    }
}
