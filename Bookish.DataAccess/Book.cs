namespace Bookish.DataAccess
{
    public class Book
    {
        public int BookID { get; set; }
        
        public string BookName { get; set; }
        
        public string BookAuthor { get; set; }
        
        public int NoCopies { get; set; }
        
        public string ISBN { get; set; }
        
        public int BookType { get; set; }
        
        public string ImageURL { get; set; }
        
        public string Description { get; set; }

        public Book()
        {
        }
        
        public Book(string bookName, string bookAuthor, int noCopies, string isbn, int bookType)
        {
            this.BookName = bookName;
            this.BookAuthor = bookAuthor;
            this.NoCopies = noCopies;
            this.ISBN = isbn;
            this.BookType = bookType;
        }

        public Book(int bookID, string bookName, string bookAuthor, int noCopies, string isbn, int bookType)
        {
            this.BookID = bookID;
            this.BookName = bookName;
            this.BookAuthor = bookAuthor;
            this.NoCopies = noCopies;
            this.ISBN = isbn;
            this.BookType = bookType;
        }
    }
}