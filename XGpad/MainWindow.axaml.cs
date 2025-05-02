using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using MsBox.Avalonia;
using XGpad.Resources.Scripts;
using XGpad.Resources.Tools;

namespace XGpad;

public partial class MainWindow : Window
{
    TabControl _JoyStack;
    public MainWindow()
    {
        InitializeComponent();
        _JoyStack = this.FindControl<TabControl>("JoyStack");
    }

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var joys = DeviceHelper.GetGamepads();
        _JoyStack.Items.Clear(); // Optional: clear previous entries
        int indexNum = 0;

        foreach (var joy in joys)
        {
            // Create a new TabItem for the current joy
            TabItem joyButton = new()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Header = $"{joy.Name} [{indexNum}]", // Displaying name and index
                Tag = joy
            };
            var mapping = GetSavedMapping(joy);

            // Create the GamepadView and associate it with the current joy
            var view = new GamepadView
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                _device = joy,
                controllerMapping = mapping
            };

            // Get the saved mapping for the current joy

            // Apply mapping settings to the view
            if (mapping != null)
            {
                view.LYinverseSW.IsChecked = mapping.INVERSE_LY;
                view.LXinverseSW.IsChecked = mapping.INVERSE_LX;
                view.RYinverseSW.IsChecked = mapping.INVERSE_RY;
                view.RXinverseSW.IsChecked = mapping.INVERSE_RX;
                view.RumbleSlider.Value = mapping.RUMBLE_GAIN.Value;
                view.RumbleCB.IsChecked = mapping.RUMBLE_ENABLE;
            }
            else
            {
                // Optionally handle the case where mapping is null, e.g., setting defaults.
                view.RumbleSlider.Value = 0;
                view.RumbleCB.IsChecked = false;
            }

            // Add the GamepadView to the TabItem and then to the stack
            joyButton.Content = view;
            view.controllerMapping = mapping;
            _JoyStack.Items.Add(joyButton);
        
            // Increment index and invalidate visual for proper rendering
            indexNum++;
            view.InvalidateVisual();
        }
    }

    private ControllerMapping GetSavedMapping(DeviceEntry joy)
    {
        // Ensure LoadData() is loading the data correctly (check if it's deserialized properly)
        var savedMappings = DataManager.LoadData(); 
        if (savedMappings == null || !savedMappings.Any())
        {
            return null; // Return null if no data is loaded
        }

        var mapping = savedMappings.FirstOrDefault(x => string.Equals(x.CONTROLLER_NAME, joy.Name, StringComparison.OrdinalIgnoreCase));
    
        if (mapping == null)
        {
            // Handle case where no mapping is found
            Console.WriteLine($"No saved mapping found for {joy.Name}");
            return null;
        }

        return mapping;
    }

}

