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
            db = new SqlConnection("Server=tcp:SEASLUG,1433;User Id=bookishService; Password=abcd;");
        }

        public List<Account> GetAllBooks()
        {
            var accounts = (List<Account>)db.Query<Account>("SELECT * FROM bookish.dbo.Accounts");

            return accounts.ToList();
        }

        public bool AddUser(string name, string pass)
        {
            Account account = new Account(name, pass);
            string sql = "INSERT INTO bookish.dbo.Accounts (AccountName,AccountPassword) Values (@AccountName,@AccountPassword)";
            db.Execute(sql, new
            {
                AccountName = name,
                AccountPassword = pass
            });
            return true;
        }
    }
}