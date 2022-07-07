using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using Bookish.DataAccess;
using Dapper;

namespace Bookish.ConsoleApp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Database db = new Database();

            Account account = db.findUsers("andrei")[0];

            DateTime borrowDate = DateTime.Now;
            DateTime dueDate = DateTime.Now;
            
            // db.borrowBook(account, db.FindBookByName("o carte"), borrowDate, dueDate);
            // db.borrowBook(account, db.FindBookByName("carte"), borrowDate, dueDate);

            // Console.WriteLine(db.GetBookDueDate(account, db.FindBookByName("Lupul")));
            // Console.WriteLine("ID PT " + account.AccountName + " -> " + account.AccountID);
            // var borrowedBooks = db.getBorrowedBooks(account);
            // foreach (var book in borrowedBooks)      
            // {
            //     Console.WriteLine(book.BookID);
            // }

            var borrowedBooks = db.getBorrowedBooks(account);
            foreach (var book in borrowedBooks)
            {
                //Console.WriteLine($"Book id: {book.BookID}\t{db.FindBookByID(book.BookID).BookName}");
            }

            //var b = new Book("Capra cu 3 iezi", "Autor", 0,"33333",1);
            //var b = new Book("Oaia cu 3 iezi", "Capra", 20,"22222",1);
            //db.AddBook(b);

            var all = db.GetAllAvailableBooks();
            foreach (var book in all)
            {
                //Console.WriteLine($"Book: {book.BookName} Cop: {book.NoCopies}");
            }
            
            Console.WriteLine(db.GetNumberOfPages());
        }
    }
}