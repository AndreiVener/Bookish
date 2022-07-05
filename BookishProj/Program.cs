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
            var res = db.GetAllBooks();
            foreach (var account in res)
            {
                Console.WriteLine(account.AccountName);
            }

        }
    }
}