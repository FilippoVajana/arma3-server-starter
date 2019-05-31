using System;
using System.Collections;
using System.Collections.Generic;
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
            var processes = new List<Process>();

            // start server
            var sp = StartServer(_config.Builder.BuildMissionArgs());
            
            // start hc
            var hc = StartHeadlessClients(_config.Builder.BuildHCArgs());

            // set resources
            processes.Append(sp);
            processes.AddRange(hc);
            SetProcessCpuAffinity(processes.ToArray(), 2);
        }

        private Process StartServer(string args)
        {
            System.Console.WriteLine($"Starting server {_config.MParams.Name}");
            
            // init proces
            var p = new Process();
            p.StartInfo.FileName = $"{_serverExe}";
            p.StartInfo.Arguments = $"{args}";
            p.Start();                    

            return p;
        }

        private Process[] StartHeadlessClients(string args)
        {
            var hcCount = _config.HCParams.Count;
            var procs = new Process[hcCount];

            for (int i = 0; i < hcCount; i++)
            {
                Task.Delay(45000).Wait();
                System.Console.WriteLine($"Starting HC{i}");
                
                // init proces
                var p = new Process();
                p.StartInfo.FileName = $"{_serverExe}";
                p.StartInfo.Arguments = $"{args}";
                p.Start();
                procs[i] = p;
            }

            return procs;
        }

        private IntPtr GetAffinityMask(int[] id)
        {            
            BitArray mask = new BitArray(16);
            foreach (var i in id)
            {
                mask[i] = true;
            }
            var result = new int[1];
            mask.CopyTo(result, 0);            
        
            return new IntPtr(result[0]);
        }

        private void SetProcessCpuAffinity(Process[] processes, int reservedCores = 0)
        {
            // get cpu cores
            var availableCores = Environment.ProcessorCount - reservedCores;
            if (availableCores <= 0)
                throw new Exception("Invalid number of cpu cores");

            int coresPerInstance = 2;
            var cores = new Queue<int>(Enumerable.Range(0, availableCores).ToList());

            for (int p = 0; p < processes.Length; p++)
            {
                var assignedCores = new int[coresPerInstance];

                for (int j = 0; j < coresPerInstance; j++)
                {
                    var id = cores.Dequeue();
                    assignedCores[j] = id;
                    cores.Enqueue(id);
                }

                var affinity = GetAffinityMask(assignedCores);
                processes[p].ProcessorAffinity = affinity;
            }

        }
        
    }
}
