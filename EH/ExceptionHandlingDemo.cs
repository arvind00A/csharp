// ============================================================
//  PROJECT 1 — Exception Handling
//  Bank Account System with Full Exception Coverage
//  Topics: try-catch-finally, throw vs throw ex,
//          custom exceptions, built-in exceptions,
//          global handler
// ============================================================


using System;
using System.Collections.Generic;
using System.Text;

namespace EH
{
    // ── Custom Exceptions ─────────────────────────────────────────
    public class InsufficientFundsException : Exception
    {
        public decimal Amount { get; }
        public decimal Balance { get; }

        public InsufficientFundsException() : base() { }

        public InsufficientFundsException(decimal amount, decimal balance)
            : base($"Cannot withdraw {amount:C2}. Current balance: {balance:C2}")
        {
            Amount = amount;
            Balance = balance;
        }

        public InsufficientFundsException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class AccountFrozenException : Exception
    {
        public string AccountId { get; }

        public AccountFrozenException(string accountId)
            : base($"Account {accountId} is frozen. Contact support.")
        {
            AccountId = accountId;
        }
    }

    public class DailyLimitExceededException : Exception
    {
        public decimal DailyLimit { get; }
        public decimal AlreadySpent { get; }

        public DailyLimitExceededException(decimal limit, decimal spent)
            : base($"Daily limit of {limit:C2} exceeded. Already spent: {spent:C2}")
        {
            DailyLimit = limit;
            AlreadySpent = spent;
        }
    }

    // ── Bank Account ──────────────────────────────────────────────
    public class BankAccount
    {
        public string Id { get; }
        public string Owner { get; }
        private decimal _balance;
        private decimal _dailyWithdrawn;
        private bool _isFrozen;

        private const decimal DailyLimit = 1000m;
        private List<string> _transactionLog = new();

        public BankAccount(string id, string owner, decimal initialBalance)
        {
            // Validate constructor arguments — ArgumentException family
            ArgumentNullException.ThrowIfNull(id, nameof(id));
            ArgumentNullException.ThrowIfNull(owner, nameof(owner));

            if (initialBalance < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(initialBalance), initialBalance,
                    "Initial balance cannot be negative");

            Id = id;
            Owner = owner;
            _balance = initialBalance;
        }

        public decimal Balance => _balance;

        public void Deposit(decimal amount)
        {
            // Validate — ArgumentException
            if (amount <= 0)
                throw new ArgumentException(
                    $"Deposit amount must be positive. Got: {amount:C2}", nameof(amount));

            if (_isFrozen)
                throw new AccountFrozenException(Id);

            _balance += amount;
            _transactionLog.Add($"[+] Deposit  {amount,10:C2}  Balance: {_balance:C2}");
            Console.WriteLine($"  ✅ Deposited {amount:C2} → Balance: {_balance:C2}");
        }

        public void Withdraw(decimal amount)
        {
            // Chain of validations — each throws a specific exception
            if (amount <= 0)
                throw new ArgumentException($"Amount must be positive. Got: {amount}", nameof(amount));

            if (_isFrozen)
                throw new AccountFrozenException(Id);

            if (amount > _balance)
                throw new InsufficientFundsException(amount, _balance);

            if (_dailyWithdrawn + amount > DailyLimit)
                throw new DailyLimitExceededException(DailyLimit, _dailyWithdrawn);

            _balance -= amount;
            _dailyWithdrawn += amount;
            _transactionLog.Add($"[-] Withdraw {amount,10:C2}  Balance: {_balance:C2}");
            Console.WriteLine($"  ✅ Withdrew  {amount:C2} → Balance: {_balance:C2}");
        }

        public void Transfer(BankAccount target, decimal amount)
        {
            Console.WriteLine($"\n  Transferring {amount:C2} from {Id} to {target.Id}...");
            try
            {
                Withdraw(amount);
                target.Deposit(amount);
                Console.WriteLine($"  ✅ Transfer complete");
            }
            catch (InsufficientFundsException)
            {
                // ✅ bare throw — preserves original stack trace
                Console.WriteLine($"  ❌ Transfer failed: insufficient funds");
                throw;
            }
            catch (Exception ex)
            {
                // Wrap lower-level exception in a domain exception
                throw new InvalidOperationException(
                    $"Transfer from {Id} to {target.Id} failed", ex);
            }
        }

        public void Freeze()
        {
            _isFrozen = true;
            Console.WriteLine($"  🔒 Account {Id} frozen");
        }

