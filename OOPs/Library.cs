using System;
using System.Collections.Generic;
using System.Text;

namespace OOPs
{
    // Class demonstrating Indexers
    public class Library
    {
        private List<Book> _books = new List<Book>();   // Private field

        // Indexer (allows array-like access: library[0])
        public Book this[int index]
        {
            get
            {
                if (index < 0 || index >= _books.Count)
                    throw new IndexOutOfRangeException("Invalid book index");
                return _books[index];
            }

            set
            {
                if (index < 0 || index >= _books.Count)
                    throw new IndexOutOfRangeException("Invalid book index");
                _books[index] = value;
                
            }
        }

        // Public method to add books
        public void AddBook(Book book)
        {
            _books.Add(book);
        }

        public int BookCount => _books.Count;   // Read-only property
    }
}
