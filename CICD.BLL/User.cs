namespace CICD.BLL
{
    public class User : Interfaces.IUser
    {
        private readonly DAL.Interfaces.IUser _userData;
        private readonly Interfaces.IConfiguration _configurationBusiness;
        private readonly Validators.User _userValidator;

        public User(DAL.Interfaces.IUser userData,
                    Interfaces.IConfiguration configurationBusiness,
                    Validators.User userValidator)
        {
            this._userData = userData;
            this._configurationBusiness = configurationBusiness;
            this._userValidator = userValidator;
        }

        public IEnumerable<BO.User> Get()
        {
            IEnumerable<BO.User> users = new List<BO.User>();

            try
            {
                users = this._userData.Get();

                BO.Configuration configuration = this._configurationBusiness.GetByType(BO.ConfigurationType.DefaultUserId);
                BO.User? defaultUser = users.FirstOrDefault(x => x.Id == Convert.ToInt32(configuration.Value));

                if (defaultUser != null)
                    defaultUser.IsDefault = true;

                return users;
            }
            catch (BO.CustomExceptions.NotFoundException)
            {
                return users;
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(null, ex);
                throw new BO.CustomExceptions.UnexpectedException(errorData);
            }
        }

        public void Insert(BO.User user)
        {
            try
            {
                this._userValidator.Validate(user);
                this._userData.Insert(user);
            }
            catch (BO.CustomExceptions.CustomExceptionBase)
            {
                throw;
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(user, ex);
                throw new BO.CustomExceptions.UnexpectedException(errorData);
            }
        }

        public void Update(BO.User user)
        {
            try
            {
                this._userValidator.Validate(user);
                this._userData.Update(user);
            }
            catch (BO.CustomExceptions.CustomExceptionBase)
            {
                throw;
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(user, ex);
                throw new BO.CustomExceptions.UnexpectedException(errorData);
            }
        }

        public void Delete(BO.User user)
        {
            try
            {
                this._userData.Delete(user);
            }
            catch (BO.CustomExceptions.CustomExceptionBase)
            {
                throw;
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(user, ex);
                throw new BO.CustomExceptions.UnexpectedException(errorData);
            }
        }
    }
}