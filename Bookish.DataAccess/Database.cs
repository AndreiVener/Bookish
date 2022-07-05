using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Bookish.DataAccess
{
    public class Database
    {
        private static IDbConnection db;
        
        public Database()
        {
            db = new SqlConnection("Server=localhost;User Id=bookishService; Password=1234;");
        }

        public List<Account> GetAllBooks()
        {
            var accounts = (List<Account>)db.Query<Account>("SELECT * FROM bookish.dbo.Accounts");

            return accounts.ToList();
        }
    }
}