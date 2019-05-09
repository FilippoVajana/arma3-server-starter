using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace arma3_server_starter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // init runner
            var runner = new Runner();
            runner.StartServer();
        }
    }

    class Runner
    {
        private string ServerFolder = @"D:\Games\Arma3\Game";

        public Runner(string serverFolder=null)
        {
            if (serverFolder != null)
            {
                ServerFolder = serverFolder;
            }
        }

        public void StartServer()
        {
            // find server exec
            var exec = Directory.GetFiles(ServerFolder, @"*x64.exe", SearchOption.TopDirectoryOnly)
                                .First();      

            // start server
            var serverProc = new Process();
            serverProc.StartInfo.FileName = $"{exec}";
            serverProc.StartInfo.CreateNoWindow = false;
            serverProc.StartInfo.UseShellExecute = false;

            serverProc.Start();            
        }
    }
}
