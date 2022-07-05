using System;

namespace Bookish.DataAccess
{
    public class BorrowedBooks
    {
        public int UserID { set; get; }
        public int BookID { set; get; }
        public DateTime BorrowDate { set; get; }
        public DateTime DueDate { set; get; }
    }
}