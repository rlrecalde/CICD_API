using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BLL
{
    public class GitHub : Interfaces.IGitHub
    {
        private readonly DAL.Interfaces.IGitHub _gitHubApi;

        public GitHub(DAL.Interfaces.IGitHub gitHubApi)
        {
            this._gitHubApi = gitHubApi;
        }

        public IEnumerable<BO.Branch> GetBranches(BO.Project project)
        {
            try
            {
                IEnumerable<BO.Branch> branches = this._gitHubApi.GetBranches(project);

                return branches;
            }
            catch (BO.CustomExceptions.CustomExceptionBase)
            {
                throw;
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(project, ex);
                throw new BO.CustomExceptions.ApiCallerException(errorData);
            }
        }

        public BO.Commit GetLastCommitByBranch(BO.Branch branch)
        {
            try
            {
                BO.Commit lastCommit = this._gitHubApi.GetLastCommitByBranch(branch);

                return lastCommit;
            }
            catch (BO.CustomExceptions.CustomExceptionBase)
            {
                throw;
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(branch, ex);
                throw new BO.CustomExceptions.ApiCallerException(errorData);
            }
        }

        public void SendComment(BO.Comment comment)
        {
            try
            {
                this._gitHubApi.SendComment(comment);
            }
            catch (BO.CustomExceptions.CustomExceptionBase)
            {
                throw;
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(comment, ex);
                throw new BO.CustomExceptions.ApiCallerException(errorData);
            }
        }
    }
}
