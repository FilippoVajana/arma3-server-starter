using System;
using System.IO;
using System.Linq;
using arma3_server_starter.Config;
using Microsoft.Extensions.Configuration;

namespace arma3_server_starter
{
    class Program
    {
        public static IConfiguration appConfig;
       
        static void Main(string[] args)
        {
            // load app config
            appConfig = ReadAppSettings();
            Console.WriteLine($"Arma3 Server Launcher v{appConfig["AppInfo:app-version"]}");
            
            // save empty app config
            // var path = appConfig["AppSettings:server-missions-folder"];
            // SaveEmptyConfig(path);
            // return;

            // select server config
            var configPath = "";
            
            if (args.Length > 0 && args[0] != null) // check Main args[0] for config path input
            {
                System.Console.WriteLine(args[0]);
                configPath = args[0];
            }
            else
            {
                // find servers config files
                var configs = FindServersConfig();

                // display configs
                System.Console.WriteLine($"Found {configs.Length} configurations");
                for (int i = 0; i < configs.Length; i++)
                {
                    System.Console.WriteLine($"{i}) {configs[i]}");
                }
                
                // choose server config
                System.Console.Write("Select a configuration: ");
                var index = Console.ReadLine(); 
                configPath = configs[int.Parse(index)];               
            }

            // load server config
            var config = new ServerConfig(configPath);

            // run server
            var runner = new Runner(config);
            runner.Run();
            // runner.StartServer();
        }

        private static IConfiguration ReadAppSettings()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }

        private static string SaveEmptyConfig(string path)
        {    
            // save
            return new ServerConfig().Save(path);
        }

        private static string[] FindServersConfig()
        {
            var path = appConfig["AppSettings:server-missions-folder"];

            // get missions folders
            var missions = Directory.GetDirectories(path);

            // for each folder search for json config files
            var configs = missions.Select(p => {
                var f = new DirectoryInfo(p);
                var files = f.GetFiles($"{f.Name}{appConfig["AppSettings:server-config-suffix"]}")
                    .Select(file => file.FullName);
                return files;
            });

            // flatten the 2D array of config paths
            return configs.SelectMany(c => c).Distinct().ToArray();
        }
    }
}
