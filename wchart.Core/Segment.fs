namespace wchart.Core


/// <summary>
/// Представляет собой модель раздела Word документа 
/// для анализа и cбора статистики
/// </summary>
type Segment (title:string, pconut:int64) = 
    member this.Title:string = title
    member this.Count:int64 = 0;
