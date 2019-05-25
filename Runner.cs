using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using arma3_server_starter.Config;

namespace arma3_server_starter
{   
    /// <summary>
    /// Handles the run of server and headless clients.
    /// </summary>
    public class Runner
    {
        private readonly ServerConfig _config;
        private readonly string _serverExe;

        /// <summary>
        /// Initializes the runner based on the config file.
        /// </summary>
        /// <param name="config"></param>
        public Runner(ServerConfig config)
        {
            _config = config;
            // find server exe
            _serverExe = Directory
                .GetFiles(Program.appConfig["AppSettings:ServerRoot"],
                                            @"*x64.exe", 
                                            SearchOption.TopDirectoryOnly)
                .First();
        }

        public void Run()
        {
            // start server
            StartServer(_config.Builder.BuildMissionArgs());

            // start hc
            StartHeadlessClients(_config.Builder.BuildHCArgs());
        }

        private void StartServer(string args)
        {
            System.Console.WriteLine($"Starting server {_config.MParams.Name}");
            
            // init proces
            var serverP = new Process();
            serverP.StartInfo.FileName = $"{_serverExe}";
            serverP.StartInfo.Arguments = $"{args}";
            serverP.Start();                    

        }

        private void StartHeadlessClients(string args)
        {
            for (int i = 0; i < _config.HCParams.Count; i++)
            {
                Task.Delay(45000).Wait();
                System.Console.WriteLine($"Starting HC{i}");
                
                // init proces
                var hcP = new Process();
                hcP.StartInfo.FileName = $"{_serverExe}";
                hcP.StartInfo.Arguments = $"{args}";
                hcP.Start();
            }
        }
    }
}
