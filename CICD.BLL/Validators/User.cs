using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BLL.Validators
{
    public class User : Interfaces.IValidator<BO.User>
    {
        private readonly DAL.Interfaces.IGitHub _gitHubApi;
        private readonly DAL.Interfaces.IUser _userData;

        public User(DAL.Interfaces.IGitHub gitHubApi,
                    DAL.Interfaces.IUser userData)
        {
            this._gitHubApi = gitHubApi;
            this._userData = userData;
        }

        public void Validate(BO.User user)
        {
            this.ValidateExistingUser(user);
            this._gitHubApi.UserAuthentication(user);
        }

        #region Private Methods

        private void ValidateExistingUser(BO.User user)
        {
            BO.User? existingUser = null;

            try
            {
                existingUser = this._userData.GetByName(user.Name);
            }
            catch (BO.CustomExceptions.NotFoundException) { }

            if (existingUser != null)
                throw new BO.CustomExceptions.ConflictException(new BO.ErrorData(user));
        }

        #endregion
    }
}
