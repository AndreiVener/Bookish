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
            var foundUsers = db.findUsers("andrei");
            foreach (var user in foundUsers)
            {
                Console.WriteLine(user.AccountName + " -> " + user.AccountPassword);
            }



        }
    }
}