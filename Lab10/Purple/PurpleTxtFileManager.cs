namespace Lab10.Purple;

public class PurpleTxtFileManager<T>:PurpleFileManager<T> where T:Lab9.Purple.Purple
{
    public PurpleTxtFileManager(string name):base(name){}
    public PurpleTxtFileManager(string name, string folderpath, string filename, string fileextension = "txt"):base(name,folderpath,filename,fileextension){}

    public override void EditFile(string text)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text)) return;
        if (!File.Exists(FullPath)) return;
        T obj = Deserialize();
        obj.ChangeText(text);
        Serialize(obj);
    }
    public override void Serialize(T obj)
    {
        if (obj == null) return;

        var dict = new Dictionary<string, string>
        {
            ["Input"] = obj.Input,
            ["Type"] = obj.GetType().Name
        };

        ChangeFileExtension("txt");

        switch (dict["Type"])
        {
            case "Task1":
            case "Task2":
                dict["Output"] = obj.ToString();
                base.EditFile(FormatSimpleOutput(dict));
                break;

            case "Task3":
                var task3 = obj as Lab9.Purple.Task3;
                dict["Output"] = task3.ToString();
                base.EditFile(FormatComplexOutput(dict, task3.Codes));
                break;

            case "Task4":
                var task4 = obj as Lab9.Purple.Task4;
                dict["Output"] = task4.ToString();
                base.EditFile(FormatComplexOutput(dict, task4.Codes));
                break;
        }
    }

    private string FormatSimpleOutput(Dictionary<string, string> dict)
    {
        return $"Input: {dict["Input"]}\n" +
               $"Output: {dict["Output"]}\n" +
               $"Type: {dict["Type"]}";
    }

    private string FormatComplexOutput(Dictionary<string, string> dict, (string, char)[] codes)
    {
        var result = $"Input: {dict["Input"]}\n" +
                     $"Output: {dict["Output"]}\n" +
                     $"Type: {dict["Type"]}\n" +
                     $"Codes: \n";

        for (int i = 0; i < codes.Length; i++)
        {
            result += $"Item{i * 2}: {codes[i].Item1}\n" +
                      $"Item{i * 2 + 1}: {codes[i].Item2}";
        
            if (i < codes.Length - 1) result += "\n";
        }

        return result;
    }

    public override T Deserialize()
{
    if (!File.Exists(FullPath) || FileExtension != "txt") 
        return null;

    var content = File.ReadAllText(FullPath);
    var lines = content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    var dict = new Dictionary<string, string>();

    // Парсинг всех полей
    int itemCounter = 0;
    foreach (var line in lines)
    {
        if (line.Contains("Input"))
            dict["Input"] = ParseValue(line);
        else if (line.Contains("Output"))
            dict["Output"] = ParseValue(line);
        else if (line.Contains("Type"))
            dict["Type"] = ParseType(line);
        else if (line.Contains("Item"))
            dict[$"Item{itemCounter++}"] = ParseItemValue(line);
    }

    // Сборка массива кортежей для Task4
    var array = BuildTupleArray(dict, itemCounter);

    // Значения по умолчанию
    if (!dict.ContainsKey("Type"))
    {
        dict["Type"] = "Task1";
        dict["Input"] = content;
    }

    // Создание объекта
    T obj = dict["Type"] switch
    {
        "Task1" => new Lab9.Purple.Task1(dict["Input"]) as T,
        "Task2" => new Lab9.Purple.Task2(dict["Input"]) as T,
        "Task3" => new Lab9.Purple.Task3(dict["Input"]) as T,
        "Task4" => new Lab9.Purple.Task4(dict["Input"], array) as T,
        _ => null
    };

    obj?.Review();
    return obj;
}
    
private string ParseValue(string line)
{
    var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    return string.Join(" ", parts.Skip(1)).Trim(' ', '"');
}

private string ParseType(string line)
{
    return line.Split(':', StringSplitOptions.RemoveEmptyEntries)[1].Trim(' ', '"');
}

private string ParseItemValue(string line)
{
    var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    return parts[1].Trim(' ', '"');
}

private (string, char)[] BuildTupleArray(Dictionary<string, string> dict, int itemCount)
{
    var array = new (string, char)[itemCount / 2];
    for (int i = 0, index = 0; i < itemCount; i += 2, index++)
    {
        array[index].Item1 = dict[$"Item{i}"];
        char.TryParse(dict[$"Item{i + 1}"], out array[index].Item2);
    }
    return array;
}

    public override void ChangeFileExtension(string extension)
    {
        base.ChangeFileFormat(extension);
    }
}