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

        public bool AddBook(Book book)
        {
            string sql = "INSERT INTO bookish.dbo.books(bookName, bookAuthor, noCopies, ISBN, bookType) VALUES (@bookName, @bookAuthor, @noCopies, @ISBN, @bookType)";

            db.Execute(sql, new
            {
                bookName = book.BookName,
                bookAuthor = book.BookAuthor,
                noCopies = book.NoCopies,
                ISBN = book.ISBN,
                BookType = book.BookType
            });

            return true;
        }

        public void AddUser(Account account)
        {
            string sql = "INSERT INTO bookish.dbo.Accounts (AccountName,AccountPassword) Values (@AccountName,@AccountPassword)";
            db.Execute(sql, new
            {
                AccountName = account.AccountName,
                AccountPassword = account.AccountPassword
            });
        }

        public List<Account> findUsers(string name)
        {
            var sqlFindAccountsName = " SELECT * FROM bookish.dbo.Accounts Where AccountName = @AccountName";
            List<Account> foundUsers = (List<Account>)db.Query<Account>(sqlFindAccountsName, new
            {
                AccountName = name
            });
            return foundUsers;
        }


    }
}