             .88888.   .88888.    a88888b. dP                  dP   
            d8'   `88 d8'   `8b  d8'   `88 88                  88   
            88        88     88  88        88d888b. .d8888b. d8888P 
            88   YP88 88  db 88  88        88'  `88 88'  `88   88   
            Y8.   .88 Y8.  Y88P  Y8.   .88 88    88 88.  .88   88   
             `88888'   `8888PY8b  Y88888P' dP    dP `88888P8   dP

`GQChat` ver 1.0

# Ссылки

* [Описание](https://github.com/damiralmaev/GQChat#%D0%BE%D0%BF%D0%B8%D1%81%D0%B0%D0%BD%D0%B8%D0%B5)
* [Использовано](https://github.com/damiralmaev/GQChat#%D0%B8%D1%81%D0%BF%D0%BE%D0%BB%D1%8C%D0%B7%D0%BE%D0%B2%D0%B0%D0%BD%D0%BE)
    * [Технологии и программы](https://github.com/damiralmaev/GQChat#технологии-и-программы)
    * [NuGet пакеты](https://github.com/damiralmaev/GQChat#nuget-%D0%BF%D0%B0%D0%BA%D0%B5%D1%82%D1%8B)
* [Папки](https://github.com/damiralmaev/GQChat#%D0%BF%D0%B0%D0%BF%D0%BA%D0%B8)
* [Лицензия](https://github.com/damiralmaev/GQChat#%D0%BB%D0%B8%D1%86%D0%B5%D0%BD%D0%B7%D0%B8%D1%8F)
* [Команды сервера](https://github.com/damiralmaev/GQChat#%D0%BA%D0%BE%D0%BC%D0%B0%D0%BD%D0%B4%D1%8B-%D1%81%D0%B5%D1%80%D0%B2%D0%B5%D1%80%D0%B0)
* [Авторы](https://github.com/damiralmaev/GQChat#%D0%B0%D0%B2%D1%82%D0%BE%D1%80%D1%8B)

# Описание

Это чат для кванториума

# Использовано

## Технологии и программы

* Visual Studio 2019
* .Net Framework 4.8
* WPF
* Unity ***(Меня заставил*** [Qliook](https://github.com/Qliook)***)***

## NuGet пакеты

| Имя пакета                        | Описание	    			     | Знак  |
|-----------------------------------|--------------------------------|-------|
| `SharpCompress`           | Это библиотека для RAR| [![sharpcompress](https://img.shields.io/nuget/vpre/sharpcompress.svg)](https://www.nuget.org/packages/sharpcompress) |
| `Newtonsoft.Json`         | Сохранения! | [![Newtonsoft.Json](https://img.shields.io/nuget/vpre/Newtonsoft.Json.svg)](https://www.nuget.org/packages/Newtonsoft.Json) |
| `ConsoleTables`         | Таблицы! | [![ConsoleTables](https://img.shields.io/nuget/vpre/ConsoleTables.svg)](https://www.nuget.org/packages/ConsoleTables) |
| `Emoji.Wpf`         | Смайлики! | [![Emoji.Wpf](https://img.shields.io/nuget/vpre/Emoji.Wpf.svg)](https://www.nuget.org/packages/Emoji.Wpf) |
| `LiveCharts.Wpf`         | Графики | [![LiveCharts.Wpf](https://img.shields.io/nuget/vpre/LiveCharts.Wpf.svg)](https://www.nuget.org/packages/LiveCharts.Wpf/) |

# Папки

| Название папки | Описание	    		   |
|----------------|-------------------------|
| `Client`       | Клиент для чата         |
| `Server`       | Сервер для чата         |
| `ClientUnity`  | Клиент для чата (Unity) |
| `Other`        | Разное                  |
| `.github`      | Настройки для github    |
| `Website`      | Сайт программы          |

# Лицензия

***GPL-3.0***
```
Permissions of this strong copyleft license are conditioned
on making available complete source code of licensed works
and modifications, which include larger works using a 
licensed work, under the same license. Copyright and license 
notices must be preserved. Contributors provide an express grant of 
patent rights.
```

# Команды сервера

| Название       | Описание	    		           | Пример                                |
|----------------|---------------------------------|---------------------------------------|
| `%REG`         | Служит для регистрации          | `%REG:email:password:nick:typeAvatar` |
| `%LOG`         | Служит для входа                | `%LOG:email:password`                 |
| `%EXI`         | Служит для отключения           | `%EXI`                                |
| `%UPM`         | Получить сообщении              | `%UPM:idChat:countMess`               |
| `%NCT`         | Создания нового чата            | `%NCT:idUser`                         |
| `%MSE`         | Отправить сообщение в от. чат   | `%MSE:idChat:textMess`                |
| `%MES`         | Отправить сообщение в об. чат   | `%MES:textMess`                       |
| `%INF`         | Получить инфо о аккаунте        | `%INF:idUser`                         |
| `%DEL`         | Удалить аккаунт                 | `%DEL`                                |
| `%SЕM`         | Отправить файл в об. чат        | `%SEM:textMess:fileName`              |
| `%SMM`         | Отправить файл в от. чат        | `%SEM:textMess:fileName:idChat`       |
| `%UUS`         | Обновление клиентов (онлайн)    | `%UUS`                                |

# Авторы

1. [Gladi](https://github.com/damiralmaev) `Самый главный (коммунист)`
2. [Qliook](https://github.com/Qliook) `Антикоммунист`
3. [sEKRETNY](https://github.com/sEKRETNY) `Анимешник`
