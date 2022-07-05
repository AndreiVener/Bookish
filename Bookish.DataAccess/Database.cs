using System;
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

        public List<Account> GetAllAccounts()
        {
            var accounts = (List<Account>)db.Query<Account>("SELECT * FROM bookish.dbo.Accounts");

            return accounts.ToList();
        }

        public Book FindBookByName(string name)
        {
            var sqlFindBookByName = " SELECT * FROM bookish.dbo.books WHERE bookName = @bookName";
            List<Book> foundBooks = (List<Book>)db.Query<Book>(sqlFindBookByName, new
            {
                bookName = name
            });
            return foundBooks.First();
        }

        public DateTime GetBookDueDate(Account account, Book book)
        {
            var sqlFindBookByName = " SELECT dueDate FROM bookish.dbo.borrowedBooks WHERE bookID = @bookID AND userID = @userID";
            List<DateTime> foundDueDates = (List<DateTime>)db.Query<DateTime>(sqlFindBookByName, new
            {
                userID = account.AccountID,
                bookID = book.BookID
            });
            return foundDueDates.First();
        }
        
        public Book FindBookByID(int id)
        {
            var sqlFindBook = " SELECT * FROM bookish.dbo.books Where bookID = @bookID";
            var foundBooks = db.Query<Book>(sqlFindBook, new
            {
                bookID = id
            });
            return foundBooks.First();
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

        public void UpdateBook(int bookID, Book newBook)
        {
            string sql = "UPDATE bookish.dbo.books SET bookName=@bookName, bookAuthor=@bookAuthor, noCopies=@noCopies, ISBN=@ISBN, bookType=@bookType WHERE bookID=@bookID";

            db.Execute(sql, new
            {
                bookName = newBook.BookName,
                bookAuthor = newBook.BookAuthor,
                noCopies = newBook.NoCopies,
                ISBN = newBook.ISBN,
                BookType = newBook.BookType,
                bookID = bookID
            });
        }

        public List<Book> GetAllAvailableBooks()
        {
            var sqlSelectAllAvailableBooks = "SELECT * FROM bookish.dbo.books WHERE noCopies > 0";
            List<Book> availableBooks = (List<Book>)db.Query<Book>(sqlSelectAllAvailableBooks);
            return availableBooks;

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

        public List<BorrowedBooks> getBorrowedBooks(Account account)
        {
            var sqlFindBorrowedBooks = "SELECT * FROM bookish.dbo.borrowedBooks WHERE userID = @userID";
            List<BorrowedBooks> borrowedBooks = (List<BorrowedBooks>)db.Query<BorrowedBooks>(sqlFindBorrowedBooks, new
            {
                userID = account.AccountID
            });
            return borrowedBooks;
        }

        public bool borrowBook(Account account, Book book, DateTime borrowDate, DateTime dueDate)
        {
            if (book.NoCopies == 0)
            {
                return false;
            }

            book.NoCopies--;
            
            UpdateBook(book.BookID, book);
            
            string sql = "INSERT INTO bookish.dbo.borrowedBooks (userID, bookID, borrowDate, dueDate) Values (@userID,@bookID,@borrowDate,@dueDate)";
            db.Execute(sql, new
            {
                userID = account.AccountID,
                bookID = book.BookID,
                borrowDate = borrowDate,
                dueDate = dueDate
            });

            return true;
        }
    }
}