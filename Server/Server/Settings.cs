using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Linq;

namespace Server
{
    public class Settings
    {
        [JsonProperty("generalChatsMess")]
        public List<Data.GeneralChatMess> GeneralChatsMess { get; set; }//Там весь общий чат
        
        [JsonProperty("clientConnectOfflines")]
        public List<Data.ClientConnectOffline> ClientConnectOfflines { get; set; }//Профиль всех клиентов
    }

    static public class SettingsManager
    {
        static void Load()//Это будет хранится в Data
        {
            if (!File.Exists("Settings.json"))
            {
                var set = new Settings()
                {
                    ClientConnectOfflines = new List<Data.ClientConnectOffline>(),
                    GeneralChatsMess = new List<Data.GeneralChatMess>()
                };

                Data.Settings = set;
                Save();
            }
            else
            {
                Data.Settings = JsonConvert.DeserializeObject<Settings>("Settings.json");
            }
        }

        static void Save()
        {
            var set = new Settings()
            {
                ClientConnectOfflines = ((Settings)Data.Settings).ClientConnectOfflines,
                GeneralChatsMess = ((Settings)Data.Settings).GeneralChatsMess
            };

            File.WriteAllText("Settings.json", JsonConvert.SerializeObject(set));
        }
    }
}
