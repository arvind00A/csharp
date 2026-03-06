using System;
using System.Collections.Generic;
using System.Text;

namespace OOPs
{
    // Derived class 1: Novel (inherits from Book - inheritance)
    internal class Novel : Book
    {
        public string Genre { get; set; }

        public Novel(string title, string author, int pageCount, string isbn, DateTime pubDate, string genre) : base(title, author, pageCount, isbn, pubDate)
        {
            Genre = genre;
        }


        // Implementing abstract method (abstraction fulfillment)
        public override void DisplaySummary()
        {
            Console.WriteLine($"Novel: {Title} by {Author} ({Genre}), Pages: {PageCount}, ISBN: {ISBN}");
        }


        // Overriding virtual method (polymorphism - runtime)
        public override void Read()
        {
            base.Read();  // Call base version
            Console.WriteLine("Enjoying the story twists!");
        }
    }
}
