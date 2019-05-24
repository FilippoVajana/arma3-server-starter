
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace arma3_server_starter.Config
{
    /// <summary>
    /// Server instance configuration parameters.
    /// </summary>
    public class ServerConfig
    {
        private readonly string _path = null;

        [JsonIgnore]
        public ConfigArgsBuilder Builder 
        {
            get
            {
                return new ConfigArgsBuilder(_path, this);
            } 
            private set
            {
                Builder = value;
            }
        }
        public MissionParams MParams { get; set; }
        public HeadlessParams HCParams { get; set; }

        public ServerConfig()
        { 
            // set default path to missions folder
            _path = Program.appConfig["AppSettings:server-missions-folder"];            
            MParams = new MissionParams();
            HCParams = new HeadlessParams();
        }
        public ServerConfig(string configPath)
        {
            _path = configPath;

            // load config
            var config = Load(_path);
            this.MParams = config.MParams;
            this.HCParams = config.HCParams;
        }

        private ServerConfig Load(string configPath)
        {
            // open file
            var json = File.ReadAllText(configPath);

            // deserialize
            var obj = JsonConvert.DeserializeObject<ServerConfig>(json);

            return obj; 
        }

        public string Save(string savePath = null)
        {
            // build json
            string json = JsonConvert.SerializeObject(this);

            // create file path (./TADST/mission_name/mission_name_server.json)
            var name = $"{MParams.Name}{Program.appConfig["AppSettings:server-config-suffix"]}";
            string path;
            if (savePath == null)
                path = Path.Combine(_path, name);
            else
                path = Path.Combine(savePath, name);
                       

            // create file
            File.WriteAllText(path, json);

            System.Console.WriteLine($"Configuration saved at {path}");
            return path;
        }

        public class ConfigArgsBuilder
        {
            private readonly string _missionPath;
            private readonly string _modsPath;
            private readonly ServerConfig _config;
            internal ConfigArgsBuilder(string basePath, ServerConfig config)
            {
                _missionPath = basePath;
                _modsPath = Program.appConfig["AppSettings:ModsDir"];
                _config = config;
            }

            private string AddBaseFolder(string p)
            {
                return Path.Combine(_missionPath, p);
            }

            private string FlattenMods(string[] mods)
            {
                var m = "";
                foreach (var mod in mods)
                {
                    m += Path.Combine(_modsPath, mod) + ";";
                }
                m = m.Substring(0, m.Length - 1);

                return m;
            }

            internal string BuildMissionArgs()
            {
                var p = _config.MParams;
                var strBase = "";
                strBase += $"-port={p.Port} ";
                strBase += $"\"-config={AddBaseFolder(p.Config)}\" ";
                strBase += $"\"-cfg={AddBaseFolder(p.Cfg)}\" ";
                strBase += $"\"-profiles={AddBaseFolder(p.Profiles)}\" ";                
                strBase += $"-name={p.Name} ";
                if (p.FilePatching)
                    strBase += "-filePatching ";
                strBase += $"\"-mod={FlattenMods(p.Mod)}\"";
                return strBase;
            }

            internal string BuildHCArgs()
            {
                var p = _config.HCParams;
                var strBase = "";
                if (p.Client)
                    strBase += "-client ";
                strBase += $"-connect={p.Connect} ";
                strBase += $"-port={p.Port}  ";
                if (p.NoSound)
                    strBase += "-nosound ";
                strBase += $"-password={p.Password} ";
                strBase += $"\"-profiles={p.Profiles}\" ";
                strBase += $"\"-mod={FlattenMods(p.Mod)}\"";

                return strBase;
            }
        }
    }
    
    public class MissionParams
    {
        public MissionParams()
        {
            Config = "";
            Cfg = "";
            Profiles = "";
            Name = "";
            Mod = new string[]{};
        }
        public int Port { get; set; }
        public string Config { get; set; }
        public string Cfg { get; set; }
        public string Profiles { get; set; }
        public string Name { get; set; }
        public string[] Mod { get; set; }
        public bool FilePatching { get; set; }        
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
        public int Count {get; set;}
    }        

}