        public void PrintStatement()
        {
            Console.WriteLine($"\n  📋 Statement for {Owner} ({Id}):");
            Console.WriteLine($"  {'─',40}");
            if (_transactionLog.Count == 0)
                Console.WriteLine("    No transactions");
            else
                foreach (string t in _transactionLog)
                    Console.WriteLine($"    {t}");
            Console.WriteLine($"  {'─',40}");
            Console.WriteLine($"    Current Balance: {_balance:C2}");
        }
    }

    // ── Main Demo ─────────────────────────────────────────────────
    internal class ExceptionHandlingDemo
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            // ── Global exception handler (last resort) ────────────
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[GLOBAL CRASH] {((Exception)e.ExceptionObject).Message}");
                Console.ResetColor();
            };

            Console.WriteLine("╔═════════════════════════════════════════════╗");
            Console.WriteLine("║  Bank Account — Exception Handling Demo  🏦 ║");
            Console.WriteLine("╚═════════════════════════════════════════════╝\n");

            // ── 1. ArgumentException on construction ──────────────
            Console.WriteLine("=== 1. Construction Validation ===");
            try
            {
                var badAccount = new BankAccount("X001", "Alice", -500m);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"  ❌ ArgumentOutOfRangeException: {ex.Message}");
            }

            try
            {
                var nullAccount = new BankAccount(null!, "Bob", 100m);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"  ❌ ArgumentNullException: {ex.ParamName} cannot be null");
            }

            // ── 2. Normal operations ──────────────────────────────
            Console.WriteLine("\n=== 2. Normal Operations ===");
            BankAccount alice = new("A001", "Alice", 500m);
            BankAccount bob = new("B001", "Bob", 200m);

            alice.Deposit(300m);
            alice.Withdraw(100m);
            bob.Deposit(50m);

            // ── 3. InsufficientFundsException ─────────────────────
            Console.WriteLine("\n=== 3. Insufficient Funds ===");
            try
            {
                alice.Withdraw(9999m);
            }
            catch (InsufficientFundsException ex)
            {
                Console.WriteLine($"  ❌ {ex.Message}");
                Console.WriteLine($"     Tried: {ex.Amount:C2} | Had: {ex.Balance:C2}");
            }

            // ── 4. DailyLimitExceededException ────────────────────
            Console.WriteLine("\n=== 4. Daily Limit ===");
            try
            {
                alice.Withdraw(600m);   // fine
                //alice.Withdraw(600m);   // exceeds daily limit of $1000
            }
            catch (DailyLimitExceededException ex)
            {
                Console.WriteLine($"  ❌ {ex.Message}");
            }

            // ── 5. AccountFrozenException ─────────────────────────
            Console.WriteLine("\n=== 5. Frozen Account ===");
            bob.Freeze();
            try
            {
                bob.Deposit(100m);
            }
            catch (AccountFrozenException ex)
            {
                Console.WriteLine($"  ❌ AccountFrozenException: {ex.Message}");
                Console.WriteLine($"     Account ID: {ex.AccountId}");
            }

            // ── 6. Transfer with try-catch-finally ────────────────
            Console.WriteLine("\n=== 6. Transfer with finally (cleanup) ===");
            BankAccount carol = new("C001", "Carol", 1000m);
            BankAccount dave = new("D001", "Dave", 100m);

            try
            {
                carol.Transfer(dave, 300m);
                carol.Transfer(dave, 800m);    // will fail: only 700 left after first transfer
            }
            catch (InsufficientFundsException ex)
            {
                Console.WriteLine($"  ❌ Transfer failed: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"  ❌ Operation failed: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"     Caused by: {ex.InnerException.Message}");
            }
            finally
            {
                // Always runs — log the state regardless
                Console.WriteLine("  🔍 [finally] Logging final state...");
                Console.WriteLine($"     Carol: {carol.Balance:C2} | Dave: {dave.Balance:C2}");
            }

            // ── 7. throw vs throw ex demonstration ────────────────
            Console.WriteLine("\n=== 7. throw vs throw ex ===");
            try
            {
                try
                {
                    alice.Withdraw(5000m);    // throws InsufficientFundsException
                }
                catch (InsufficientFundsException ex)
                {
                    Console.WriteLine($"  Inner catch — rethrowing with bare 'throw'...");
                    throw;    // ✅ preserves original stack trace
                              // throw ex;  ❌ would reset the stack trace to this line
                }
            }
            catch (InsufficientFundsException ex)
            {
                Console.WriteLine($"  Outer catch — original exception preserved: {ex.Message}");
            }

            // ── 8. Statements ─────────────────────────────────────
            Console.WriteLine();
            alice.PrintStatement();
            carol.PrintStatement();

            Console.WriteLine("\n✅ All exception patterns demonstrated!");
        }
    }
}
