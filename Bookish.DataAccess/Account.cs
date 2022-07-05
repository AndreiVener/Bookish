namespace Bookish.DataAccess
{
    public class Account
    {
        public int AccountID { get; set; }
        
        public string AccountName { get; set; }
        
        public string AccountPassword { get; set; }

        public string HashPassword()
        {
            return AccountPassword;
        }
    }
}