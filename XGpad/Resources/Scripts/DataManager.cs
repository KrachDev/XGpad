using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using XGpad.Resources.Tools;

namespace XGpad.Resources.Scripts;

public class DataManager
    {// Get the directory where the application is running
        public static string appDirectory = AppDomain.CurrentDomain.BaseDirectory;

        // Combine the directory with the file name to get the full file path
        public static string filePath = Path.Combine(appDirectory, "InputsModified.json");



       public static void SaveData(ControllerMapping entry)
        { 
            bool fileLocked = true;
            int retries = 0;
            const int maxRetries = 3;

            while (fileLocked && retries < maxRetries)
            {
                try
                {
                    List<ControllerMapping> entries = new List<ControllerMapping>();

                    // 2. Handle potential deserialization errors
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            string jsonData = File.ReadAllText(filePath);
                            if (entries != null)
                            {
                                entries = JsonConvert.DeserializeObject<List<ControllerMapping>>(jsonData);

                            }
                            else
                            {
                                entries = new List<ControllerMapping>();
                            }

                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"Failed to deserialize existing game data: {ex.Message}");
                        }
                    }
                    // 3. Find and update or add entry
                    try
                    {




                        if (entries != null)
                        {
                            int index = entries.FindIndex(e => e.CONTROLLER_NAME == entry.CONTROLLER_NAME);
                            if (index != -1)
                            {
                                entries[index] = entry;
                            }
                            else
                            {
                                entries.Add(entry);
                            }

                        }
                        else
                        {
                            entries = new List<ControllerMapping> { entry };

                        }


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Checking Existing Game Error: " + ex.Message);
                    }

                    // 4. Serialize and write data with error handling
                    string json = JsonConvert.SerializeObject(entries, Formatting.Indented);
                    using (StreamWriter sw = new StreamWriter(filePath))
                    {
                        try
                        {
                            sw.Write(json);
                            fileLocked = false; // Successful write unlocks the file
                        }
                        catch (IOException ex)
                        {
                            Console.WriteLine($"Failed to write data to file: {ex.Message}");
                        }
                        finally
                        {
                            sw.Dispose(); // Explicitly dispose StreamWriter
                        }
                    }
                }
                catch (IOException)
                {
                    // File is locked, wait and retry
                    retries++;
                    System.Threading.Thread.Sleep(1000);
                }
            }

            if (fileLocked)
            {
                // 5. Failed to save data after retries
                Console.WriteLine("Failed to save data. File is locked by another process.");
            }
        }

        public static List<ControllerMapping> LoadData()
        {
            // Check if the file exists
            if (File.Exists(filePath))
            {
                // Read the JSON string from the file
                string json = File.ReadAllText(filePath);

                // Deserialize the JSON string to a List of ControllerMapping objects
                List<ControllerMapping> entries = JsonConvert.DeserializeObject<List<ControllerMapping>>(json);
                return entries ?? new List<ControllerMapping>(); // return an empty list if null
            }
            else
            {
                // Return an empty list if the file does not exist
                return new List<ControllerMapping>();
            }
        }    }