namespace wchart.Core
open System.Security
open System
open System.IO
open System.Diagnostics


// Основная задача: определить установленную версию Microsoft Office
// на устройстве.
// ЕСЛИ офиса не существует - Приложение не должно работать дальше !
// ИНАЧЕ работа разрешена.
//  Office 97 - 7.0
//  Office 98 - 8.0
//  Office 2000 - 9.0
//  Office XP - 10.0
//  Office 2003 - 11.0
//  Office 2007 - 12.0
//  Office 2010 - 14.0 
//  Office 2013 - 15.0
//  Office 2016 - 16.0 (или 15.0)
//  Office 2019 - 16.0
module Office = 
    let nullVersion:Version = Version(0, 0, 0, 0)

    let initializePerferences() = 
        0

    /// <summary>
    /// Получает основную версию приложения 
    /// по запущенному процессу
    /// Если приложение не запущено, версия сравнивается 
    ///     Office.nullVersion:Version
    /// </summary>
    let getVersionByProcess (): Version =
        try
            let proc = 
                Process.GetProcessesByName("winword")
                    |> Array.item 0
                
            Version(proc.MainModule.FileVersionInfo.FileVersion)
        with
            | :? InvalidOperationException
            | :? IndexOutOfRangeException -> nullVersion

    /// <summary>
    /// Возвращает полное название продукта на 
    /// основе ведущей версии программного обеспечения
    /// </summary>
    /// <param name="maj">Основаная версия Microsoft Office</param>
    let getNameFromVersion (maj:int): String = 
        "Microsoft Office " + (
        match maj with
            | 0  -> "не установлен на этом компьютере"
            | 7  -> "97"
            | 8  -> "98"
            | 9  -> "2000"
            | 10 -> "eXPerience"
            | 11 -> "2003"
            | 12 -> "2007"
            | 14 -> "2010"
            | 15 -> "2013"
            | 16 -> "2016 или 2019"
            | _  -> "Неизвестной версии"
        )
    
    /// <summary>
    /// Создает файл конфигурации с 
    /// хранимой версией Office
    /// </summary>
    /// <param name="version">Параметр который будет записан в файл</param>
    let setConfiguration (version: string): unit = 
        File.WriteAllText("wchart.Config.InstalledOffice", version);


    /// <summary>
    /// Устанавливает список фильтров для поиска
    /// </summary>
    /// <param name="filter"></param>
    let setFilter (filter: string[]): unit =
        File.WriteAllLines("wchart.Filters", filter)

    /// <summary>
    /// Получает версию из файла конфигурации
    /// Если файла нет, возвращает 0
    /// </summary>
    let getConfiguration (): string =
        if File.Exists("wchart.Config.InstalledOffice") then
            File.ReadAllText("wchart.Config.InstalledOffice")
        else
            nullVersion.Major.ToString()
