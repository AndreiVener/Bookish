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

        public Account()
        {
        }

        public Account(string name, string pass)
        {
            AccountName = name;
            AccountPassword = pass;
        }
    }
}