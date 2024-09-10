open wchart.Core
open System
open System.Diagnostics
open Microsoft.Win32;

//
// Добавить в список "Открыть с помощью"
// HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts\*.doc\OpenWithList
// REG_SZ name -> "Название приложения (ветки реестра)"
// 
// Чтобы найти приложение в недрах реестра
// HKEY_CLASSES_ROOT\Applications\мое название\shell\open\command
// REG_SZ name -> "путь до приложения", "%1"
// Можно попробовать еще
// REG_EXPAND_SZ name "path" "%1" но в путь ...\shell\edit\command
//

let createHive ():int =
    printfn "-----------------------------------"
    printfn " Добавить ассоциацию Word's Chart"
    printfn "-----------------------------------"

    printfn "\nМастер установки ключей установит значение программы
внутри реестра операционной системы. Это поможет вам в будущем открывать
документы с помощью WChart и сохранять статистику. 
Это будет возможно сделать через меню \"Открыть с помощью...\".
Удаление ключа реестра из мастера настройки так же возможно.

ВНИМАНИЕ: Действия вступят в силу, если мастер открыт От имени \"Администратора\".
"
    printfn "Вернуться назад - :main"
    printfn "Установить ассоциацию - [Enter]"

    match Console.ReadLine () with
    | ":main" -> -1
    | ":exit" -> exit -1
    | _ -> 
    ()
    
    
    let key = 
        Registry.ClassesRoot
                .CreateSubKey(@"Applications\wchart.exe\shell\open\command")
    
    key.SetValue (null, 
                AppDomain.CurrentDomain.BaseDirectory + 
                "\\wchart.exe" + "\"%1\"")
    printfn "Ключ %s установлен" key.Name
    key.Close ()
    let _ = Process.Start("control", "/name Microsoft.DefaultPrograms /page pageFileAssociations")
    0

let destroyHive ():int = 
    printfn "-----------------------------------"
    printfn " Удалить ассоциацию Word's Chart"
    printfn "-----------------------------------"

    printfn "Действия вступят в силу, если запустить мастер
От имени \"Администратора\". Работа с реестром требует пользователя
с привелегиями."
    
    let key = Registry.ClassesRoot.OpenSubKey("Applications\\wchart.exe")
    if key = null then
       printfn "В реестре Windows нет ветки Word's Chart.";
       -1
    else
    ()
    
    Registry.ClassesRoot.DeleteSubKeyTree("Applications\\wchart.exe")
    key.Close ()
    0

let catchProcess ():int = 
    printfn "------------------------------"
    printfn " Найти Microsoft Office Word"
    printfn "------------------------------"

    printfn "\nДля работы функции, откройте Microsoft Word"
    printfn "Установщик проверит процесс лишь один раз."
    printfn "И запишет сведения в файл конфигурации приложения."
    printfn "Word Chart cannot works without installed Office."

    printfn "Когда откроете MS Word ЖМИТЕ 'Enter'..."

    let result = Console.ReadLine() // how to escape this.
    let version = 
        Office.getVersionByProcess().Major
    // Program+Pipe #1 input at line 45@45

    printfn "%d" version

    if version = Office.nullVersion.Major then
        printfn "Отмена операции."
        exit 0
    else
        printfn "Ты сделал это!"

    Office.setConfiguration(version.ToString())

    version
    


let main () = 
    printfn "-------------------------------------"
    printfn " Word's Chart Установка и настройка  "
    printfn "-------------------------------------"
    printfn "\nИспользуй это до запуска wchart.exe."
    printfn "Это поможет установить флаги для главного приложения.
wchart.exe использует COM объект, пренадлежащий пакету Office. 
Без установленного офиса продолжение работы невозможно.
    "
    printfn "Чтобы продолжить работу введите определение..."
    printfn ":proc - Зарегистрировать версию MS Office для Word's Chart"
    printfn ":regc - Зарегистрировать ассоциацию документов с Word's Chart"
    printfn ":regd - Удалить запись ассоциации из реестра"
    
    let mutable result = String.Empty

    while true do
        result <- Console.ReadLine ()
        match result with
        | ":proc" -> printfn "Версия Office: %d" (int(catchProcess())) // catch process
        | ":regc" -> printfn "Операция завершена: %d" (int(createHive()))
        | ":regd" -> printfn "Операция завершена: %d" (int(destroyHive()))
        | ":exit" -> exit 0
        | _  -> printfn "Определение не найдено"

    let _ = Console.ReadLine()
    ()

main ()
    |> ignore