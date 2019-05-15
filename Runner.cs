using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace arma3_server_starter
{
    public class Runner
    {
        private string ServerFolder = @"D:\Games\Arma3\Game";

        public Runner(string serverFolder=null)
        {
            if (serverFolder != null)
            {
                ServerFolder = serverFolder;
            }
        }

        private void RunInNewTerminal(string exec, string parameters)
        {
            var terminalExec = @"start cmd /k";
            var terminalParams = $"{exec} {parameters}";
            var serverProc = new Process();
            serverProc.StartInfo.FileName = terminalExec;
            serverProc.StartInfo.Arguments = terminalParams;
            serverProc.Start();
        }
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

            var serverProc = new Process();
            serverProc.StartInfo.FileName = $"{exec}";
            serverProc.StartInfo.Arguments = $"{serverParams}";
            serverProc.StartInfo.CreateNoWindow = false;
            serverProc.StartInfo.UseShellExecute = true;
            serverProc.Start();            

            // start headless clients

            int hc_num = 2;
            var hcParams = @"-client -connect=localhost -port=2302  -nosound -password=sig4freedom -profiles=""D:\Games\Arma3\Profiles"" -mod=""D:\Games\Arma3\Game\mods\@Advanced AI Command;D:\Games\Arma3\Game\mods\@LYTHIUM;D:\Games\Arma3\Game\mods\@Jbad;D:\Games\Arma3\Game\mods\@RHSUSAF;D:\Games\Arma3\Game\mods\@RHSAFRE;D:\Games\Arma3\Game\mods\@Project OPFOR;D:\Games\Arma3\Game\mods\@CBA_A3""";
            for (int i = 0; i < hc_num; i++)
            {                
                Task.Delay(60000).Wait();
                Console.WriteLine($"Starting Arma3 HC{i}");  

                var hcProc = new Process();
                hcProc.StartInfo.FileName = $"{exec}";
                hcProc.StartInfo.Arguments = $"{hcParams}";
                hcProc.StartInfo.CreateNoWindow = false;
                hcProc.StartInfo.UseShellExecute = false;
                hcProc.Start();
            }           
        }
    }
}
