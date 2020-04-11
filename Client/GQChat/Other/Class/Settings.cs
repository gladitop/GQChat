using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace GQChat.Other.Class
{
    public class Settings
    {
        [JsonProperty("passworld")]
        public string Passworld { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("nick")]
        public string Nick { get; set; }

        [JsonProperty("id")]
        public long ID { get; set; }

        [JsonProperty("officlial")]
        public bool Officlial { get; set; }

        [JsonProperty("customAvatar")]
        public bool CustomAvatar { get; set; }
    }

    static public class SettingsManager
    {
        static public Settings Load()
        {
            if (!File.Exists(Data.SavePath))
            {
                Settings settings = new Settings()
                {
                    CustomAvatar = false,
                    ID = 0,
                    Email = "lol",
                    Passworld = "123",
                    Nick = "Gladi",
                    Officlial = true
                };

                Save(settings);

                return settings;
            }
            else
            {
                Settings settings = JsonConvert.DeserializeObject<Settings>(Data.SavePath);
                return settings;
            }
        }

        static public void Save(Settings settings)
        {
            File.WriteAllText(Data.SavePath, JsonConvert.SerializeObject(settings));
        }
    }
}
