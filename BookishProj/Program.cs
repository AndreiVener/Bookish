using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Bookish.DataAccess;
using Dapper;

namespace Bookish.ConsoleApp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Database db = new Database();
            Account account = new Account("tessttt","bubugaga");
            
            db.AddUser(account);
            var res = db.GetAllBooks();
            foreach (var account1 in res)
            {
                Console.WriteLine(account1.AccountName);
            }
            
            

        }
    }
}