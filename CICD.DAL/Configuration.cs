using Dapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DAL
{
    public class Configuration : Interfaces.IConfiguration
    {
        private readonly Model.CICDContext _cicdContext;
        private readonly Mappers.IConfiguration _configurationMapper;

        public Configuration(Model.CICDContext cicdContext,
                             Mappers.IConfiguration configurationMapper)
        {
            this._cicdContext = cicdContext;
            this._configurationMapper = configurationMapper;
        }

        public IEnumerable<BO.Configuration> Get()
        {
            var queryBuilder = new QueryBuilder(new CultureInfo("en-us"), "yyyy/MM/dd HH:mm:ss");

            string query = queryBuilder.Select<Model.Configuration>(x => new { x.Id, x.ConfigurationTypeId, x.Value })
                           .Build();

            var dbConnection = this._cicdContext.Connection;

            IEnumerable<Model.Configuration> configurationsDB = dbConnection.Query<Model.Configuration>(query);
            IEnumerable<BO.Configuration> configurations = this._configurationMapper.DbsToBos(configurationsDB);

            return configurations;
        }

        public BO.Configuration GetByType(BO.ConfigurationType configurationType)
        {
            var queryBuilder = new QueryBuilder(new CultureInfo("en-us"), "yyyy/MM/dd HH:mm:ss");
            var equalToOperator = QueryBuilder.Operators.EqualTo;

            string query = queryBuilder.Select<Model.Configuration>(x => new { x.Id, x.ConfigurationTypeId, x.Value })
                                       .Where<Model.Configuration>(x => x.ConfigurationTypeId, equalToOperator, (int)configurationType)
                           .Build();

            var dbConnection = this._cicdContext.Connection;

            Model.Configuration? configurationDB = dbConnection.Query<Model.Configuration>(query).FirstOrDefault();
            BO.Configuration? configuration = this._configurationMapper.DbToBo(configurationDB);

            if (configuration == null)
                throw new BO.CustomExceptions.NotFoundException(new BO.ErrorData(configurationType));

            return configuration;
        }

        public void Insert(BO.Configuration configuration)
        {
            var queryBuilder = new QueryBuilder(new CultureInfo("en-us"), "yyyy/MM/dd HH:mm:ss");

            var dbConnection = this._cicdContext.Connection;

            string query = queryBuilder
                           .Insert<Model.Configuration>(dbConnection, x => new
                                { x.ConfigurationTypeId, x.Value },
                                (int)configuration.ConfigurationType, configuration.Value)
                           .Build();

            object configurationId = dbConnection.ExecuteScalar(query);

            configuration.Id = (int)(long)configurationId;
        }

        public void Update(BO.Configuration configuration)
        {
            var queryBuilder = new QueryBuilder(new CultureInfo("en-us"), "yyyy/MM/dd HH:mm:ss");
            var equalToOperator = QueryBuilder.Operators.EqualTo;

            string query = queryBuilder.Update<Model.Configuration>(x => new { x.Value }, configuration.Value)
                                       .Where<Model.Configuration>(x => x.Id, equalToOperator, configuration.Id)
                           .Build();

            var dbConnection = this._cicdContext.Connection;

            int rowNumber = dbConnection.Execute(query);

            if (rowNumber == 0)
                throw new BO.CustomExceptions.NotFoundException(new BO.ErrorData(configuration));
        }
    }
}
