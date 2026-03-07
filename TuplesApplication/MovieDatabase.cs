// ============================================================
//  PROJECT 4 — Tuples
//  Mini Movie Database
//  Topics: Named tuples, returning multiple values,
//          deconstruction, tuple arrays, discards (_)
// ============================================================

using System;

class MovieDatabase
{
    static void Main()
    {
        Console.WriteLine("╔══════════════════════════════╗");
        Console.WriteLine("║   Mini Movie Database 🎬     ║");
        Console.WriteLine("╚══════════════════════════════╝\n");

        // ── 1. Basic Tuple ────────────────────────────────────
        Console.WriteLine("=== 1. Basic Tuple ===");

        // Positional tuple (less readable — use named instead)
        (string, int, double) movieBasic = ("Inception", 2010, 8.8);
        Console.WriteLine($"Title: {movieBasic.Item1}, Year: {movieBasic.Item2}, Rating: {movieBasic.Item3}");

        // Named tuple (MUCH better!) ✅
        (string Title, int Year, double Rating) movie = ("Inception", 2010, 8.8);
        Console.WriteLine($"Title: {movie.Title}, Year: {movie.Year}, Rating: {movie.Rating}");

        // ── 2. Array of Tuples = lightweight "database" ───────
        Console.WriteLine("\n=== 2. Movie Library ===");

        // Each tuple: (Title, Year, Genre, Rating, Duration in minutes)
        (string Title, int Year, string Genre, double Rating, int Duration)[] movies =
        {
            ("The Shawshank Redemption", 1994, "Drama",     9.3, 142),
            ("The Dark Knight",          2008, "Action",    9.0, 152),
            ("Inception",                2010, "Sci-Fi",    8.8, 148),
            ("Interstellar",             2014, "Sci-Fi",    8.6, 169),
            ("The Matrix",               1999, "Action",    8.7, 136),
            ("Parasite",                 2019, "Thriller",  8.5, 132),
            ("Forrest Gump",             1994, "Drama",     8.8, 142),
        };

        // Print all movies
        Console.WriteLine($"{"#",-3} {"Title",-30} {"Year",4} {"Genre",-10} {"Rating",6} {"Duration",9}");
        Console.WriteLine(new string('─', 70));

        for (int i = 0; i < movies.Length; i++)
        {
            var m = movies[i];
            Console.WriteLine($"{i + 1,-3} {m.Title,-30} {m.Year,4} {m.Genre,-10} {m.Rating,6:F1} {m.Duration,6} min");
        }

        // ── 3. Method returning a Tuple ───────────────────────
        Console.WriteLine("\n=== 3. Database Stats (Tuple return value) ===");

        var stats = GetStats(movies);
        Console.WriteLine($"Total movies    : {stats.Count}");
        Console.WriteLine($"Highest rating  : {stats.HighestRating} ({stats.BestTitle})");
        Console.WriteLine($"Lowest rating   : {stats.LowestRating}");
        Console.WriteLine($"Average rating  : {stats.AverageRating:F2}");
        Console.WriteLine($"Longest film    : {stats.LongestTitle} ({stats.LongestDuration} min)");

        // ── 4. Deconstruction ─────────────────────────────────
        Console.WriteLine("\n=== 4. Deconstruction ===");

        // Destructure the return value into separate variables
        var (count, highest, bestTitle, lowest, avg, longestTitle, longestDur) = GetStats(movies);
        Console.WriteLine($"Destructured → Best: '{bestTitle}' with {highest} ⭐");

        // Discard values we don't need with _
        var (_, _, topMovie, _, _, _, _) = GetStats(movies);
        Console.WriteLine($"Using discard  → Top movie: '{topMovie}'");

        // ── 5. Filter by genre (returns tuple array) ──────────
        Console.WriteLine("\n=== 5. Filter by Genre ===");

        string filterGenre = "Sci-Fi";
        Console.WriteLine($"Movies in genre '{filterGenre}':");

        foreach (var m in movies)
        {
            if (m.Genre == filterGenre)
                Console.WriteLine($"  • {m.Title} ({m.Year}) — {m.Rating}⭐");
        }

        // ── 6. Method that returns a search result ────────────
        Console.WriteLine("\n=== 6. Search by Title ===");

        var result = FindMovie(movies, "Inception");
        if (result.Found)
            Console.WriteLine($"Found: {result.Movie.Title} | {result.Movie.Year} | {result.Movie.Genre} | ⭐{result.Movie.Rating}");
        else
            Console.WriteLine("Movie not found.");

        var notFound = FindMovie(movies, "Avatar");
        Console.WriteLine($"Search 'Avatar': Found = {notFound.Found}");

        // ── 7. Swap two values using tuple ────────────────────
        Console.WriteLine("\n=== 7. Swap with Tuple (C# trick) ===");
        int x = 10, y = 20;
        Console.WriteLine($"Before swap: x={x}, y={y}");
        (x, y) = (y, x);    // elegant one-liner swap!
        Console.WriteLine($"After swap : x={x}, y={y}");

        // ── 8. Tuple comparison ───────────────────────────────
        Console.WriteLine("\n=== 8. Tuple Equality ===");
        var t1 = (Name: "Alice", Score: 95);
        var t2 = (Name: "Alice", Score: 95);
        var t3 = (Name: "Bob", Score: 80);

        Console.WriteLine($"t1 == t2 : {t1 == t2}");  // true
        Console.WriteLine($"t1 == t3 : {t1 == t3}");  // false

        Console.WriteLine("\n✅ Program complete!");
        Console.ReadLine();
    }

    // Returns multiple stats about the movie collection
    static (int Count,
            double HighestRating, string BestTitle,
            double LowestRating,
            double AverageRating,
            string LongestTitle, int LongestDuration)
        GetStats((string Title, int Year, string Genre, double Rating, int Duration)[] movies)
    {
        int count = movies.Length;
        double highest = movies[0].Rating;
        double lowest = movies[0].Rating;
        string bestTitle = movies[0].Title;
        int longestDur = movies[0].Duration;
        string longestTitle = movies[0].Title;
        double sum = 0;

        foreach (var m in movies)
        {
            sum += m.Rating;
            if (m.Rating > highest) { highest = m.Rating; bestTitle = m.Title; }
            if (m.Rating < lowest) lowest = m.Rating;
            if (m.Duration > longestDur) { longestDur = m.Duration; longestTitle = m.Title; }
        }

        return (count, highest, bestTitle, lowest, sum / count, longestTitle, longestDur);
    }

    // Search: returns a tuple with a Found flag AND the movie data
    static (bool Found, (string Title, int Year, string Genre, double Rating, int Duration) Movie)
        FindMovie(
            (string Title, int Year, string Genre, double Rating, int Duration)[] movies,
            string title)
    {
        foreach (var m in movies)
            if (m.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                return (true, m);

        return (false, default);
    }
}