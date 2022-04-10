using Dapper;
using System.Globalization;

namespace CICD.DAL
{
    public class User : Interfaces.IUser
    {
        private readonly Model.CICDContext _cicdContext;
        private readonly Mappers.IUser _userMapper;

        public User(Model.CICDContext cicdContext,
                    Mappers.IUser userMapper)
        {
            this._cicdContext = cicdContext;
            this._userMapper = userMapper;
        }

        public IEnumerable<BO.User> Get()
        {
            var queryBuilder = new QueryBuilder(new CultureInfo("en-us"), "yyyy/MM/dd HH:mm:ss");

            string query = queryBuilder.Select<Model.User>(x => new { x.Id, x.Name, x.Token })
                           .Build();

            var dbConnection = this._cicdContext.Connection;

            IEnumerable<Model.User> usersDB = dbConnection.Query<Model.User>(query);
            IEnumerable<BO.User> users = this._userMapper.DbsToBos(usersDB);

            return users;
        }

        public BO.User GetByName(string name)
        {
            var queryBuilder = new QueryBuilder(new CultureInfo("en-us"), "yyyy/MM/dd HH:mm:ss");
            var equalToOperator = QueryBuilder.Operators.EqualTo;

            string query = queryBuilder.Select<Model.User>(x => new { x.Id, x.Name, x.Token })
                                       .Where<Model.User>(x => x.Name, equalToOperator, name)
                           .Build();

            var dbConnection = this._cicdContext.Connection;

            Model.User? userDB = dbConnection.Query<Model.User>(query).FirstOrDefault();
            BO.User? user = this._userMapper.DbToBo(userDB);

            if (user == null)
                throw new BO.CustomExceptions.NotFoundException(new BO.ErrorData(name));

            return user;
        }

        public void Insert(BO.User user)
        {
            var queryBuilder = new QueryBuilder(new CultureInfo("en-us"), "yyyy/MM/dd HH:mm:ss");

            var dbConnection = this._cicdContext.Connection;

            string query = queryBuilder
                           .Insert<Model.User>(dbConnection, x => new
                                { x.Name, x.Token },
                                user.Name, user.Token)
                           .Build();

            object userId = dbConnection.ExecuteScalar(query);

            user.Id = (int)(long)userId;
        }

        public void Update(BO.User user)
        {
            var queryBuilder = new QueryBuilder(new CultureInfo("en-us"), "yyyy/MM/dd HH:mm:ss");
            var equalToOperator = QueryBuilder.Operators.EqualTo;

            string query = queryBuilder.Update<Model.User>(x => new { x.Name, x.Token }, user.Name, user.Token)
                                       .Where<Model.User>(x => x.Id, equalToOperator, user.Id)
                           .Build();

            var dbConnection = this._cicdContext.Connection;

            int rowNumber = dbConnection.Execute(query);

            if (rowNumber == 0)
                throw new BO.CustomExceptions.NotFoundException(new BO.ErrorData(user));
        }

        public void Delete(BO.User user)
        {
            var queryBuilder = new QueryBuilder(new CultureInfo("en-us"), "yyyy/MM/dd HH:mm:ss");
            var equalToOperator = QueryBuilder.Operators.EqualTo;

            string query = queryBuilder.Delete(typeof(Model.User))
                                       .Where<Model.User>(x => x.Id, equalToOperator, user.Id)
                           .Build();

            var dbConnection = this._cicdContext.Connection;

            int rowNumber = dbConnection.Execute(query);

            if (rowNumber == 0)
                throw new BO.CustomExceptions.NotFoundException(new BO.ErrorData(user));
        }
    }
}