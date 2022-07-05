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
    }
}