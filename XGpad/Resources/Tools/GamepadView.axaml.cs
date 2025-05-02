using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using EvDevSharp;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using XGpad.Resources.Scripts;

namespace XGpad.Resources.Tools;

public partial class GamepadView : UserControl
{
    public DeviceEntry _device;
    public XboxdrvHelper xboxdrvHelper = new XboxdrvHelper();
    public ControllerMapping controllerMapping ;
    
    public GamepadView()
    {
        InitializeComponent();
    }
    
    private void InitializeMappingUI()
    {
            if (controllerMapping != null)
            {
                MessageBoxManager.GetMessageBoxStandard("test",controllerMapping.INVERSE_LY.Value.ToString()).ShowAsync();
                RumbleSlider.Value = controllerMapping.RUMBLE_GAIN.Value;
                LYinverseSW.IsChecked = controllerMapping.INVERSE_LY;
                LXinverseSW.IsChecked = controllerMapping.INVERSE_LX;
                RYinverseSW.IsChecked = controllerMapping.INVERSE_RY;
                RXinverseSW.IsChecked = controllerMapping.INVERSE_RX;
            }
        
    }

    private void ButtonA_OnClick(object? sender, RoutedEventArgs e)
{
    string MappingData = DeviceHelper.Inputremapper(DeviceEntry.GetEventValue(_device));
    MessageBoxManager.GetMessageBoxStandard("TEST MAPPING", MappingData.ToString()).ShowAsync();
    if (!string.IsNullOrWhiteSpace(MappingData) && MappingData.StartsWith("BTN"))
    {
        controllerMapping.BTN_A = MappingData;
    }
}

private void ButtonB_OnClick(object? sender, RoutedEventArgs e)
{
    string MappingData = DeviceHelper.Inputremapper(DeviceEntry.GetEventValue(_device));
    MessageBoxManager.GetMessageBoxStandard("TEST MAPPING", MappingData.ToString()).ShowAsync();
    if (!string.IsNullOrWhiteSpace(MappingData) && MappingData.StartsWith("BTN"))
    {
        controllerMapping.BTN_B = MappingData;
    }
}

private void ButtonX_OnClick(object? sender, RoutedEventArgs e)
{
    string MappingData = DeviceHelper.Inputremapper(DeviceEntry.GetEventValue(_device));
    MessageBoxManager.GetMessageBoxStandard("TEST MAPPING", $"OG: {controllerMapping.BTN_X}\nMF: {MappingData}").ShowAsync();
    if (!string.IsNullOrWhiteSpace(MappingData) && MappingData.StartsWith("BTN"))
    {
        controllerMapping.BTN_X = MappingData;
    }
}

private void ButtonY_OnClick(object? sender, RoutedEventArgs e)
{
    string MappingData = DeviceHelper.Inputremapper(DeviceEntry.GetEventValue(_device));
    MessageBoxManager.GetMessageBoxStandard("TEST MAPPING", MappingData.ToString()).ShowAsync();
    if (!string.IsNullOrWhiteSpace(MappingData) && MappingData.StartsWith("BTN"))
    {
        controllerMapping.BTN_Y = MappingData;
    }
}

private void ButtonLB_OnClick(object? sender, RoutedEventArgs e)
{
    string MappingData = DeviceHelper.Inputremapper(DeviceEntry.GetEventValue(_device));
    MessageBoxManager.GetMessageBoxStandard("TEST MAPPING", MappingData.ToString()).ShowAsync();
    if (!string.IsNullOrWhiteSpace(MappingData) && MappingData.StartsWith("BTN"))
    {
        controllerMapping.BTN_LB = MappingData;
    }
}

private void ButtonLT_OnClick(object? sender, RoutedEventArgs e)
{
    string MappingData = DeviceHelper.Inputremapper(DeviceEntry.GetEventValue(_device));
    MessageBoxManager.GetMessageBoxStandard("TEST MAPPING", MappingData.ToString()).ShowAsync();
    if (!string.IsNullOrWhiteSpace(MappingData) && MappingData.StartsWith("BTN"))
    {
        controllerMapping.BTN_LT = MappingData;
    }
}

private void ButtonRB_OnClick(object? sender, RoutedEventArgs e)
{
    string MappingData = DeviceHelper.Inputremapper(DeviceEntry.GetEventValue(_device));
    MessageBoxManager.GetMessageBoxStandard("TEST MAPPING", MappingData.ToString()).ShowAsync();
    if (!string.IsNullOrWhiteSpace(MappingData) && MappingData.StartsWith("BTN"))
    {
        controllerMapping.BTN_RB = MappingData;
    }
}

private void ButtonRT_OnClick(object? sender, RoutedEventArgs e)
{
    string MappingData = DeviceHelper.Inputremapper(DeviceEntry.GetEventValue(_device));
    MessageBoxManager.GetMessageBoxStandard("TEST MAPPING", MappingData.ToString()).ShowAsync();
    if (!string.IsNullOrWhiteSpace(MappingData) && MappingData.StartsWith("BTN"))
    {
        controllerMapping.BTN_RT = MappingData;
    }
}

private void ButtonStart_OnClick(object? sender, RoutedEventArgs e)
{
    string MappingData = DeviceHelper.Inputremapper(DeviceEntry.GetEventValue(_device));
    MessageBoxManager.GetMessageBoxStandard("TEST MAPPING", MappingData.ToString()).ShowAsync();
    if (!string.IsNullOrWhiteSpace(MappingData) && MappingData.StartsWith("BTN"))
    {
        controllerMapping.BTN_START = MappingData;
    }
}

private void ButtonBack_OnClick(object? sender, RoutedEventArgs e)
{
    string MappingData = DeviceHelper.Inputremapper(DeviceEntry.GetEventValue(_device));
    MessageBoxManager.GetMessageBoxStandard("TEST MAPPING", MappingData.ToString()).ShowAsync();
    if (!string.IsNullOrWhiteSpace(MappingData) && MappingData.StartsWith("BTN"))
    {
        controllerMapping.BTN_BACK = MappingData;
    }
}

private void ButtonLS_OnClick(object? sender, RoutedEventArgs e)
{
    string MappingData = DeviceHelper.Inputremapper(DeviceEntry.GetEventValue(_device));
    MessageBoxManager.GetMessageBoxStandard("TEST MAPPING", MappingData.ToString()).ShowAsync();
    if (!string.IsNullOrWhiteSpace(MappingData) && MappingData.StartsWith("BTN"))
    {
        controllerMapping.BTN_LS = MappingData;
    }
}

private void ButtonRS_OnClick(object? sender, RoutedEventArgs e)
{
    string MappingData = DeviceHelper.Inputremapper(DeviceEntry.GetEventValue(_device));
    MessageBoxManager.GetMessageBoxStandard("TEST MAPPING", MappingData.ToString()).ShowAsync();
    if (!string.IsNullOrWhiteSpace(MappingData) && MappingData.StartsWith("BTN"))
    {
        controllerMapping.BTN_RS = MappingData;
    }
}


    

private async void StartEmuBTN_OnClick(object? sender, RoutedEventArgs e)
{
    if (xboxdrvHelper.IsRunning() || StartEmuBTN.Content == "STOP")
    {
        // Stop the emulation if it's running
        StartEmuBTN.Content = "START Emulation";
        await xboxdrvHelper.StopXboxdrv();
    }
    else
    {
        xboxdrvHelper = new XboxdrvHelper();
        // Start the emulation if it's not running
        await xboxdrvHelper.StartXboxdrv("/dev/input/" + DeviceEntry.GetEventValue(_device), controllerMapping);
        if (xboxdrvHelper.IsRunning())
        {
            StartEmuBTN.Content = "STOP";    
        }
        
    }
}


