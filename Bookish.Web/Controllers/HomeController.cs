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
        private Account account = db.findUsers("andrei").First();
        
        public ActionResult Index()
        {
            return View();
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
            return View();
        }

        public ActionResult AddBook2([Form] string name, string author, int nocopies, string isbn)
        {
            int isPaper = 0;

            if (Request.Form["booktype"] == "Paper")
            {
                isPaper = 1;
            }

            db.AddBook(new Book(name, author, nocopies, isbn,  isPaper));

            return View("Index");
        }
        
        public ActionResult Account()
        {
            BorrowedBooksModel model = new BorrowedBooksModel();
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
            BorrowedBooksModel model = new BorrowedBooksModel();
            //db.
            
            return View("Account", model);
        }
        
        public ActionResult Books([Form] string search)
        {
            BooksModel booksModel = new BooksModel();
            booksModel.atPage = 0;
            booksModel.numberOfPages = db.GetNumberOfPages();
            
            if (search != null)
            {
                booksModel.books.Add(db.FindBookByName(search));

                return View("Books", booksModel);
            }

            booksModel.books = db.GetPaginatedAvailableBooks(0);

            return View("Books", booksModel);
        }

        public ActionResult Books2([Form] int newPage, int maxPage)
        {
            BooksModel booksModel = new BooksModel();
            
            booksModel.atPage = newPage;
            booksModel.numberOfPages = maxPage;

            booksModel.books = db.GetPaginatedAvailableBooks(booksModel.atPage);
            
            return View("Books", booksModel);
        }
        
        public ActionResult Books3(BooksModel booksModel)
        {
            booksModel.books = db.GetPaginatedAvailableBooks(booksModel.atPage);
            
            return View("Books", booksModel);
        }

        public ActionResult BooksPage([Form] int page)
        {
            BooksModel booksModel = new BooksModel();

            booksModel.books = db.GetPaginatedAvailableBooks(page);

            return View("Books", booksModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}