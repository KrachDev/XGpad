using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace XGpad.Resources.Scripts;

public class DeviceHelper
{
  [StructLayout(LayoutKind.Sequential)]
public struct timeval
{
    public long tv_sec;
    public long tv_usec;
}

[StructLayout(LayoutKind.Sequential)]
public struct input_event
{
    public timeval time;
    public ushort type;
    public ushort code;
    public int value;
}

private const ushort EV_KEY = 0x01;
private const ushort EV_ABS = 0x03;

// Button mappings (as in your original code)
private static readonly Dictionary<int, string> ButtonNames = new()
{
    { 0x120, "BTN_TRIGGER" },
    { 0x121, "BTN_THUMB" },
    { 0x122, "BTN_THUMB2" },
    { 0x123, "BTN_TOP" },
    { 0x124, "BTN_TOP2" },
    { 0x125, "BTN_PINKIE" },
    { 0x126, "BTN_BASE" },
    { 0x127, "BTN_BASE2" },
    { 0x128, "BTN_BASE3" },
    { 0x129, "BTN_BASE4" },
    { 0x12a, "BTN_BASE5" },
    { 0x12b, "BTN_BASE6" },
};

// Axis mappings (added for axis codes)
    private static readonly Dictionary<int, string> AxisNames = new()
    {
        { 0x00, "ABS_X" }, // Left X Axis
        { 0x01, "ABS_Y" }, // Left Y Axis
        { 0x02, "ABS_Z" }, // Left Trigger
        { 0x03, "ABS_RZ" }, // Right Trigger
        { 0x10, "ABS_RX" }, // Right X Axis
        { 0x11, "ABS_RY" }, // Right Y Axis
        { 0x12, "ABS_HAT0X" }, // D-Pad X
        { 0x13, "ABS_HAT0Y" }, // D-Pad Y
        { 0x14, "ABS_RX2" }, // Right X Axis 2
        { 0x15, "ABS_RY2" }, // Right Y Axis 2
        { 0x05, "ABS_RZ" }, // Example: Right Trigger Axis (Axis code 5)
    };


public static string Inputremapper(string devicePath, bool isAxis = false, CancellationToken? cancelToken = null)
{
    if (string.IsNullOrWhiteSpace(devicePath))
    {
        return "Invalid Path";
    }

    string fullPath = $"/dev/input/{devicePath}";
    int structSize = Marshal.SizeOf<input_event>();

    try
    {
        using var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        byte[] buffer = new byte[structSize];

        while (true)
        {
            cancelToken?.ThrowIfCancellationRequested();

            int read = fs.Read(buffer, 0, structSize);
            if (read != structSize)
                continue;

            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var ev = Marshal.PtrToStructure<input_event>(handle.AddrOfPinnedObject());
            handle.Free();
                
            if (ev.type == EV_KEY)
            {
                // Handle button events (pressed, released, held)
                string name = ButtonNames.TryGetValue(ev.code, out var val) ? val : $"UNKNOWN({ev.code})";

                return name;
            }
            // Handle axis events
            else if (ev.type == EV_ABS && isAxis)
            {                  
                string name = AxisNames.TryGetValue(ev.code, out var val) ? val : $"UNKNOWN({ev.code})";
                int axisValue = ev.value;

                // Apply axis inversion if needed (e.g., for Y axes, we invert by multiplying by -1)
                if (name.Contains("Y")) // Invert the Y axis for certain cases
                {
                    axisValue = -axisValue;
                }

                return name;
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error reading from device: {ex.Message}");
        return ex.Message;
    }
}

    public static List<DeviceEntry> GetGamepads(string path = "/proc/bus/input/devices")
    {
        var lines = File.ReadAllLines(path);
        var gamepads = new List<DeviceEntry>();
        var current = new DeviceEntry();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                // Check if this device has a js handler (i.e., it's a joystick/gamepad)
                if (!string.IsNullOrWhiteSpace(current.Handlers) && current.Handlers.Contains("js"))
                    gamepads.Add(current);

                current = new DeviceEntry(); // Start next device
                continue;
            }

            var trimmed = line.Trim();

            if (trimmed.StartsWith("I:"))
            {
                var parts = trimmed.Substring(2).Split();
                foreach (var part in parts)
                {
                    if (part.StartsWith("Bus=")) current.Bus = part.Substring(4);
                }
            }
            else if (trimmed.StartsWith("N:")) current.Name = trimmed.Split('=')[1].Trim('"');
            else if (trimmed.StartsWith("P:")) current.Phys = trimmed.Substring(2).Trim();
            else if (trimmed.StartsWith("S:")) current.Sysfs = trimmed.Substring(2).Trim();
            else if (trimmed.StartsWith("U:")) current.Uniq = trimmed.Substring(2).Trim();
            else if (trimmed.StartsWith("H:")) current.Handlers = trimmed.Substring(2).Trim();
            else if (trimmed.StartsWith("B: PROP=")) current.PROP = Convert.ToInt32(trimmed.Substring(8), 16);
            else if (trimmed.StartsWith("B: EV=")) current.EV = Convert.ToInt32(trimmed.Substring(6), 16);
        }

        // Check last entry in case file doesn't end with a blank line
        if (!string.IsNullOrWhiteSpace(current.Handlers) && current.Handlers.Contains("js"))
            gamepads.Add(current);

        return gamepads;
    }

}
public class DeviceEntry
{
    public string Bus { get; set; }
    public string Name { get; set; }
    public string Phys { get; set; }
    public string Sysfs { get; set; }
    public string Uniq { get; set; }
    public string Handlers { get; set; }
    public int PROP { get; set; }
    public int EV { get; set; }
    
    public static string GetJsvalue(DeviceEntry device)
    {
        return device.Handlers.Split(' ')[1].Replace("Handlers=", "");
    }

    public static string GetEventValue(DeviceEntry device)
    {
        return device.Handlers.Split(' ')[0].Replace("Handlers=", "");

    }
}