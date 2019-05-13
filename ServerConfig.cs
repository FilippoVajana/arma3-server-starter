
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
        public ServerConfig()
        {            
        }

        // parameters
        public string ServerFolder { get; set; }
        public string MissionsFolder { get; set; }
        public string ModsFolder { get; set; }
        public int HCCount { get; set; }
        public ServerParams SParams { get; set; }
        public HCParams HParams { get; set; }

        private ServerConfig Load(string configPath)
        {
            throw new NotImplementedException("Config load not implemented yet.");
        }

        public void SaveConfig()
        {
            // build json
            string json = JsonConvert.SerializeObject(this);

            // create filename
            var name = "server.json"; 
            var path = Path.Combine(MissionsFolder, name);           

            // create file
            File.WriteAllText(path, json);
        }
    }

    public class ServerParams
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

    public class HCParams
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