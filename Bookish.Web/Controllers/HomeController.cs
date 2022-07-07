using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using Bookish.DataAccess;
using Bookish.Web.Models;

namespace Bookish.Web.Controllers
{
    public class HomeController : Controller
    {
        private static Database db = new Database();

        public ActionResult Index()
        {
            return View();
        }

        public bool verifyUserIntegrity()
        {
            return Request.Cookies["userIDTest"] != null;
        }
        
        public ActionResult Index2(string login_txt, string password_txt)
        {
            Account account = db.loginUser(login_txt, password_txt);
            
            if (account != null)
            {
                Response.SetCookie(new HttpCookie("userIDTest", account.AccountID.ToString()));
                Response.Cookies["userIDTest"].Value = account.AccountID.ToString();
                Response.Redirect("/Home/Books");
            }
            
            return View("Index");
        }

        public ActionResult Logout()
        {
            Response.Cookies["userIDTest"].Expires = DateTime.Now.AddDays(-1);

            return View("Index");
        }
        
        public ActionResult Register()
        {
            return View();
        }
        
        public ActionResult Register2([Form] string login_txt, string password_txt, string repassword_txt)
        {
            if (login_txt != null && password_txt != null && password_txt == repassword_txt)
            {
                db.AddUser(new Account(login_txt, password_txt));
            }

            return View("Index");
        }
        
        public ActionResult AddBook()
        {
            if (!verifyUserIntegrity())
            {
                return View("Index");
            }
            
            return View();
        }

        public ActionResult AddBook2([Form] string name, string author, int nocopies, string isbn, string description, string imageLink)
        {
            
            if (!verifyUserIntegrity())
            {
                return View("Index");
            }
            
            int isPaper = 0;

            if (Request.Form["booktype"] == "Paper")
            {
                isPaper = 1;
            }

            Book book = new Book(name, author, nocopies, isbn, isPaper);
            book.Description = description;
            book.ImageURL = imageLink;
            db.AddBook(book);

            return View("Index");
        }
        
        public ActionResult Account()
        {
            if (!verifyUserIntegrity())
            {
                return View("Index");
            }
            
            BorrowedBooksModel model = new BorrowedBooksModel();
            Account account = getCurrentAccount();
            List<BorrowedBooks> borrowedBooksList = db.getBorrowedBooks(account);

            Console.WriteLine(borrowedBooksList.Count);
            
            foreach (var borrowedBook in borrowedBooksList)
            {
                model.BorrowedBooksList.Add(borrowedBook, db.FindBookByID(borrowedBook.BookID));
            }
            
            return View("Account", model);
        }

        public ActionResult Account2([Form] int borrowedBookID, int accountID)
        {
            if (!verifyUserIntegrity())
            {
                return View("Index");
            }
            
            BorrowedBooksModel model = new BorrowedBooksModel();
            //db.
            
            return View("Account", model);
        }
        
        public ActionResult ViewBook([Form] int bookID)
        {
            Book book = db.FindBookByID(bookID);
            
            return View("ViewBook", book);
        }
        
        public ActionResult Books([Form] string search, BooksModel booksModel)
        {
            if (!verifyUserIntegrity())
            {
                return View("Index");
            }

            if (!string.IsNullOrEmpty(search))
            {
                booksModel.books = db.FindBookByName(search);
                booksModel.numberOfPages = db.GetNumberOfPages(search);
                return View("Books", booksModel);
            }

            booksModel.books = db.GetPaginatedAvailableBooks(booksModel.atPage);
            booksModel.numberOfPages = db.GetNumberOfPages();

            return View("Books", booksModel);
        }

        public ActionResult BooksPage([Form] int page)
        {
            if (!verifyUserIntegrity())
            {
                return View("Index");
            }
            
            BooksModel booksModel = new BooksModel();

            booksModel.books = db.GetPaginatedAvailableBooks(page);

            return View("Books", booksModel);
        }

        public ActionResult Borrow([Form] int bookID)
        {
            Account account = getCurrentAccount();
            
            db.borrowBook(account, db.FindBookByID(bookID), DateTime.Now, DateTime.Today.AddDays(10));
            
            Response.Redirect("/Home/Account");
            return View("Account");
        }
        
        public ActionResult UnBorrow([Form] int borrowedBookID)
        {
            Account account = getCurrentAccount();
            db.unBorrowBook(account, db.FindBookByID(borrowedBookID));
            
            Response.Redirect("/Home/Account");
            return View("Account");
        }

        public ActionResult About()
        {
            if (!verifyUserIntegrity())
            {
                return View("Index");
            }
            
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            if (!verifyUserIntegrity())
            {
                return View("Index");
            }
            
            ViewBag.Message = "Your contact page.";
            return View();
        }

        private Account getCurrentAccount()
        {
            return db.FindUserByID(Convert.ToInt32(Request.Cookies["userIDTest"].Value));
        }
    }
}