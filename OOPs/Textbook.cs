using System;
using System.Collections.Generic;
using System.Text;

namespace OOPs
{
    // Derived class 2: Textbook (inherits from Book - inheritance)
    internal class Textbook : Book
    {
        public string Subject { get; set; }

        public Textbook(string title, string author, int pageCount, string isbn, DateTime pubDate, string subject) : base(title, author, pageCount, isbn, pubDate)
        {
            Subject = subject;
        }


        // Implementing abstract method
        public override void DisplaySummary()
        {
            Console.WriteLine($"Textbook: {Title} on {Subject} by {Author}, Pages: {PageCount}, ISBN: {ISBN}");
        }


        // Overriding virtual method (polymorphism)
        public override void Read()
        {
            base.Read();
            Console.WriteLine("Taking notes on key concepts!");
        }
    }
}
