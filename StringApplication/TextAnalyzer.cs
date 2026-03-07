// ============================================================
//  PROJECT 2 — String Operations
//  Text Analyzer & Word Counter
//  Topics: Substring, Replace, IndexOf, Contains,
//          Split, Join, ToUpper, Trim, StartsWith, EndsWith
// ============================================================

using System;

class TextAnalyzer
{
    static void Main()
    {
        Console.WriteLine("╔══════════════════════════════╗");
        Console.WriteLine("║   Text Analyzer & Counter 📝 ║");
        Console.WriteLine("╚══════════════════════════════╝\n");

        string text = "  The quick brown fox jumps over the lazy dog. " +
                      "The dog barked loudly. The fox ran away quickly.  ";

        // ── 1. Trim whitespace ────────────────────────────────
        string clean = text.Trim();
        Console.WriteLine("=== Original (trimmed) ===");
        Console.WriteLine(clean);

        // ── 2. Length ─────────────────────────────────────────
        Console.WriteLine($"\nCharacter count : {clean.Length}");

        // ── 3. ToUpper / ToLower ──────────────────────────────
        Console.WriteLine($"UPPERCASE       : {clean.ToUpper()}");
        Console.WriteLine($"lowercase       : {clean.ToLower()}");

        // ── 4. Contains, StartsWith, EndsWith ─────────────────
        Console.WriteLine("\n=== Search Operations ===");
        Console.WriteLine($"Contains 'fox'       : {clean.Contains("fox")}");
        Console.WriteLine($"Contains 'cat'       : {clean.Contains("cat")}");
        Console.WriteLine($"StartsWith 'The'     : {clean.StartsWith("The")}");
        Console.WriteLine($"EndsWith 'quickly.'  : {clean.EndsWith("quickly.")}");

        // ── 5. IndexOf / LastIndexOf ──────────────────────────
        int firstThe = clean.IndexOf("The");
        int lastThe = clean.LastIndexOf("The");
        int foxPos = clean.IndexOf("fox");

        Console.WriteLine("\n=== Position Finding ===");
        Console.WriteLine($"First 'The' at index : {firstThe}");
        Console.WriteLine($"Last  'The' at index : {lastThe}");
        Console.WriteLine($"'fox' starts at index: {foxPos}");

        // ── 6. Substring ──────────────────────────────────────
        Console.WriteLine("\n=== Substring Extraction ===");
        string firstSentence = clean.Substring(0, clean.IndexOf('.') + 1);
        Console.WriteLine($"First sentence : {firstSentence}");

        string fromFox = clean.Substring(foxPos, 9);  // 9 chars from 'fox'
        Console.WriteLine($"9 chars from fox: '{fromFox}'");

        // ── 7. Replace ────────────────────────────────────────
        Console.WriteLine("\n=== Replace ===");
        string replaced = clean.Replace("fox", "cat").Replace("dog", "mouse");
        Console.WriteLine(replaced);

        // ── 8. Split & Join ───────────────────────────────────
        Console.WriteLine("\n=== Word Analysis (Split) ===");
        // Split on spaces and punctuation, remove empty entries
        string[] words = clean.Split(new char[] { ' ', '.', ',', '!', '?' },
                                     StringSplitOptions.RemoveEmptyEntries);

        Console.WriteLine($"Total words   : {words.Length}");

        // Count unique words (case-insensitive)
        int uniqueCount = 0;
        string[] seen = new string[words.Length];

        for (int i = 0; i < words.Length; i++)
        {
            string lower = words[i].ToLower();
            bool alreadySeen = Array.Exists(seen, s => s == lower);
            if (!alreadySeen)
            {
                seen[uniqueCount] = lower;
                uniqueCount++;
            }
        }
        Console.WriteLine($"Unique words  : {uniqueCount}");

        // Find the longest word
        string longest = words[0];
        foreach (string w in words)
            if (w.Length > longest.Length) longest = w;

        Console.WriteLine($"Longest word  : '{longest}' ({longest.Length} chars)");

        // ── 9. Join ───────────────────────────────────────────
        Console.WriteLine("\n=== Rejoin with dash ===");
        string dashed = string.Join("-", words);
        Console.WriteLine(dashed);

        // ── 10. Palindrome checker ────────────────────────────
        Console.WriteLine("\n=== Palindrome Checker ===");
        string[] testWords = { "racecar", "hello", "madam", "world", "level" };
        foreach (string w in testWords)
        {
            bool isPalindrome = IsPalindrome(w);
            Console.WriteLine($"  '{w}' → {(isPalindrome ? "✅ palindrome" : "❌ not palindrome")}");
        }

        // ── 11. Simple Caesar cipher (using IndexOf + Substring) ──
        Console.WriteLine("\n=== Caesar Cipher (shift 3) ===");
        string secret = "Hello World";
        string encoded = CaesarCipher(secret, 3);
        string decoded = CaesarCipher(encoded, -3);
        Console.WriteLine($"Original : {secret}");
        Console.WriteLine($"Encoded  : {encoded}");
        Console.WriteLine($"Decoded  : {decoded}");

        Console.WriteLine("\n✅ Program complete!");
    }

    // Check if a word reads the same forwards and backwards
    static bool IsPalindrome(string word)
    {
        string lower = word.ToLower();
        int len = lower.Length;
        for (int i = 0; i < len / 2; i++)
            if (lower[i] != lower[len - 1 - i]) return false;
        return true;
    }

    // Simple Caesar cipher: shift each letter by 'shift' positions
    static string CaesarCipher(string text, int shift)
    {
        char[] result = new char[text.Length];
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            if (char.IsLetter(c))
            {
                char baseChar = char.IsUpper(c) ? 'A' : 'a';
                result[i] = (char)(((c - baseChar + shift + 26) % 26) + baseChar);
            }
            else result[i] = c;
        }
        return new string(result);
    }
}