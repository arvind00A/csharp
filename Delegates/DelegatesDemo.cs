// ============================================================
//  PROJECT 2 — Delegates
//  Middleware Processing Pipeline
//  Topics: Action, Func, Predicate, Anonymous Methods,
//          Multicast delegates, Custom delegates
// ============================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Delegates
{
    // ── Models ────────────────────────────────────────────────────
    public record HttpRequest(string Method, string Path, Dictionary<string, string> Headers,
        string Body = "", string? UserId = null);

    public record HttpResponse(int StatusCode, string Body, bool IsSuccess = true);

    // ── Custom delegate type ───────────────────────────────────────
    public delegate HttpResponse MiddlewareFunc(HttpRequest request);
    public delegate bool RequestValidator(HttpRequest request);


    // ── Main Demo ─────────────────────────────────────────────────
    internal class DelegatesDemo
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║  Delegates — Middleware Pipeline 🔧      ║");
            Console.WriteLine("╚══════════════════════════════════════════╝\n");

            Demo_ActionFuncPredicate();
            Demo_MulticastDelegates();
            Demo_AnonymousMethods();
            Demo_CustomDelegates();
            Demo_MiddlewarePipeline();

        }

        // ── 1. Action, Func, Predicate ────────────────────────────
        static void Demo_ActionFuncPredicate()
        {
            Console.WriteLine("=== 1. Action, Func, Predicate ===\n");

            // Action — void return
            Action<string> log = msg => Console.WriteLine($"  [LOG] {msg}");
            Action<string, int> logCode = (msg, code) => Console.WriteLine($"  [{code}] {msg}");
            Action<HttpRequest> printReq = req => Console.WriteLine($"  → {req.Method} {req.Path}");

            log("App started");
            logCode("Unauthorized", 401);
            printReq(new HttpRequest("GET", "/api/users", new()));

            // Action multiline — anonymous method style
            Action<string> banner = delegate (string title)
            {
                Console.WriteLine($"\n  ┌─ {title} ─┐");
            };
            banner("Request Log");

            // Func — returns a value
            Func<string, bool> isAuth = path => path.StartsWith("/api/");
            Func<int, string> statusText = code => code switch {
                200 => "OK",
                201 => "Created",
                400 => "Bad Request",
                401 => "Unauthorized",
                404 => "Not Found",
                500 => "Server Error",
                _ => "Unknown"
            };
            Func<HttpRequest, string> getUser = req =>
                req.Headers.TryGetValue("Authorization", out var v) ? v : "anonymous";

            Console.WriteLine($"\n  /api/users requires auth: {isAuth("/api/users")}");
            Console.WriteLine($"  /public/home requires auth: {isAuth("/public/home")}");
            Console.WriteLine($"  Status 404: {statusText(404)}");
            Console.WriteLine($"  Status 201: {statusText(201)}");

            var req2 = new HttpRequest("POST", "/api/login",
                new Dictionary<string, string> { ["Authorization"] = "user123" });
            Console.WriteLine($"  User from header: {getUser(req2)}");

            // Predicate — returns bool
            Predicate<HttpRequest> hasBody = r => !string.IsNullOrEmpty(r.Body);
            Predicate<HttpRequest> isGet = r => r.Method == "GET";
            Predicate<HttpRequest> isPost = r => r.Method == "POST";
            Predicate<string> isEmail = s => s.Contains("@") && s.Contains(".");

            var postReq = new HttpRequest("POST", "/api/data", new(), "{ \"name\": \"test\" }");
            Console.WriteLine($"\n  POST has body: {hasBody(postReq)}");
            Console.WriteLine($"  POST isGet: {isGet(postReq)}");
            Console.WriteLine($"  'test@example.com' isEmail: {isEmail("test@example.com")}");
            Console.WriteLine($"  'not-an-email' isEmail: {isEmail("not-an-email")}");

            // Passing delegates to a method
            var requests = new List<HttpRequest>
        {
            new("GET",    "/api/users",  new(), ""),
            new("POST",   "/api/login",  new(), "{\"user\":\"Alice\"}"),
            new("DELETE", "/api/users/1",new(), ""),
            new("GET",    "/public/home",new(), ""),
        };

            Console.WriteLine("\n  POST requests with body:");
            requests.FindAll(r => isPost(r) && hasBody(r))
                    .ForEach(r => Console.WriteLine($"    {r.Method} {r.Path}"));
            Console.WriteLine();
        }

        // ── 2. Multicast Delegates ────────────────────────────────
        static void Demo_MulticastDelegates()
        {
            Console.WriteLine("=== 2. Multicast Delegates ===\n");

            // Build a logging pipeline with multicast Action
            Action<HttpRequest> requestPipeline = null!;

            // Add middleware functions one by one
            requestPipeline += req => Console.WriteLine($"  [TIMESTAMP] {DateTime.Now:HH:mm:ss.fff}");
            requestPipeline += req => Console.WriteLine($"  [REQUEST]   {req.Method} {req.Path}");
            requestPipeline += req => Console.WriteLine($"  [HEADERS]   {req.Headers.Count} headers");
            requestPipeline += req =>
            {
                if (!string.IsNullOrEmpty(req.Body))
                    Console.WriteLine($"  [BODY]      {req.Body[..Math.Min(40, req.Body.Length)]}...");
            };

            Console.WriteLine("  Processing request through multicast pipeline:");
            var request = new HttpRequest("POST", "/api/orders",
                new() { ["Content-Type"] = "application/json", ["Accept"] = "application/json" },
                "{ \"productId\": 42, \"quantity\": 3 }");

            requestPipeline(request);

            // Remove one step
            Action<HttpRequest> timestampStep = req => Console.WriteLine($"  [TIMESTAMP] {DateTime.Now}");
            requestPipeline -= timestampStep;  // won't affect the lambda above (different instance)

            // Inspect invocation list
            Console.WriteLine($"\n  Pipeline steps count: {requestPipeline.GetInvocationList().Length}");

            // Func multicast — only last return value kept
            Func<int, int> transform = x => x + 1;
            transform += x => x * 2;
            transform += x => x - 3;
            int result = transform(5);
            Console.WriteLine($"\n  Func multicast (5): result = {result} (only LAST delegate's value: 5-3=2)");

            // Iterate to get all return values
            Console.WriteLine("  All transform results for input 5:");
            foreach (Func<int, int> fn in transform.GetInvocationList().Cast<Func<int, int>>())
                Console.WriteLine($"    → {fn(5)}");
            Console.WriteLine();
        }

        // ── 3. Anonymous Methods ──────────────────────────────────
        static void Demo_AnonymousMethods()
        {
            Console.WriteLine("=== 3. Anonymous Methods ===\n");

            // Old syntax: delegate keyword
            Func<string, string> sanitize = delegate (string input)
            {
                string result = input.Trim().ToLower();
                result = result.Replace("<", "&lt;").Replace(">", "&gt;");
                return result;
            };

            Predicate<string> isValidPath = delegate (string path)
            {
                return path.StartsWith("/") && !path.Contains("..");
            };

            Console.WriteLine("  Anonymous method - sanitize:");
            Console.WriteLine($"    Input:  '  <script>alert()</script>  '");
            Console.WriteLine($"    Output: '{sanitize("  <script>alert()</script>  ")}'");

            Console.WriteLine("\n  Anonymous method - path validation:");
            string[] paths = { "/api/users", "../etc/passwd", "/api/../config", "/valid/path" };
            foreach (string p in paths)
                Console.WriteLine($"    {p,-25} valid: {isValidPath(p)}");

            // Anonymous method can omit parameter list entirely (unique feature)
            Action clickHandler = delegate { Console.WriteLine("  Clicked! (no params needed)"); };
            clickHandler();

            // Side by side comparison
            Console.WriteLine("\n  Comparison — anonymous vs lambda:");

            // Anonymous
            Func<int, int, int> powOld = delegate (int b, int e)
            {
                int result2 = 1;
                for (int i = 0; i < e; i++) result2 *= b;
                return result2;
            };

            // Lambda
            Func<int, int, int> powNew = (b, e) =>
            {
                int result2 = 1;
                for (int i = 0; i < e; i++) result2 *= b;
                return result2;
            };

            Console.WriteLine($"    2^8 (anonymous): {powOld(2, 8)}");
            Console.WriteLine($"    2^8 (lambda):    {powNew(2, 8)}");
            Console.WriteLine();
        }

        // ── 4. Custom Delegate Types ──────────────────────────────
        static void Demo_CustomDelegates()
        {
            Console.WriteLine("=== 4. Custom Delegate Types ===\n");

            // Use our custom delegates
            RequestValidator authValidator = req =>
                req.Headers.ContainsKey("Authorization");

            RequestValidator sizeValidator = req =>
                req.Body.Length < 10_000;

            RequestValidator methodValidator = req =>
                new[] { "GET", "POST", "PUT", "DELETE", "PATCH" }.Contains(req.Method);

            // Combine validators
            RequestValidator[] validators = { authValidator, sizeValidator, methodValidator };

            MiddlewareFunc routeHandler = req => req.Path switch
            {
                "/api/users" => new HttpResponse(200, "[{\"id\":1,\"name\":\"Alice\"}]"),
                "/api/login" => new HttpResponse(200, "{\"token\":\"abc123\"}"),
                "/health" => new HttpResponse(200, "OK"),
                _ => new HttpResponse(404, "Not Found", false)
            };

            var testRequests = new[]
            {
                new HttpRequest("GET",  "/api/users", new() {["Authorization"]="Bearer token"}, ""),
                new HttpRequest("POST", "/api/login",  new() {["Authorization"]="Bearer token"}, "{\"user\":\"Bob\"}"),
                new HttpRequest("GET",  "/api/missing",new() {["Authorization"]="Bearer token"}, ""),
                new HttpRequest("GET",  "/api/users",  new(),  ""),  // missing auth
            };

            Console.WriteLine("  Custom delegate validation pipeline:");
            foreach (var req in testRequests)
            {
                bool valid = validators.All(v => v(req));
                if (!valid)
                {
                    Console.WriteLine($"  ❌ {req.Method,-7} {req.Path,-20} REJECTED (validation failed)");
                    continue;
                }
                var response = routeHandler(req);
                string icon = response.IsSuccess ? "✅" : "⚠️";
                Console.WriteLine($"  {icon} {req.Method,-7} {req.Path,-20} {response.StatusCode} {response.Body[..Math.Min(30, response.Body.Length)]}");
            }
            Console.WriteLine();
        }

        // ── 5. Full Middleware Pipeline ───────────────────────────
        static void Demo_MiddlewarePipeline()
        {
            Console.WriteLine("=== 5. Full Middleware Pipeline ===\n");

            // Build pipeline using Func<HttpRequest, HttpResponse>
            // Each middleware wraps the next one

            Func<HttpRequest, HttpResponse> finalHandler = req =>
                new HttpResponse(200, $"{{\"path\":\"{req.Path}\",\"user\":\"{req.UserId ?? "anon"}\"}}");

            // Wrap with auth middleware
            Func<HttpRequest, HttpResponse> withAuth = req =>
            {
                if (!req.Headers.ContainsKey("Authorization"))
                    return new HttpResponse(401, "Unauthorized", false);
                return finalHandler(req);
            };

            // Wrap with logging middleware
            var sw = new Stopwatch();
            Func<HttpRequest, HttpResponse> withLogging = req =>
            {
                sw.Restart();
                Console.WriteLine($"  → Incoming: {req.Method} {req.Path}");
                var response = withAuth(req);
                sw.Stop();
                Console.WriteLine($"  ← Response: {response.StatusCode} ({sw.ElapsedMilliseconds}ms)");
                return response;
            };

            // Wrap with error handling middleware
            Func<HttpRequest, HttpResponse> withErrorHandling = req =>
            {
                try
                {
                    return withLogging(req);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  ❌ Error: {ex.Message}");
                    return new HttpResponse(500, "Internal Server Error", false);
                }
            };

            var pipeline = withErrorHandling;

            Console.WriteLine("  Processing requests through full pipeline:\n");

            var requests = new[]
            {
                new HttpRequest("GET", "/api/products", new() {["Authorization"]="Bearer abc"}, "", "user1"),
                new HttpRequest("POST","/api/orders",   new() {["Authorization"]="Bearer xyz"}, "{}", "user2"),
                new HttpRequest("GET", "/api/admin",    new(), "", null),   // no auth
            };

            foreach (var req in requests)
            {
                var resp = pipeline(req);
                Console.WriteLine($"  Result: {(resp.IsSuccess ? "✅" : "❌")} {resp.StatusCode}\n");
            }

            Console.WriteLine("✅ Delegates demo complete!");
        }
    }
}
