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
        public long LastId { get; set; }//Последний id

        [JsonProperty("lastIdMessagesMain")]
        public long LastIdMessMain { get; set; }//Последний id сообщение (в общем чате)

        [JsonProperty("messageInfoChats")]
        public List<Data.IMessageInfoChat> MessageInfoChats { get; set; }//Информация для отдельных чатов
    }

    public static class SettingsManager
    {
        public const string FilePath = "Settings.json";

        public static void Load()
        {
            if (!File.Exists(FilePath))
            {
                Data.Settings = new Settings()
                {
                    LastId = 0,
                    LastIdMessMain = 0,
                    MessageInfoChats = new List<Data.IMessageInfoChat>()
                };

                Save();
            }
            else
            {
                var settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(FilePath));
                Data.Settings = settings;
            }
        }

        public static void Save()
        {
            var settings = (Settings)Data.Settings;

            var set = new Settings()
            {
                LastId = settings.LastId,
                LastIdMessMain = settings.LastIdMessMain,
                MessageInfoChats = settings.MessageInfoChats
            };

            File.WriteAllText(FilePath, JsonConvert.SerializeObject(set));
        }
    }
}
