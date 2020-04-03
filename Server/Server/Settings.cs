using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Data;

namespace Server
{
    public class Settings
    {
        [JsonProperty("lastId")]
        public long LastId { get; set; }
    }

    public static class SettingsManager
    {
        public const string FilePath = "Settings.json";

        public static void Load()
        {
            if (!File.Exists(FilePath))
            {
                Data.Settings = new Settings() { LastId = 0 };
                Save();
            }
            else
            {
                Data.Settings = JsonConvert.DeserializeObject<Settings>(FilePath);
            }
        }

        public static void Save()
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(Data.Settings));
        }
    }
}