    private void SaveMappingBTN_OnClick(object? sender, RoutedEventArgs e)
    {
        controllerMapping.CONTROLLER_NAME = _device.Name;
       DataManager.SaveData(controllerMapping);
    }


    private void LYinverseSW_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (controllerMapping != null)
        {
            controllerMapping.INVERSE_LY = LYinverseSW.IsChecked.GetValueOrDefault();
        }
        else
        {
            // Handle the case where controllerMapping is null
            Console.WriteLine("controllerMapping is null");
        }
    }

    private void LXinverseSW_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (controllerMapping != null)
        {
            controllerMapping.INVERSE_LX = LXinverseSW.IsChecked.GetValueOrDefault();
        }
        else
        {
            // Handle the case where controllerMapping is null
            Console.WriteLine("controllerMapping is null");
        }
    }

    private void RYinverseSW_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (controllerMapping != null)
        {
            controllerMapping.INVERSE_RY = RYinverseSW.IsChecked.GetValueOrDefault();
        }
        else
        {
            // Handle the case where controllerMapping is null
            Console.WriteLine("controllerMapping is null");
        }
    }

    private void RXinverseSW_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (controllerMapping != null)
        {
            controllerMapping.INVERSE_RX = RXinverseSW.IsChecked.GetValueOrDefault();
        }
        else
        {
            // Handle the case where controllerMapping is null
            Console.WriteLine("controllerMapping is null");
        }
    }


    private void ButtonRSUp_OnClick(object? sender, RoutedEventArgs e)
    {
        string MappingData = DeviceHelper.Inputremapper(DeviceEntry.GetEventValue(_device), true);
        MessageBoxManager.GetMessageBoxStandard("TEST MAPPING", MappingData.ToString()).ShowAsync();
        if (!string.IsNullOrWhiteSpace(MappingData) && MappingData.StartsWith("ABS"))
        {
            controllerMapping.AXIS_RY = MappingData;
        }    
    }
    
    private void ButtonLSY_OnClick(object? sender, RoutedEventArgs e)
    {
        string MappingData = DeviceHelper.Inputremapper(DeviceEntry.GetEventValue(_device), true);
        MessageBoxManager.GetMessageBoxStandard("TEST MAPPING", MappingData.ToString()).ShowAsync();
        if (!string.IsNullOrWhiteSpace(MappingData) && MappingData.StartsWith("ABS"))
        {
            controllerMapping.AXIS_LY= MappingData;
        }    
    }

    private void ButtonLSX_OnClick(object? sender, RoutedEventArgs e)
    {
        string MappingData = DeviceHelper.Inputremapper(DeviceEntry.GetEventValue(_device), true);
        MessageBoxManager.GetMessageBoxStandard("TEST MAPPING", MappingData.ToString()).ShowAsync();
        if (!string.IsNullOrWhiteSpace(MappingData) && MappingData.StartsWith("ABS"))
        {
            controllerMapping.AXIS_LX = MappingData;
        }    
    }

    private void ButtonRSY_OnClick(object? sender, RoutedEventArgs e)
    {
        string MappingData = DeviceHelper.Inputremapper(DeviceEntry.GetEventValue(_device), true);
        MessageBoxManager.GetMessageBoxStandard("TEST MAPPING", MappingData.ToString()).ShowAsync();
        if (!string.IsNullOrWhiteSpace(MappingData) && MappingData.StartsWith("ABS"))
        {
            controllerMapping.AXIS_RY = MappingData;
        }    
    }

    private void ButtonRSX_OnClick(object? sender, RoutedEventArgs e)
    {
        string MappingData = DeviceHelper.Inputremapper(DeviceEntry.GetEventValue(_device), true);
        MessageBoxManager.GetMessageBoxStandard("TEST MAPPING", MappingData.ToString()).ShowAsync();
        if (!string.IsNullOrWhiteSpace(MappingData) && MappingData.StartsWith("ABS"))
        {
            controllerMapping.AXIS_RX = MappingData;
        }    
    }

    private void RumbleSlider_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        controllerMapping.RUMBLE_GAIN = RumbleSlider.Value;
    }

    private void RumbleCB_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        controllerMapping.RUMBLE_ENABLE = RumbleCB.IsChecked;
    }
}

