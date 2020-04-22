using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace GQChat.Other.Class
{
    public class Settings//Вот тут настройки
    {
        [JsonProperty("passworld")]//Пароль
        public string Passworld { get; set; }

        [JsonProperty("email")]//Почта
        public string Email { get; set; }

        [JsonProperty("nick")]//Ник
        public string Nick { get; set; }

        [JsonProperty("id")]//Id
        public long ID { get; set; }

        [JsonProperty("officlial")]//Аккаунт официальный
        public bool Officlial { get; set; }

        [JsonProperty("customAvatar")]//Есть аватарка для аккаунта?
        public bool CustomAvatar { get; set; }

        [JsonProperty("")]//При закрытие программы она будет работать в фоновом режиме
        public bool CloseProgram { get; set; }
    }

    static public class SettingsManager//Управление настройками
    {
        static public Settings Load()//Загрузка
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

        static public void Save(Settings settings)//Сохранение
        {
            File.WriteAllText(Data.SavePath, JsonConvert.SerializeObject(settings));
        }
    }
}
