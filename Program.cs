using System;
using System.IO;
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
            Console.WriteLine($"Arma3 Server Launcher [v{appConfig["AppInfo:app-version"]}]");
            
            // save empty config
            var path = Path.Combine(appConfig["AppSettings:server-folder"], appConfig["AppSettings:server-missions-folder"]);
            SaveEmptyConfig(path);

            // find servers config files
            // display
            // choose config file
            // load server config


            // run server
            // var runner = new Runner();
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
            // init
            var mission = new MissionParams();
            var hc = new HeadlessParams();
            var server = new ServerConfig(path){
                HCCount = 2,
                MissionParams = mission,
                HCParams = hc
            };

            // save
            return ServerConfig.SaveConfig(server);
        }
    }
}
