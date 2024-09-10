' Предполагаем, что программа была добавлена с именем "MyProgram"
Const strProgramName = "MyProgram"

' Удаление ключа
Set objShell = CreateObject("WScript.Shell")
objShell.RegDelete "HKCR\*\shell\OpenWith" & strProgramName

' Удаление из "Программы по умолчанию"
objShell.Run "%SystemRoot%\system32\control.exe /name Microsoft.DefaultPrograms /page pageFileAssociations", 1, True
