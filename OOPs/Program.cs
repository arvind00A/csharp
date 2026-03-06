// See https://aka.ms/new-console-template for more information
using OOPs;


// Create library
Library library = new Library();

// Create books (inheritance + constructors)
Novel novel = new Novel("The Great Gatsby", "F. Scott Fitzgerald", 180, "978-0743273565", new DateTime(1925, 4, 10), "Classic");
Textbook textbook = new Textbook("C# in Depth", "Jon Skeet", 500, "978-1617294532", new DateTime(2019, 3, 1), "Programming");


// Add to library (using indexer)
library.AddBook(novel);
library.AddBook(textbook);


// Demonstrate polymorphism
foreach (Book book in new Book[] {novel, textbook })
{
    book.DisplaySummary();  // Abstract method call
    book.Read();            // Polymorphic virtual method
    Console.WriteLine();
}

// Access via indexer
Console.WriteLine("First book in library:");
library[0].DisplaySummary();

// Demonstrate properties
novel.Title = "Updated Title";  // Setter with validation
// novel.Title = " ";           // Would throw exception
novel.WriteOnlyNotes = "Secret note";  // Write-only
// Console.WriteLine(novel.WriteOnlyNotes);  // Compile error (write-only)

// Access modifiers in action
Console.WriteLine($"Publication Date (internal): {novel.PublicationDate:yyyy-MM-dd}");
Console.WriteLine($"ISBN (read-only): {novel.ISBN}");
Console.WriteLine($"Page Count (auto-implemented): {novel.PageCount}");

// textbook is internal, so accessible here (same assembly)
// If in another assembly, couldn't create Textbook instance

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();