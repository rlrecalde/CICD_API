using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BLL
{
    public class Configuration : Interfaces.IConfiguration
    {
        private readonly DAL.Interfaces.IConfiguration _configurationBusiness;

        public Configuration(DAL.Interfaces.IConfiguration configurationBusiness)
        {
            this._configurationBusiness = configurationBusiness;
        }

        public IEnumerable<BO.Configuration> Get()
        {
            try
            {
                IEnumerable<BO.Configuration> configurations = this._configurationBusiness.Get();

                return configurations;
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(null, ex);
                throw new BO.CustomExceptions.UnexpectedException(errorData);
            }
        }

        public BO.Configuration GetByType(BO.ConfigurationType configurationType)
        {
            try
            {
                BO.Configuration configuration = this._configurationBusiness.GetByType(configurationType);

                return configuration;
            }
            catch (BO.CustomExceptions.CustomExceptionBase)
            {
                throw;
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(configurationType, ex);
                throw new BO.CustomExceptions.UnexpectedException(errorData);
            }
        }

        public void Insert(BO.Configuration configuration)
        {
            try
            {
                this._configurationBusiness.Insert(configuration);
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(configuration, ex);
                throw new BO.CustomExceptions.UnexpectedException(errorData);
            }
        }

        public void Update(BO.Configuration configuration)
        {
            try
            {
                this._configurationBusiness.Update(configuration);
            }
            catch (BO.CustomExceptions.CustomExceptionBase)
            {
                throw;
            }
            catch (Exception ex)
            {
                var errorData = new BO.ErrorData(configuration, ex);
                throw new BO.CustomExceptions.UnexpectedException(errorData);
            }
        }
    }
}