public class ControllerMapping
{ 
    public string? CONTROLLER_NAME { get; set; } = "joystick";
    public bool? INVERSE_LY { get; set; } = false;
    public bool? INVERSE_RY { get; set; } = false;
    public bool? INVERSE_LX { get; set; } = false;
    public bool? INVERSE_RX { get; set; } = false;
    public string? BTN_A { get; set; } = "BTN_THUMB2";
    public string? BTN_X { get; set; }= "BTN_TOP";
    public string? BTN_B { get; set; }= "BTN_THUMB";
    public string? BTN_Y { get; set; }= "BTN_TRIGGER";
    
    public string? BTN_RB { get; set; }= "BTN_BASE2";
    public string? BTN_RT { get; set; }= "BTN_PINKIE";
    public string? BTN_LB { get; set; }= "BTN_BASE";
    public string? BTN_LT { get; set; }= "BTN_TOP2";
    
    public string? BTN_START { get; set; }= "BTN_BASE4";
    public string? BTN_BACK { get; set; }= "BTN_BASE3";
    public string? BTN_LS { get; set; }= "BTN_BASE5";
    public string? BTN_RS { get; set; }= "BTN_BASE6";
    public string? AXIS_RX { get; set; }= "ABS_RZ";
    public string? AXIS_RY { get; set; }= "ABS_Z";
    public string? AXIS_LX { get; set; }= "ABS_X";
    public string? AXIS_LY { get; set; }= "ABS_Y";
    public bool? RUMBLE_ENABLE { get; set; } = true;
    public double? RUMBLE_GAIN {get; set; } = 100f;
}