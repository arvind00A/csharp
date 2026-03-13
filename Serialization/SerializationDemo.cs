// ============================================================
//  PROJECT 3 — Serialization
//  Game Save System (JSON + XML + Binary)
//  Topics: System.Text.Json, XmlSerializer,
//          BinaryWriter/BinaryReader, attributes
// ============================================================


using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Serialization
{
    // ══════════════════════════════════════════════════════════════
    //  MODELS
    // ══════════════════════════════════════════════════════════════

    // ── JSON model — with attributes ─────────────────────────────
    public class PlayerProfile
    {
        [JsonPropertyName("player_id")]
        public int PlayerId { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; } = "";

        [JsonPropertyName("email")]
        public string Email { get; set; } = "";

        [JsonIgnore]                   // never save password to disk!
        public string PasswordHash { get; set; } = "";

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [JsonPropertyName("total_playtime_hours")]
        public double TotalPlaytimeHours { get; set; }

        [JsonPropertyName("achievements")]
        public List<string> Achievements { get; set; } = new();

        [JsonPropertyName("settings")]
        public GameSettings Settings { get; set; } = new();
    }

    public class GameSettings
    {
        [JsonPropertyName("volume")]
        public int Volume { get; set; } = 80;

        [JsonPropertyName("fullscreen")]
        public bool Fullscreen { get; set; } = true;

        [JsonPropertyName("difficulty")]
        public string Difficulty { get; set; } = "Normal";

        [JsonPropertyName("language")]
        public string Language { get; set; } = "en";
    }

    // ── XML model — with XmlSerializer attributes ─────────────────
    [XmlRoot("game_save")]
    public class GameSave
    {
        [XmlAttribute("version")]
        public string Version { get; set; } = "1.0";

        [XmlAttribute("save_slot")]
        public int SaveSlot { get; set; }

        [XmlElement("player_name")]
        public string PlayerName { get; set; } = "";

        [XmlElement("level")]
        public int Level { get; set; }

        [XmlElement("experience")]
        public int Experience { get; set; }

        [XmlElement("gold")]
        public int Gold { get; set; }

        [XmlElement("current_map")]
        public string CurrentMap { get; set; } = "";

        [XmlArray("inventory")]
        [XmlArrayItem("item")]
        public List<InventoryItem> Inventory { get; set; } = new();

        [XmlArray("quests")]
        [XmlArrayItem("quest")]
        public List<QuestStatus> Quests { get; set; } = new();

        [XmlElement("saved_at")]
        public string SavedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public class InventoryItem
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlElement("name")]
        public string Name { get; set; } = "";

        [XmlElement("quantity")]
        public int Quantity { get; set; }

        [XmlElement("rarity")]
        public string Rarity { get; set; } = "Common";
    }

    public class QuestStatus
    {
        [XmlAttribute("id")]
        public string QuestId { get; set; } = "";

        [XmlElement("title")]
        public string Title { get; set; } = "";

        [XmlElement("completed")]
        public bool Completed { get; set; }

        [XmlElement("progress")]
        public int Progress { get; set; }
    }

    // ── Binary model — high-score leaderboard ────────────────────
    // (BinaryWriter handles manually — no attributes needed)
    public record HighScoreEntry(string PlayerName, int Score,
        int Level, DateTime AchievedAt);

    // ══════════════════════════════════════════════════════════════
    //  SERIALIZATION SERVICES
    // ══════════════════════════════════════════════════════════════

    // ── JSON Service (System.Text.Json) ──────────────────────────
    public static class JsonSaveService
    {
        private static readonly JsonSerializerOptions _opts = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = null,     // use our [JsonPropertyName] attrs
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        public static void Save(PlayerProfile profile, string path)
        {
            string json = JsonSerializer.Serialize(profile, _opts);
            File.WriteAllText(path, json);
            Console.WriteLine($"  💾 JSON saved: {path} ({new FileInfo(path).Length} bytes)");
        }

        public static PlayerProfile? Load(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"  ❌ JSON file not found: {path}");
                return null;
            }
            string json = File.ReadAllText(path);
            var profile = JsonSerializer.Deserialize<PlayerProfile>(json, _opts);
            Console.WriteLine($"  📂 JSON loaded: {path}");
            return profile;
        }

        public static string ToJson(PlayerProfile profile) =>
            JsonSerializer.Serialize(profile, _opts);

        public static async Task SaveAsync(PlayerProfile profile, string path)
        {
            await using var stream = File.Create(path);
            await JsonSerializer.SerializeAsync(stream, profile, _opts);
            Console.WriteLine($"  💾 JSON saved async: {path}");
        }
    }

    // ── XML Service (XmlSerializer) ───────────────────────────────
    public static class XmlSaveService
    {
        private static readonly XmlSerializer _xs = new(typeof(GameSave));

        public static void Save(GameSave save, string path)
        {
            using var sw = new StreamWriter(path);
            _xs.Serialize(sw, save);
            Console.WriteLine($"  💾 XML saved: {path} ({new FileInfo(path).Length} bytes)");
        }

        public static GameSave? Load(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"  ❌ XML file not found: {path}");
                return null;
            }
            using var sr = new StreamReader(path);
            var save = (GameSave?)_xs.Deserialize(sr);
            Console.WriteLine($"  📂 XML loaded: {path}");
            return save;
        }

        public static string ToXmlString(GameSave save)
        {
            using var ms = new MemoryStream();
            _xs.Serialize(ms, save);
            return System.Text.Encoding.UTF8.GetString(ms.ToArray());
        }
    }

    // ── Binary Service (BinaryWriter/Reader) ─────────────────────
    public static class BinarySaveService
    {
        private const int FORMAT_VERSION = 2;  // bump when format changes

        public static void SaveLeaderboard(
            List<HighScoreEntry> entries, string path)
        {
            using var fs = File.Create(path);
            using var bw = new BinaryWriter(fs);

            bw.Write(FORMAT_VERSION);           // version header
            bw.Write(entries.Count);            // count

            foreach (var e in entries)
            {
                bw.Write(e.PlayerName);          // string
                bw.Write(e.Score);               // int (4 bytes)
                bw.Write(e.Level);               // int (4 bytes)
                bw.Write(e.AchievedAt.Ticks);    // long (8 bytes)
            }
            Console.WriteLine($"  💾 Binary saved: {path} ({new FileInfo(path).Length} bytes)");
        }

        public static List<HighScoreEntry> LoadLeaderboard(string path)
        {
            var entries = new List<HighScoreEntry>();
            if (!File.Exists(path))
            {
                Console.WriteLine($"  ❌ Binary file not found: {path}");
                return entries;
            }

            using var fs = File.OpenRead(path);
            using var br = new BinaryReader(fs);

            int version = br.ReadInt32();        // must read in same order!
            if (version != FORMAT_VERSION)
                throw new InvalidDataException($"Unsupported save version: {version}");

            int count = br.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                string name = br.ReadString();
                int score = br.ReadInt32();
                int level = br.ReadInt32();
                long ticks = br.ReadInt64();
                entries.Add(new HighScoreEntry(name, score, level, new DateTime(ticks)));
            }

            Console.WriteLine($"  📂 Binary loaded: {path} ({entries.Count} entries)");
            return entries;
        }
    }

    // ══════════════════════════════════════════════════════════════
    //  MAIN DEMO
    // ══════════════════════════════════════════════════════════════
    internal class SerializationDemo
    {
        static async Task Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║  Serialization — Game Save System 🎮     ║");
            Console.WriteLine("╚══════════════════════════════════════════╝\n");

            await Demo1_JsonSerialization();
            Demo2_XmlSerialization();
            Demo3_BinarySerialization();
            Demo4_FormatComparison();
        }

        // ── 1. JSON Serialization ─────────────────────────────────
        static async Task Demo1_JsonSerialization()
        {
            Console.WriteLine("=== 1. JSON Serialization (System.Text.Json) ===\n");

            var profile = new PlayerProfile
            {
                PlayerId = 1042,
                Username = "DragonSlayer99",
                Email = "player@game.com",
                PasswordHash = "secret123",   // [JsonIgnore] — won't be saved
                TotalPlaytimeHours = 247.5,
                CreatedAt = DateTime.Now.AddDays(-90),
                Achievements = new() { "First Blood", "Dragon Slayer", "Speed Runner", "No Damage" },
                Settings = new GameSettings
                {
                    Volume = 65,
                    Fullscreen = true,
                    Difficulty = "Hard",
                    Language = "en"
                }
            };

            string path = Path.Combine(Path.GetTempPath(), "player_profile.json");

            // Serialize to file
            JsonSaveService.Save(profile, path);

            // Serialize to string (for display)
            string json = JsonSaveService.ToJson(profile);
            Console.WriteLine("  JSON content (first 300 chars):");
            Console.WriteLine(json[..Math.Min(300, json.Length)]);
            Console.WriteLine("  ...\n");

            // Verify [JsonIgnore] works
            bool passwordInJson = json.Contains("PasswordHash") || json.Contains("secret123");
            Console.WriteLine($"  [JsonIgnore] working: password in JSON = {passwordInJson}");

            // Deserialize
            var loaded = JsonSaveService.Load(path);
            Console.WriteLine($"  Loaded: {loaded?.Username} | Achievements: {loaded?.Achievements.Count}");
            Console.WriteLine($"  Playtime: {loaded?.TotalPlaytimeHours}h | Difficulty: {loaded?.Settings.Difficulty}");

            // Async serialize
            await JsonSaveService.SaveAsync(profile, Path.Combine(Path.GetTempPath(), "player_async.json"));

            // Serialize list
            var profiles = new List<PlayerProfile> { profile,
            new() { PlayerId = 1043, Username = "NightOwl", TotalPlaytimeHours = 89 }
        };
            string listJson = JsonSerializer.Serialize(profiles, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine($"\n  Serialized {profiles.Count} profiles ({listJson.Length} chars)\n");
        }

        // ── 2. XML Serialization ──────────────────────────────────
        static void Demo2_XmlSerialization()
        {
            Console.WriteLine("=== 2. XML Serialization (XmlSerializer) ===\n");

            var save = new GameSave
            {
                SaveSlot = 1,
                Version = "2.0",
                PlayerName = "DragonSlayer99",
                Level = 42,
                Experience = 125_000,
                Gold = 8_750,
                CurrentMap = "Dragon's Lair — Level 5",
                Inventory = new()
                {
                    new() { Id = 101, Name = "Excalibur",      Quantity = 1, Rarity = "Legendary" },
                    new() { Id = 205, Name = "Health Potion",  Quantity = 15, Rarity = "Common" },
                    new() { Id = 312, Name = "Dragon Scale",   Quantity = 3, Rarity = "Rare" },
                    new() { Id = 407, Name = "Magic Scroll",   Quantity = 7, Rarity = "Uncommon" },
                },

                Quests = new()
                {
                    new() { QuestId = "Q001", Title = "Slay the Dragon",  Completed = true,  Progress = 100 },
                    new() { QuestId = "Q002", Title = "Find the Artifact", Completed = false, Progress = 60 },
                    new() { QuestId = "Q003", Title = "Save the Village",  Completed = false, Progress = 25 },
                }
            };

            string path = Path.Combine(Path.GetTempPath(), "game_save_slot1.xml");

            // Serialize to file
            XmlSaveService.Save(save, path);

            // Show XML output
            string xml = XmlSaveService.ToXmlString(save);
            Console.WriteLine("  XML content (first 500 chars):");
            Console.WriteLine(xml[..Math.Min(500, xml.Length)]);
            Console.WriteLine("  ...\n");

            // Deserialize
            var loaded = XmlSaveService.Load(path);
            Console.WriteLine($"  Loaded: {loaded?.PlayerName} | Level {loaded?.Level}");
            Console.WriteLine($"  Inventory: {loaded?.Inventory.Count} items");
            Console.WriteLine($"  Quests: {loaded?.Quests.Count(q => q.Completed)} completed, " +
                              $"{loaded?.Quests.Count(q => !q.Completed)} in progress");

            // Show all inventory items
            Console.WriteLine("\n  Inventory:");
            foreach (var item in loaded?.Inventory ?? new())
                Console.WriteLine($"    [{item.Rarity,-10}] {item.Name} x{item.Quantity}");
            Console.WriteLine();
        }

        // ── 3. Binary Serialization ───────────────────────────────
        static void Demo3_BinarySerialization()
        {
            Console.WriteLine("=== 3. Binary Serialization (BinaryWriter/Reader) ===\n");

            var leaderboard = new List<HighScoreEntry>
            {
                new("DragonSlayer99", 1_250_000, 42, DateTime.Now.AddDays(-2)),
                new("NightOwl",         980_500, 38, DateTime.Now.AddDays(-5)),
                new("SpeedRunner_X",    875_200, 35, DateTime.Now.AddDays(-1)),
                new("ZeroDeaths",       760_100, 40, DateTime.Now.AddDays(-3)),
                new("GoldFarmer",       650_800, 33, DateTime.Now.AddDays(-7)),
                new("CasualGamer",      520_300, 28, DateTime.Now.AddDays(-10)),
                new("HardcoreHero",     490_000, 45, DateTime.Now.AddDays(-4)),
                new("PvPMaster",        445_750, 36, DateTime.Now.AddDays(-6)),
            };

            string path = Path.Combine(Path.GetTempPath(), "leaderboard.bin");

            // Save
            BinarySaveService.SaveLeaderboard(leaderboard, path);

            // Load and display
            var loaded = BinarySaveService.LoadLeaderboard(path);
            Console.WriteLine("\n  🏆 Leaderboard:");
            Console.WriteLine($"  {"Rank",-5} {"Player",-20} {"Score",12} {"Level",6} {"Date",-12}");
            Console.WriteLine($"  {new string('─', 60)}");

            int rank = 1;
            foreach (var e in loaded.OrderByDescending(e => e.Score))
            {
                string medal = rank switch { 1 => "🥇", 2 => "🥈", 3 => "🥉", _ => $"  {rank}." };
                Console.WriteLine($"  {medal,-5} {e.PlayerName,-20} {e.Score,12:N0} {e.Level,6} " +
                                  $"{e.AchievedAt:MMM dd}");
                rank++;
            }
            Console.WriteLine();
        }

        // ── 4. Format comparison ──────────────────────────────────
        static void Demo4_FormatComparison()
        {
            Console.WriteLine("=== 4. Format Size Comparison ===\n");

            // Save same data in all 3 formats and compare sizes
            var profile = new PlayerProfile
            {
                PlayerId = 1,
                Username = "TestPlayer",
                Email = "test@game.com",
                TotalPlaytimeHours = 100,
                Achievements = new() { "Achievement1", "Achievement2", "Achievement3" }
            };

            // JSON
            string jsonStr = JsonSerializer.Serialize(profile,
                new JsonSerializerOptions { WriteIndented = false });
            int jsonSize = System.Text.Encoding.UTF8.GetByteCount(jsonStr);

            // XML
            var save = new GameSave { PlayerName = "TestPlayer", Level = 10, Gold = 500 };
            string xmlStr = XmlSaveService.ToXmlString(save);
            int xmlSize = System.Text.Encoding.UTF8.GetByteCount(xmlStr);

            // Binary
            var entries = new List<HighScoreEntry>
            { new("TestPlayer", 1000, 10, DateTime.Now) };
            string binPath = Path.Combine(Path.GetTempPath(), "test_compare.bin");
            BinarySaveService.SaveLeaderboard(entries, binPath);
            int binSize = (int)new FileInfo(binPath).Length;

            Console.WriteLine($"  {"Format",-20} {"Size (bytes)",12} {"Relative",-10}");
            Console.WriteLine($"  {new string('─', 46)}");
            Console.WriteLine($"  {"JSON (compact)",-20} {jsonSize,12} {"1.0x",-10}");
            Console.WriteLine($"  {"XML",-20} {xmlSize,12} {$"{(double)xmlSize / jsonSize:F1}x",-10}");
            Console.WriteLine($"  {"Binary",-20} {binSize,12} {$"{(double)binSize / jsonSize:F1}x",-10}");

            Console.WriteLine($"\n  Format selection guide:");
            Console.WriteLine($"    📡 API / Web:        → JSON (System.Text.Json)");
            Console.WriteLine($"    🗂️  Config / Legacy:  → XML");
            Console.WriteLine($"    ⚡ Performance:      → Binary (BinaryWriter) or MessagePack");
            Console.WriteLine($"    🔒 Sensitive data:   → Binary + encryption");

            Console.WriteLine("\n✅ Serialization demo complete!");
        }
    }
}
