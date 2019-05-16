
using System;
using System.IO;
using Newtonsoft.Json;

namespace arma3_server_starter
{
    /// <summary>
    /// Server specific configuration parameters.
    /// </summary>
    public class ServerConfig
    {
        private readonly string _path = null;
        public ServerConfig(){ }
        public ServerConfig(string missionPath)
        {
            _path = missionPath;
        }

        // parameters
        public int HCCount { get; set; }
        public MissionParams MissionParams { get; set; }
        public HeadlessParams HCParams { get; set; }

        private ServerConfig Load(string configPath)
        {
            throw new NotImplementedException("Config load not implemented yet.");
        }

        public static string SaveConfig(ServerConfig instance)
        {
            // build json
            string json = JsonConvert.SerializeObject(instance);

            // create file path 
            // (./TADST/mission_name/mission_name_server.json)

            var name = $"{instance.MissionParams.Name}_server.json"; 
            var path = Path.Combine(instance._path, name);           

            // create file
            File.WriteAllText(path, json);

            System.Console.WriteLine($"Configuration saved at {path}");
            return path;
        }        
    }

    public class MissionParams
    {
        public int Port { get; set; }
        public string Config { get; set; }
        public string Cfg { get; set; }
        public string Profiles { get; set; }
        public string Name { get; set; }
        public string[] Mod { get; set; }
        public bool FilePatching { get; set; }

        public override string ToString()
        {
            return string.Empty;
        }
    }

    public class HeadlessParams
    {
        public bool Client { get; set; }
        public string Connect { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public string Profiles { get; set; }
        public string[] Mod { get; set; }
        public bool NoSound { get; set; }

        public override string ToString()
        {
            return string.Empty;
        }
    }

}