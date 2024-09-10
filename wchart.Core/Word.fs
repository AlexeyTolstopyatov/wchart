namespace wchart.Core
open System
open Microsoft.Office.Interop.Word
open System.Reflection

//
// Основная задача - чтение данных из документа, 
// ЕСЛИ документ не зашифрован (не имеет пароля)
// запись данных в коллекцию заголовоков, 
// рассчет статистики для передачи данных.
// Для хранения данных использовать: wchart.Core.Segment
//      wchart.Core.Segment(title, paragraphs count)
//
module Word =
    let getWordsCount (data:Paragraph) = 
        data.Range.Text.Split(' ').LongLength

    let getTitles (path: string) =  
        let app = 
            Activator.CreateInstance(Type.GetTypeFromProgID("Word.Application")) :?> Application
        
        let doc:Document = app.Documents.Open(ref path)

        //
        // Коллекция стилей в документе
        //
        let styles:WdBuiltinStyle[] = [|
            WdBuiltinStyle.wdStyleHeading1 
            WdBuiltinStyle.wdStyleHeading2
            WdBuiltinStyle.wdStyleHeading3
            |]

        // 
        // Получить абзацы и их стили
        //
        let mutable regions = ResizeArray<Segment>()
        let mutable p_index:int = 0;
        let mutable p_title:string = "Empty paragraph"

        for paragraph in doc.Paragraphs do
            // Если стиль абзаца это хотябы один из этих стилей

            if paragraph.Range.Style = styles[0] || 
                paragraph.Range.Style = styles[1] ||
                paragraph.Range.Style = styles[2] then
                // Заголовок найден, -> новый регион
                p_title <- paragraph.Range.Text
                p_index <- 1 + p_index
            
            regions.Add(Segment(p_title, getWordsCount(doc.Paragraphs[p_index])))

        if p_index = 0 then
            regions.Add(Segment(p_title, p_index))
            
        doc.Close()
        app.Quit()
        regions // return value

    
