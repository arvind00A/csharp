using System;
using System.Collections.Generic;
using System.Text;

namespace OOPs
{
    // Abstract base class demonstrating abstraction
    public abstract class Book
    {
        // Private field (access modifier: private - only accessible inside this class)
        private string _isbn;

        // Protected field (access modifier: protected - accessible in this class and derived classes)
        protected string Author {  get; set; }

        // Internal property (access modifier: internal - accessible within the same assembly/project)
        internal DateTime PublicationDate { get; set; }


        // Public property with full getter/setter (manual backing field + validation)
        public string Title
        {
            get { return _title; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Title cannot be empty");
                _title = value;
            }
        }

        private string _title; // Backing field for Title


        // Auto-implemented property (C# shorthand - compiler creates backing field)
        public int PageCount { get; set; }


        // Read-only property (can only be set in constructor or initializer)
        public string ISBN
        {
            get => _isbn;
        }


        // Write-only property (rare, but demonstrated - can only set, not get from outside)
        public string WriteOnlyNotes { private get; set; }


        // Constructor to initialize read-only and other properties
        protected Book(string title, string author, int pageCount, string isbn, DateTime pubDate)
        {
            Title = title;
            Author = author;
            PageCount = pageCount;
            _isbn = isbn;
            PublicationDate = pubDate;
        }

        // Abstract method (must be implemented by derived classes - abstraction)
        public abstract void DisplaySummary();


        // Virtual method (can be overridden - for polymorphism)
        public virtual void Read()
        {
            Console.WriteLine($"Reading book: {Title} by {Author}");
        }
    }
}
