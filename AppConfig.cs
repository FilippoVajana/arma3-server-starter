using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace arma3_server_starter.Config
{
    public class AppConfig
    {   
        [JsonIgnore]
        private string _configPath;        
        public AppInfo AppInfo {get; private set;}
        public AppSettings AppSettings {get; private set;}        
        private Func<string, bool> appsettingsExists = delegate(string path)
        {
            return File.Exists(Path.Combine(path, "appsettings.json"));
        };

        public AppConfig()
        {
            _configPath = Directory.GetCurrentDirectory();

            if (appsettingsExists(_configPath) == false)
            {     
                System.Console.WriteLine("App config file not found, initialize a new one.");

                AppInfo = new AppInfo();
                AppSettings = InitAppSettings();

                SaveSettingsFile();
            }
        }
        
        private AppSettings InitAppSettings()
        {
            var settings = new AppSettings();

            System.Console.WriteLine("Server root:");
            settings.ServerRoot = Console.ReadLine();

            System.Console.WriteLine("Mods directory:");
            settings.ModsDir = Console.ReadLine();
            
            System.Console.WriteLine("Missions directory:");
            settings.MissionsDir = Console.ReadLine();
            
            return settings;            
        }

        private void SaveSettingsFile()
        {
            var json = JsonConvert.SerializeObject(this);
            var name = Path.Combine(_configPath, "appsettings.json");
            File.WriteAllText(name, json);

            System.Console.WriteLine($"Saved new app configuration file {name}");
        }

        public IConfiguration Build()
        {
            // init builder
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }
    }

    public class AppInfo
    {
        public string Version => "0.3";
    }
    public class AppSettings
    { 
        public string ServerRoot { get; internal set; }
        public string ModsDir { get; internal set; }
        public string MissionsDir { get; internal set; }
    }
}