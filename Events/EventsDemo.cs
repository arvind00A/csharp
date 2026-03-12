// ============================================================
//  PROJECT 3 — Events & Event Handling
//  Smart Home Automation System
//  Topics: event keyword, EventHandler<T>, custom EventArgs,
//          subscribe/unsubscribe, multiple subscribers,
//          event with cancellation
// ============================================================


using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Events
{
    // ── EventArgs ─────────────────────────────────────────────────
    public class SensorReadingEventArgs : EventArgs
    {
        public string SensorId { get; }
        public double Value { get; }
        public string Unit { get; }
        public DateTime ReadAt { get; } = DateTime.Now;

        public SensorReadingEventArgs(string id, double value, string unit)
            => (SensorId, Value, Unit) = (id, value, unit);
    }

    public class DeviceStateChangedEventArgs : EventArgs
    {
        public string DeviceId { get; }
        public string OldState { get; }
        public string NewState { get; }
        public string? Reason { get; }

        public DeviceStateChangedEventArgs(string id, string old, string @new, string? reason = null)
            => (DeviceId, OldState, NewState, Reason) = (id, old, @new, reason);
    }

    public class AlertEventArgs : EventArgs
    {
        public string SensorId { get; }
        public string AlertType { get; }
        public string Message { get; }
        public bool IsCritical { get; }

        public AlertEventArgs(string sensorId, string type, string msg, bool critical = false)
            => (SensorId, AlertType, Message, IsCritical) = (sensorId, type, msg, critical);
    }


    // Supports cancellation by subscriber
    public class CommandEventArgs : EventArgs
    {
        public string DeviceId { get; }
        public string Command { get; }
        public bool Cancel { get; set; } = false;
        public string? CancelReason { get; set; }

        public CommandEventArgs(string deviceId, string command)
            => (DeviceId, Command) = (deviceId, command);
    }


    // ── Publisher: Sensor ─────────────────────────────────────────
    public class TemperatureSensor
    {
        public string Id { get; }
        public string Room { get; }
        private double _current = 20.0;

        public double MinThreshold { get; set; } = 15.0;
        public double MaxThreshold { get; set; } = 30.0;

        // Events
        public event EventHandler<SensorReadingEventArgs>? ReadingTaken;
        public event EventHandler<AlertEventArgs>? AlertRaised;

        public TemperatureSensor(string id, string room) => (Id, Room) = (id, room);

        public void SetTemperature(double temp)
        {
            double previous = _current;
            _current = temp;

            // Fire reading event
            ReadingTaken?.Invoke(this,
                new SensorReadingEventArgs(Id, temp, "°C"));

            // Fire alert if threshold crossed
            if (temp > MaxThreshold)
                AlertRaised?.Invoke(this, new AlertEventArgs(
                    Id, "OVERHEAT",
                    $"{Room}: Temperature {temp}°C exceeds max {MaxThreshold}°C",
                    critical: temp > MaxThreshold + 5));

            if (temp < MinThreshold)
                AlertRaised?.Invoke(this, new AlertEventArgs(
                    Id, "TOO_COLD",
                    $"{Room}: Temperature {temp}°C below min {MinThreshold}°C"));
        }
    }


    // ── Publisher: Smart Device ───────────────────────────────────
    public class SmartDevice
    {
        public string Id { get; }
        public string Name { get; }
        private string _state = "OFF";

        public event EventHandler<DeviceStateChangedEventArgs>? StateChanged;
        public event EventHandler<CommandEventArgs>? BeforeCommand;

        public SmartDevice(string id, string name) => (Id, Name) = (id, name);

        public string State => _state;

        public void ExecuteCommand(string command)
        {
            // Fire BeforeCommand — subscribers can cancel
            var cmdArgs = new CommandEventArgs(Id, command);
            BeforeCommand?.Invoke(this, cmdArgs);

            if (cmdArgs.Cancel)
            {
                Console.WriteLine($"  ⛔ Command '{command}' cancelled: {cmdArgs.CancelReason}");
                return;
            }

            string newState = command switch
            {
                "TURN_ON" => "ON",
                "TURN_OFF" => "OFF",
                "SET_ECO_MODE" => "ECO",
                "SET_BOOST" => "BOOST",
                _ => _state
            };

            if (newState == _state) return;

            var prevState = _state;
            _state = newState;

            StateChanged?.Invoke(this,
                new DeviceStateChangedEventArgs(Id, prevState, newState));
        }

        public int SubscriberCount =>
            StateChanged?.GetInvocationList().Length ?? 0;

    }


    // ── Subscribers ───────────────────────────────────────────────
    public class DashboardDisplay
    {
        private List<string> _log = new();

        public void OnSensorReading(object? sender, SensorReadingEventArgs e)
        {
            string entry = $"  📊 [{e.ReadAt:HH:mm:ss}] {e.SensorId}: {e.Value}{e.Unit}";
            _log.Add(entry);
            Console.WriteLine(entry);
        }

        public void OnDeviceStateChanged(object? sender, DeviceStateChangedEventArgs e)
        {
            string icon = e.NewState switch { "ON" => "🟢", "OFF" => "🔴", "ECO" => "🌿", _ => "⚡" };
            Console.WriteLine($"  {icon} [{DateTime.Now:HH:mm:ss}] {e.DeviceId}: {e.OldState} → {e.NewState}");
        }

        public void PrintLog()
        {
            Console.WriteLine($"\n  Dashboard Log ({_log.Count} entries):");
            foreach (string entry in _log) Console.WriteLine(entry);
        }
    }

    public class AlertManager
    {
        private int _alertCount = 0;

        public void OnAlert(object? sender, AlertEventArgs e)
        {
            _alertCount++;
            string prefix = e.IsCritical ? "🚨 CRITICAL" : "⚠️  WARNING";
            Console.WriteLine($"  {prefix} [{e.AlertType}] {e.Message}");

            if (e.IsCritical)
                Console.WriteLine($"  ☎️  Notifying emergency contact...");
        }

        public int AlertCount => _alertCount;
    }


    public class EnergyMonitor
    {
        private Dictionary<string, int> _commandCount = new();

        public void OnDeviceStateChanged(object? sender, DeviceStateChangedEventArgs e)
        {
            _commandCount[e.DeviceId] = _commandCount.GetValueOrDefault(e.DeviceId, 0) + 1;
            double estimatedWatts = e.NewState switch
            {
                "ON" => 150.0,
                "ECO" => 60.0,
                "BOOST" => 300.0,
                "OFF" => 0.0,
                _ => 0.0
            };
            if (estimatedWatts > 0)
                Console.WriteLine($"  ⚡ Energy: {e.DeviceId} drawing ~{estimatedWatts}W ({e.NewState})");
        }

        public void PrintSummary()
        {
            Console.WriteLine("\n  Energy Monitor — Device Activity:");
            foreach (var (id, count) in _commandCount)
                Console.WriteLine($"    {id}: {count} state change(s)");
        }
    }


    public class SecurityGuard
    {
        private static readonly HashSet<string> _blockedCommands =
            new() { "OVERRIDE", "DISABLE_ALARM", "UNLOCK_ALL" };

        // Cancel dangerous commands
        public void OnBeforeCommand(object? sender, CommandEventArgs e)
        {
            if (_blockedCommands.Contains(e.Command))
            {
                e.Cancel = true;
                e.CancelReason = "Command blocked by security policy";
                Console.WriteLine($"  🛡️  Security: Blocked '{e.Command}' on {e.DeviceId}");
            }
        }
    }


    // ── Main ──────────────────────────────────────────────────────
    internal class EventsDemo
    {
       static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║  Events — Smart Home Automation 🏠       ║");
            Console.WriteLine("╚══════════════════════════════════════════╝\n");

            // Create devices and sensors
            var livingRoom = new TemperatureSensor("TEMP-LR", "Living Room");
            var bedroom = new TemperatureSensor("TEMP-BR", "Bedroom");
            bedroom.MaxThreshold = 26.0;

            var ac      = new SmartDevice("AC-01", "Air Conditioner");
            var heater  = new SmartDevice("HEAT-01", "Electric Heater");
            var smartTv = new SmartDevice("TV-01", "Smart TV");

            // Create subscribers
            var dashboard       = new DashboardDisplay();
            var alertManager    = new AlertManager();
            var energyMonitor   = new EnergyMonitor();
            var security        = new SecurityGuard();


            // ── Wire up events ────────────────────────────────────
            Console.WriteLine("=== Subscribing to events ===\n");

            // Sensor subscriptions
            livingRoom.ReadingTaken += dashboard.OnSensorReading;
            bedroom.ReadingTaken    += dashboard.OnSensorReading;
            livingRoom.AlertRaised  += alertManager.OnAlert;
            bedroom.AlertRaised     += alertManager.OnAlert;

            // Lambda subscriber - auto log alerts to "file"
            livingRoom.AlertRaised += (s, e) =>
                File.AppendAllText("alerts.log",
                    $"{DateTime.Now}:[{e.AlertType}] {e.Message}\n");

            // Device subscriptions
            ac.StateChanged      += dashboard.OnDeviceStateChanged;
            heater.StateChanged  += dashboard.OnDeviceStateChanged;
            smartTv.StateChanged += dashboard.OnDeviceStateChanged;

            ac.StateChanged     += energyMonitor.OnDeviceStateChanged;
            heater.StateChanged += energyMonitor.OnDeviceStateChanged;

            // Security gate on ALL devices
            ac.BeforeCommand        += security.OnBeforeCommand;
            heater.BeforeCommand    += security.OnBeforeCommand;

            Console.WriteLine("  All event handlers registered.\n");

            // ── Simulate sensor readings ──────────────────────────
            Console.WriteLine("=== Temperature Sensor Readings ===\n");
            livingRoom.SetTemperature(22.5);
            bedroom.SetTemperature(24.0);
            livingRoom.SetTemperature(31.0);  // triggers WARNING alert
            bedroom.SetTemperature(27.5);     // triggers bedroom WARNING
            livingRoom.SetTemperature(38.0);  // triggers CRITICAL alert
            bedroom.SetTemperature(14.0);     // triggers TOO_COLD alert

            // Security blocks dangerous commands
            Console.WriteLine("\n=== Security Blocked Commands ===\n");
            ac.ExecuteCommand("OVERRIDE");
            heater.ExecuteCommand("DISABLE_ALARM");

            // ── Unsubscribe ───────────────────────────────────────
            Console.WriteLine("\n=== Unsubscribe Demo ===\n");
            Console.WriteLine("  Removing dashboard from bedroom sensor...");
            bedroom.ReadingTaken -= dashboard.OnSensorReading;

            bedroom.SetTemperature(20.0);  // dashboard won't show this
            Console.WriteLine("  (Dashboard did NOT show the bedroom reading above)");

            // ── Multiple subscribers demo ─────────────────────────
            Console.WriteLine("\n=== Multiple Subscribers on One Event ===\n");
            
            Console.WriteLine($"  ac.StateChanged has {ac.SubscriberCount} subscribers");

            // Add a temporary lambda subscriber
            EventHandler<DeviceStateChangedEventArgs> tempHandler =
                (s, e) => Console.WriteLine($"  📱 Mobile app notified: {e.DeviceId} → {e.NewState}");
            ac.StateChanged += tempHandler;

            Console.WriteLine("\n  Turning AC to BOOST (all subscribers notified):");
            ac.ExecuteCommand("SET_BOOST");

            ac.StateChanged -= tempHandler;  // remove temporary subscriber
            Console.WriteLine($"\n  After removing temp handler: {ac.SubscriberCount} subscribers");

            // ── Final summaries ───────────────────────────────────
            energyMonitor.PrintSummary();
            Console.WriteLine($"\n  Total alerts raised: {alertManager.AlertCount}");

            Console.WriteLine("\n✅ Events & Event Handling demo complete!");
        }
    }
}
