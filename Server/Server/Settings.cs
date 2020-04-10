using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

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
        const string FilePath = "Settings.json";//Где файл сохранения?

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
                Settings settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(FilePath));
                Data.Settings = settings;
            }
        }

        public static void Save()
        {
            Settings settings = (Settings)Data.Settings;

            Settings set = new Settings()
            {
                LastId = settings.LastId,
                LastIdMessMain = settings.LastIdMessMain,
                MessageInfoChats = settings.MessageInfoChats
            };

            File.WriteAllText(FilePath, JsonConvert.SerializeObject(set));
        }
    }
}
