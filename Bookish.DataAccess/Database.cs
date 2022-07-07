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
        private static IDbConnection _db;
        
        public Database()
        {
            _db = new SqlConnection("Server=tcp:SEASLUG,1433;User Id=bookishService; Password=abcd;");
        }

        public List<Account> GetAllAccounts()
        {
            var accounts = (List<Account>)_db.Query<Account>("SELECT * FROM bookish.dbo.Accounts");

            return accounts.ToList();
        }

        public List<Book> FindBookByName(string name)
        {

            var sqlFindBookByName = $" SELECT * FROM bookish.dbo.books WHERE bookName LIKE '%{name}%' OR bookAuthor LIKE '%{name}%'";
            List<Book> foundBooks = (List<Book>)_db.Query<Book>(sqlFindBookByName);
            return foundBooks;
        }

        public DateTime GetBookDueDate(Account account, Book book)
        {
            var sqlFindBookByName = " SELECT dueDate FROM bookish.dbo.borrowedBooks WHERE bookID = @bookID AND userID = @userID";
            List<DateTime> foundDueDates = (List<DateTime>)_db.Query<DateTime>(sqlFindBookByName, new
            {
                userID = account.AccountID,
                bookID = book.BookID
            });
            return foundDueDates.First();
        }

        public Book FindBookByID(int id)
        {
            var sqlFindBook = " SELECT * FROM bookish.dbo.books Where bookID = @bookID";
            var foundBooks = _db.Query<Book>(sqlFindBook, new
            {
                bookID = id
            }).ToList();
            if (foundBooks.Count > 0)
            {
                return foundBooks.First();
            }

            return null;
        }

        public Account FindUserByID(int id)
        {
            var sqlFindAccount = " SELECT * FROM bookish.dbo.accounts Where accountID = @bookID";
            var foundAccounts = _db.Query<Account>(sqlFindAccount, new
            {
                bookID = id
            }).ToList();
            if (foundAccounts.Count > 0)
            {
                return foundAccounts.First();
            }

            return null;
        }
        
        public bool AddBook(Book book)
        {
            string sql = "INSERT INTO bookish.dbo.books(bookName, bookAuthor, noCopies, ISBN, bookType, description, imageURL) VALUES (@bookName, @bookAuthor, @noCopies, @ISBN, @bookType, @description, @imageURL)";

            _db.Execute(sql, new
            {
                bookName = book.BookName,
                bookAuthor = book.BookAuthor,
                noCopies = book.NoCopies,
                ISBN = book.ISBN,
                BookType = book.BookType,
                description = book.Description,
                imageURL = book.ImageURL
            });

            return true;
        }

        public void UpdateBook(int bookID, Book newBook)
        {
            string sql = "UPDATE bookish.dbo.books SET bookName=@bookName, bookAuthor=@bookAuthor, noCopies=@noCopies, ISBN=@ISBN, bookType=@bookType WHERE bookID=@bookID";

            _db.Execute(sql, new
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
            var sqlSelectAllAvailableBooks = "SELECT * FROM bookish.dbo.books WHERE noCopies > 0 ORDER BY bookName";
            List<Book> availableBooks = (List<Book>)_db.Query<Book>(sqlSelectAllAvailableBooks);
            return availableBooks;

        }

        public List<Book> GetPaginatedAvailableBooks(int page)
        {
            //each page contains 5 books
            var sqlSelectPaginatedAvailableBooks = "SELECT * FROM bookish.dbo.books WHERE noCopies > 0 ORDER BY bookName OFFSET " 
                                                   + (page * 5) + " ROWS FETCH NEXT 5 ROWS ONLY";
            List<Book> availableBooks = (List<Book>)_db.Query<Book>(sqlSelectPaginatedAvailableBooks);
            return availableBooks;
        }
        
        public void AddUser(Account account)
        {
            string sql = "INSERT INTO bookish.dbo.Accounts (AccountName,AccountPassword) Values (@AccountName,@AccountPassword)";
            _db.Execute(sql, new
            {
                AccountName = account.AccountName,
                AccountPassword = account.AccountPassword
            });
        }

        public List<Account> findUsers(string name)
        {
            var sqlFindAccountsName = " SELECT * FROM bookish.dbo.Accounts WHERE AccountName = @AccountName";
            List<Account> foundUsers = (List<Account>)_db.Query<Account>(sqlFindAccountsName, new
            {
                AccountName = name
            });
            return foundUsers;
        }
        
        public Account loginUser(string name, string password)
        {
            var sqlFindAccount = " SELECT * FROM bookish.dbo.Accounts WHERE AccountName = @AccountName AND AccountPassword = @AccountPassword";
            List<Account> foundUsers = (List<Account>)_db.Query<Account>(sqlFindAccount, new
            {
                AccountName = name,
                AccountPassword = password
            });

            if (foundUsers.Count > 0)
            {
                return foundUsers.First();
            }

            return null;
        }

        public List<BorrowedBooks> getBorrowedBooks(Account account)
        {
            var sqlFindBorrowedBooks = "SELECT * FROM bookish.dbo.borrowedBooks WHERE userID = @userID";
            List<BorrowedBooks> borrowedBooks = (List<BorrowedBooks>)_db.Query<BorrowedBooks>(sqlFindBorrowedBooks, new
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

            List<BorrowedBooks> borrowedBooks = getBorrowedBooks(account);

            foreach (var borrowedBook in borrowedBooks)
            {
                if (borrowedBook.BookID == book.BookID)
                {
                    return false;
                }
            }

            book.NoCopies--;
            
            UpdateBook(book.BookID, book);
            
            string sql = "INSERT INTO bookish.dbo.borrowedBooks (userID, bookID, borrowDate, dueDate) Values (@userID,@bookID,@borrowDate,@dueDate)";
            _db.Execute(sql, new
            {
                userID = account.AccountID,
                bookID = book.BookID,
                borrowDate = borrowDate,
                dueDate = dueDate
            });

            return true;
        }

        public bool unBorrowBook(Account account, Book borrowedBook)
        {
            string sql = "DELETE FROM bookish.dbo.borrowedBooks WHERE userID = @userID AND bookID = @bookID";
            _db.Execute(sql, new
            {
                userID = account.AccountID,
                bookID = borrowedBook.BookID
            });

            borrowedBook.NoCopies++;
            UpdateBook(borrowedBook.BookID, borrowedBook);

            return true;
        }

        public int GetNumberOfPages()
        {
            var sql = "SELECT COUNT(*) FROM bookish.dbo.books WHERE noCopies > 0";
            List<int> numberOfPages = (List<int>)_db.Query<int>(sql);
            return (int)Math.Ceiling((float)numberOfPages.First()/5);
        }
        
        public int GetNumberOfPages(string search)
        {
            var sqlFindBookByName = " SELECT COUNT(*) FROM bookish.dbo.books WHERE bookName = @bookName";
            List<int> foundBooks = (List<int>)_db.Query<int>(sqlFindBookByName, new
            {
                bookName = search
            });
            return foundBooks.First();
        }
    }
}