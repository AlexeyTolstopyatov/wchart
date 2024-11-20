' (С) Толстопятов Алексей А
'       ContentProcessor
'  Создает файл с таблицей заголовков и количеством слов
' Используется в wchart.Core.Content для создания модели документа
' 
' TODO: Заменить это на двоичный файл
Option Explicit

Dim objWord, objDoc, objSection, i, strSectionHeading, lngWordCount, objStyle, strStyleName
Dim fso, file, strOutput, arrBuiltInStyles, paragraph

Set objWord = CreateObject("Word.Application")
objWord.Visible = True

Set objDoc = objWord.Documents.Open("D:\Projects\cs\wchart\wchart.Core\s.docx") ' Замените на путь к вашему файлу
Set fso = CreateObject("Scripting.FileSystemObject")
Set file = fso.CreateTextFile("result.csv", True)

file.WriteLine "Style;Count"

For Each paragraph In objDoc.Paragraphs
    ' Обработка ошибки если нет ни одного абзаца
    On Error Resume Next 

    If Err.Number <> 0 Then
        ' Заголовков нет
        Err.Clear
    Else
        lngWordCount = paragraph.Range.Words.Count
        strOutput = paragraph.Style.NameLocal & ";" & (lngWordCount - 1) & ";" & paragraph.Range.Text
        file.WriteLine strOutput
    End If

    On Error GoTo 0
Next

file.Close
objDoc.Close False
objWord.Quit

Set file = Nothing
Set fso = Nothing
Set objSection = Nothing
Set objDoc = Nothing
Set objWord = Nothing
Set objStyle = Nothing
