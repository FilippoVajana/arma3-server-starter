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
        private const string ServerFolder = @"D:\Games\Arma3\Game";
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
                .GetFiles(Program.appConfig["AppSettings:server-folder"],
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
                Task.Delay(30000).Wait();
                System.Console.WriteLine($"Starting HC{i}");

                var hcP = new Process();
                hcP.StartInfo.FileName = $"{_serverExe}";
                hcP.StartInfo.Arguments = $"{args}";
                hcP.Start();
            }
        }

        /// <summary>
        /// DEPRECATED.
        /// Runs the server and the headless clients from hard coded parameters.
        /// </summary>
        public void StartServer()
        {            
            // mission params

            // vanilla mission
            //var serverParams = @"-port=2302 ""-config=D:\Games\Arma3\Game\TADST\KP_964_Altis\TADST_config.cfg"" ""-cfg=D:\Games\Arma3\Game\TADST\KP_964_Altis\TADST_basic.cfg"" ""-profiles=D:\Games\Arma3\Game\TADST\KP_964_Altis"" -name=KP_964_Altis -filePatching";
            
            // RHS mission
            //var serverParams = @"-port=2302 ""-config=D:\Games\Arma3\Game\TADST\KP_964_RHS_Altis\TADST_config.cfg"" ""-cfg=D:\Games\Arma3\Game\TADST\KP_964_RHS_Altis\TADST_basic.cfg"" ""-profiles=D:\Games\Arma3\Game\TADST\KP_964_RHS_Altis"" -name=KP_964_RHS_Altis -filePatching ""-mod=D:\Games\Arma3\Game\mods\@RHSUSAF;D:\Games\Arma3\Game\mods\@RHSAFRE;D:\Games\Arma3\Game\mods\@Project OPFOR;D:\Games\Arma3\Game\mods\@CBA_A3""";

            // Lythium RHS
            var serverParams = @"-port=2302 ""-config=D:\Games\Arma3\Game\TADST\KP_964_RHS_Lythium\TADST_config.cfg"" ""-cfg=D:\Games\Arma3\Game\TADST\KP_964_RHS_Lythium\TADST_basic.cfg"" ""-profiles=D:\Games\Arma3\Game\TADST\KP_964_RHS_Lythium"" -name=KP_964_RHS_Lythium -filePatching ""-mod=D:\Games\Arma3\Game\mods\@Advanced AI Command;D:\Games\Arma3\Game\mods\@LYTHIUM;D:\Games\Arma3\Game\mods\@Jbad;D:\Games\Arma3\Game\mods\@RHSUSAF;D:\Games\Arma3\Game\mods\@RHSAFRE;D:\Games\Arma3\Game\mods\@Project OPFOR;D:\Games\Arma3\Game\mods\@CBA_A3""";

            // start server
            Console.WriteLine("Starting Arma3 Server");

            var serverP = new Process();
            serverP.StartInfo.FileName = $"{_serverExe}";
            serverP.StartInfo.Arguments = $"{serverParams}";
            serverP.Start();
            Console.WriteLine("Server started");                       

            // start headless clients            
            int hc_num = 2;
            var hcParams = @"-client -connect=localhost -port=2302  -nosound -password=sig4freedom -profiles=""D:\Games\Arma3\Profiles"" -mod=""D:\Games\Arma3\Game\mods\@Advanced AI Command;D:\Games\Arma3\Game\mods\@LYTHIUM;D:\Games\Arma3\Game\mods\@Jbad;D:\Games\Arma3\Game\mods\@RHSUSAF;D:\Games\Arma3\Game\mods\@RHSAFRE;D:\Games\Arma3\Game\mods\@Project OPFOR;D:\Games\Arma3\Game\mods\@CBA_A3""";
            
            for (int i = 0; i < hc_num; i++)
            {                
                Task.Delay(60000).Wait();
                Console.WriteLine($"Starting Arma3 HC#{i}");  

                var hcP = new Process();
                hcP.StartInfo.FileName = $"{_serverExe}";
                hcP.StartInfo.Arguments = $"{hcParams}";
                hcP.Start();
            }           
        }
    }
}
