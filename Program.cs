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
            // init step
            Setup();

            // welcome message
            Console.WriteLine($"Arma3 Server Launcher v.{appConfig["AppInfo:Version"]}\n");
            
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
                System.Console.WriteLine($"Found {configs.Length} configurations:");
                for (int i = 0; i < configs.Length; i++)
                {
                    System.Console.WriteLine($"  {i}) \t{configs[i]}");
                }
                
                // choose server config
                System.Console.WriteLine();
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

        private static void Setup()
        {
            // get app settings
            var c = new AppConfig();
            appConfig = c.Build();
        }

        private static string[] FindServersConfig()
        {
            var path = appConfig["AppSettings:MissionsDir"];

            // get missions folders
            var missions = Directory.GetDirectories(path);

            // for each folder search for json config files
            var configs = missions.Select(p => {
                var f = new DirectoryInfo(p);
                var files = f.GetFiles($"{f.Name}_server.json")
                    .Select(file => file.FullName);
                return files;
            });

            // flatten the 2D array of config paths
            return configs.SelectMany(c => c).Distinct().ToArray();
        }
    }
}
