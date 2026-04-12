using System;
using System.IO;
using System.Collections.Generic;

public class Extract{
    public static string PromptForFolderLocation(){
        while (true){

            Console.Write("Enter the folder location: ");
            string? folderPath = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(folderPath) && Directory.Exists(folderPath)){
                Console.WriteLine($"Folder found: {folderPath}\n");
                return folderPath;
            }
            else{
                Console.WriteLine($"No folder exists at '{folderPath}'. Please try again.\n");
            }
        }
    }
    public static void FileCountAndType(string folderPath){
        // Cannot handle Sub-Folders
        string[] files = Directory.GetFiles(folderPath);
        var fileTypes = new Dictionary<string, int>();
        
        foreach (string filePath in files){
            string fileExtension = Path.GetExtension(filePath);
            if (fileTypes.ContainsKey(fileExtension)){
                fileTypes[fileExtension] += 1;
            }
            else{
                fileTypes[fileExtension] = 1;
            }
        }
        Console.WriteLine($"\nTotal files: {files.Length}");
        foreach (string type in fileTypes.Keys){
            Console.WriteLine($"{type} files: {fileTypes[type]}");
        }
    }

    

    public static void Main(string[] args){
        if (args.Length > 0 && args[0] == "geo"){
            TestGeoTree();
            return;
        }
        string folderPath = PromptForFolderLocation();
        FileCountAndType(folderPath);
    }

    public static void TestGeoTree(){
        var geo = new GeoTree("geoTree.json");
        Console.WriteLine("GeoTree Test - Enter coordinates to lookup cities");
        Console.WriteLine("Format: lat,lon (e.g., 42.50779,1.52109)");
        Console.WriteLine("Type 'exit' to quit\n");

        while (true){
            Console.Write("Enter coords: ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input) || input.ToLower() == "exit")
                break;

            string[] parts = input.Split(',');
            if (parts.Length != 2){
                Console.WriteLine("Invalid format. Use: lat,lon\n");
                continue;
            }

            if (double.TryParse(parts[0].Trim(), out double lat) && 
                double.TryParse(parts[1].Trim(), out double lon)){
                var (city, country) = geo.Lookup(lat, lon);
                if (city != null){
                    Console.WriteLine($"Found: {city} ({country})\n");
                }
                else{
                    Console.WriteLine("No city found at those coordinates\n");
                }
            }
            else{
                Console.WriteLine("Could not parse coordinates\n");
            }
        }
    }
}
