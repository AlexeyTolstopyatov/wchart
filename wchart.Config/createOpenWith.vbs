' Надо узнать где будет установлена программа
Const app = "C:\Program Files\MyProgram\MyProgram.exe"

' Добавить ключ в реестр. (Требуются права Администратора)
Set wso = CreateObject("WScript.Shell")
wso.RegWrite "HKCR\myfiletype\shell\open\command", strProgramPath & " ""%1""", "REG_SZ"

' Добавляем программу в меню "Открыть с помощью"
wso.Run "%SystemRoot%\system32\control.exe /name Microsoft.DefaultPrograms /page pageFileAssociations", 1, True
