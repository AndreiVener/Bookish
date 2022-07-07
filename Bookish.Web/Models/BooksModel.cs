using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using Bookish.DataAccess;

namespace Bookish.Web.Models
{
    public class BooksModel
    {
        public List<Book> books;

        public int atPage { get; set; }
        public int numberOfPages { get; set; }

        public BooksModel()
        {
            books = new List<Book>();
            atPage = 0;
        }
    }
}