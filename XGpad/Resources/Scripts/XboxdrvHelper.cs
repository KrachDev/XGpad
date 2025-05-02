using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MsBox.Avalonia;
using XGpad.Resources.Tools;

public class XboxdrvHelper
{
    Process process = null;
    public async Task<Process> StartXboxdrv(string devicePath, ControllerMapping mappingSchema)
    {
        try
        {
            string buttonMap = string.Join(",",
                $"{mappingSchema.BTN_A}=a",
                $"{mappingSchema.BTN_B}=b",
                $"{mappingSchema.BTN_X}=x",
                $"{mappingSchema.BTN_Y}=y",
                $"{mappingSchema.BTN_LB}=lb",
                $"{mappingSchema.BTN_LT}=lt",
                $"{mappingSchema.BTN_RB}=rb",
                $"{mappingSchema.BTN_RT}=rt",
                $"{mappingSchema.BTN_START}=start",
                $"{mappingSchema.BTN_BACK}=back",
                $"{mappingSchema.BTN_LS}=tl",
                $"{mappingSchema.BTN_RS}=tr"
            );
            string axisMap = string.Join(",",
                $"{mappingSchema.AXIS_LX}=x1",
                $"{mappingSchema.AXIS_LY}=y1",
                $"{mappingSchema.AXIS_RX}=x2",
                $"{mappingSchema.AXIS_RY}=y2",
                "ABS_HAT0X=dpad_x",
                "ABS_HAT0Y=dpad_y"
            );

            string axisInversion = string.Join(",",
                $"{(mappingSchema.INVERSE_LY.HasValue && mappingSchema.INVERSE_LY.Value ? "-Y1" : "Y1")}=Y1",
                $"{(mappingSchema.INVERSE_RY.HasValue && mappingSchema.INVERSE_RY.Value ? "-Y2" : "Y2")}=Y2",
                $"{(mappingSchema.INVERSE_LX.HasValue && mappingSchema.INVERSE_LX.Value ? "-X1" : "X1")}=X1",
                $"{(mappingSchema.INVERSE_RX.HasValue && mappingSchema.INVERSE_RX.Value ? "-X2" : "X2")}=X2"
            );



            var arguments = $"xboxdrv --evdev \"{devicePath}\" " +
                            $"--evdev-absmap {axisMap} " +
                            $"--axismap {axisInversion} " +
                            $"--evdev-keymap {buttonMap} " +
                            "--trigger-as-button " +
                            "--detach-kernel-driver " +
                            "--mimic-xpad " +
                            "--silent ";

            if (mappingSchema.RUMBLE_ENABLE.Value)
            {
                arguments += $"--force-feedback --rumble-gain {mappingSchema.RUMBLE_GAIN} --ff-device joystick ";
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = "pkexec",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = false // Set to true if you want no terminal window
            };

            process = new Process { StartInfo = startInfo };

            process.OutputDataReceived += (s, e) => Console.WriteLine(e.Data);
            process.ErrorDataReceived += (s, e) => MessageBoxManager.GetMessageBoxStandard("Error", e.Data).ShowAsync();

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return process;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public bool IsRunning()
    {
        if (process == null)
        {
            return false;
        }
        else
        {
            return !process.HasExited;
        }
    }
    public async Task StopXboxdrv()
    {
        if (process != null && !process.HasExited)
        {
            Process.Start("pkexec", $"kill {process.Id}");
            
        }
    }
}