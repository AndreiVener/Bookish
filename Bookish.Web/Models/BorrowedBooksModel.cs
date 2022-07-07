using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bookish.DataAccess;

namespace Bookish.Web.Models
{
    public class BorrowedBooksModel
    {
        public Dictionary<BorrowedBooks, Book> BorrowedBooksList { get; set; }

        public BorrowedBooksModel()
        {
            BorrowedBooksList= new Dictionary<BorrowedBooks, Book>();
        }
    }
}