// ============================================================
//  PROJECT 1 — Arrays
//  Student Grade Tracker
//  Topics: Single-Dimensional Arrays, Array Methods,
//          Array Class (Sort, Reverse, BinarySearch)
// ============================================================

using System;

class StudentGradeTracker
{
    static void Main()
    {
        Console.WriteLine("╔══════════════════════════════╗");
        Console.WriteLine("║   Student Grade Tracker 📊   ║");
        Console.WriteLine("╚══════════════════════════════╝\n");

        // ── 1. Single-Dimensional Array ──────────────────────
        string[] students = { "Alice", "Bob", "Charlie", "Diana", "Eve" };
        int[] grades = { 88, 72, 95, 60, 81 };

        Console.WriteLine("=== Original Grades ===");
        PrintGrades(students, grades);

        // ── 2. Array Properties ──────────────────────────────
        Console.WriteLine($"\nTotal students : {students.Length}");
        Console.WriteLine($"Array rank     : {grades.Rank}");   // 1 = one-dimensional

        // ── 3. Manual stats (no LINQ) ────────────────────────
        int sum = 0;
        int highest = grades[0];
        int lowest = grades[0];

        for (int i = 0; i < grades.Length; i++)
        {
            sum += grades[i];
            if (grades[i] > highest) highest = grades[i];
            if (grades[i] < lowest) lowest = grades[i];
        }

        double average = (double)sum / grades.Length;

        Console.WriteLine("\n=== Statistics ===");
        Console.WriteLine($"Highest grade : {highest}");
        Console.WriteLine($"Lowest grade  : {lowest}");
        Console.WriteLine($"Average grade : {average:F1}");

        // ── 4. Array.Sort + Array.Reverse ────────────────────
        // Clone grades first so we don't destroy the original pairing
        int[] sortedGrades = (int[])grades.Clone();
        Array.Sort(sortedGrades);

        Console.WriteLine("\n=== Grades Sorted Ascending ===");
        Console.WriteLine(string.Join(", ", sortedGrades));

        Array.Reverse(sortedGrades);
        Console.WriteLine("=== Grades Sorted Descending ===");
        Console.WriteLine(string.Join(", ", sortedGrades));

        // ── 5. Array.IndexOf ─────────────────────────────────
        int targetGrade = 95;
        int foundAt = Array.IndexOf(grades, targetGrade);
        if (foundAt >= 0)
            Console.WriteLine($"\nGrade {targetGrade} belongs to: {students[foundAt]}");

        // ── 6. Array.Exists + Array.Find ─────────────────────
        bool hasFailure = Array.Exists(grades, g => g < 65);
        Console.WriteLine($"\nAny student failed (< 65)? {hasFailure}");

        // ── 7. Array.BinarySearch (must sort first!) ─────────
        int[] sortedCopy = (int[])grades.Clone();
        Array.Sort(sortedCopy);
        int searchFor = 81;
        int bsIndex = Array.BinarySearch(sortedCopy, searchFor);
        Console.WriteLine($"BinarySearch for {searchFor} in sorted array → index {bsIndex}");

        // ── 8. Multi-Dimensional: Semester grades ─────────────
        Console.WriteLine("\n=== 2D Array: Semester Grades ===");
        //            Sem1  Sem2  Sem3
        int[,] semGrades = {
            { 80, 85, 88 },   // Alice
            { 70, 74, 72 },   // Bob
            { 90, 93, 95 },   // Charlie
        };

        string[] threeStudents = { "Alice", "Bob", "Charlie" };

        for (int row = 0; row < semGrades.GetLength(0); row++)
        {
            Console.Write($"{threeStudents[row],-10}");
            int rowSum = 0;
            for (int col = 0; col < semGrades.GetLength(1); col++)
            {
                Console.Write($"Sem{col + 1}:{semGrades[row, col]}  ");
                rowSum += semGrades[row, col];
            }
            Console.WriteLine($"→ Avg: {rowSum / semGrades.GetLength(1):F0}");
        }

        // ── 9. Jagged Array: variable quiz scores ─────────────
        Console.WriteLine("\n=== Jagged Array: Quiz Scores ===");
        int[][] quizScores = new int[3][];
        quizScores[0] = new int[] { 9, 8 };          // Alice took 2 quizzes
        quizScores[1] = new int[] { 7, 6, 8, 9 };    // Bob took 4 quizzes
        quizScores[2] = new int[] { 10, 9, 10 };      // Charlie took 3 quizzes

        for (int i = 0; i < quizScores.Length; i++)
        {
            Console.Write($"{threeStudents[i],-10}scores: ");
            Console.WriteLine(string.Join(", ", quizScores[i]));
        }

        Console.WriteLine("\n✅ Program complete!");
    }

    // Helper: print student-grade pairs with a letter grade
    static void PrintGrades(string[] names, int[] grades)
    {
        for (int i = 0; i < names.Length; i++)
        {
            string letter = grades[i] >= 90 ? "A" :
                            grades[i] >= 80 ? "B" :
                            grades[i] >= 70 ? "C" :
                            grades[i] >= 60 ? "D" : "F";

            Console.WriteLine($"  {names[i],-10} {grades[i],3}  [{letter}]");
        }
    }
}