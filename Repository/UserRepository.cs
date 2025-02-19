using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using UsersAuth.Models;

namespace UsersAuth.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<User> RegisterUser(string username, string email, string password)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "EXEC [dbo].[User_Register] @Username, @Password, @Email";

                var userId = await dbConnection.ExecuteScalarAsync<int>(sqlQuery, new { Username = username, Email = email, Password = password });
                return new User
                {
                    Id = userId,
                    Username = username,
                    Email = email,
                    Password = password
                };
            }
        }

        public async Task<User> GetUserByUsername(string username)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "SELECT * FROM Users WHERE Username = @Username";
                return await dbConnection.QueryFirstOrDefaultAsync<User>(sqlQuery, new { Username = username });
            }
        }

        public async Task<User> GetUserById(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "SELECT * FROM Users WHERE Id = @Id";
                return await dbConnection.QueryFirstOrDefaultAsync<User>(sqlQuery, new { Id = id });
            }
        }

        public async Task<User> AuthenticateUser(string username, string password)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "EXEC [dbo].[User_Login] @Username, @Password";
                return await dbConnection.QueryFirstOrDefaultAsync<User>(sqlQuery, new { Username = username, Password = password });
            }
        }
    }
}
