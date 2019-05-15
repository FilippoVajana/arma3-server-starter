using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace arma3_server_starter
{   
    /// <summary>
    /// Handles the run of server and headless clients.
    /// </summary>
    public class Runner
    {
        private string ServerFolder = @"D:\Games\Arma3\Game";

        public Runner()
        {

        }

        /// <summary>
        /// Initializes the runner based on the config file.
        /// </summary>
        /// <param name="config"></param>
        public Runner(ServerConfig config)
        {
            throw new NotImplementedException();
        }


        private Process StartServer(ServerConfig config)
        {
            throw new NotImplementedException();
        }

        private Process StartHeadless(ServerConfig config)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// DEPRECATED.
        /// Runs the server and the headless clients from hard coded parameters.
        /// </summary>
        public void StartServer()
        {
            // find server exec
            var exec = Directory.GetFiles(ServerFolder, @"*x64.exe", SearchOption.TopDirectoryOnly)
                                .First();      

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
            serverP.StartInfo.FileName = $"{exec}";
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
                hcP.StartInfo.FileName = $"{exec}";
                hcP.StartInfo.Arguments = $"{hcParams}";
                hcP.Start();
            }           
        }
    }
}
