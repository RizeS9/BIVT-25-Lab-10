using System.Text.Encodings.Web;
using System.Text.Json;
using Lab9.Purple;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Lab10.Purple;

public class PurpleJsonFileManager<T>:PurpleFileManager<T> where T:Lab9.Purple.Purple
{

    public PurpleJsonFileManager(string name) : base(name) {}

    public PurpleJsonFileManager(string name, string folderPath, string fileName, string fileExtension = "txt") : base(
        name, folderPath, fileName, fileExtension){}
    public override void EditFile(string text)
    {
        if (!File.Exists(FullPath)) return;
        T obj = Deserialize();
        obj.ChangeText(text);
        Serialize(obj);
    }
    public override void Serialize(T obj)
    {
        if (obj == null) return;
    
        var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(
            JsonConvert.SerializeObject(obj, Formatting.Indented));
    
        dict["Type"] = obj.GetType().Name;
    
        ChangeFileExtension("json");
        base.EditFile(JsonConvert.SerializeObject(dict, Formatting.Indented));
    }

    public override T Deserialize()
{
    if (!File.Exists(FullPath) || FileExtension != "json") 
        return null;

    string content = File.ReadAllText(FullPath);
    var dict = new Dictionary<string, string>();
    var lines = content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    
    foreach (var line in lines)
    {
        if (line.Contains("Input"))
        {
            var inputValue = line
                .Replace(@"\""", "\"")
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Aggregate((a, b) => a + " " + b)
                .Trim('"', ',');
            
            dict["Input"] = inputValue;
        }
        else if (line.Contains("Type"))
        {
            var typeValue = line.Split(':', StringSplitOptions.RemoveEmptyEntries)[1]
                .Trim('"', ' ', ',');
            dict["Type"] = typeValue;
        }
    }
    
    var items = lines
        .Select((line, index) => new { line, index })
        .Where(x => x.line.Contains("Item"))
        .ToList();

    var array = new (string, char)[items.Count / 2];
    for (int i = 0; i < items.Count; i += 2)
    {
        var itemLine = items[i].line;
        var valuePart = itemLine.Split(':', StringSplitOptions.RemoveEmptyEntries)[1]
            .Trim(',', ' ', '"');
        
        char charValue;
        char.TryParse(items[i + 1].line.Split(':')[1].Trim(',', ' ', '"'), out charValue);
        
        array[i / 2] = (valuePart, charValue);
    }
    
    if (!dict.ContainsKey("Type"))
    {
        dict["Type"] = typeof(Lab9.Purple.Task1).Name;
        dict["Input"] = content;
    }
    
    T obj = dict["Type"] switch
    {
        "Task1" => new Lab9.Purple.Task1(dict["Input"]) as T,
        "Task2" => new Lab9.Purple.Task2(dict["Input"]) as T,
        "Task3" => new Lab9.Purple.Task3(dict["Input"]) as T,
        "Task4" => new Lab9.Purple.Task4(dict["Input"], array) as T,
        _ => null
    };

    obj.Review();
    return obj;
}

    public override void ChangeFileExtension(string extension)
    {
        base.ChangeFileFormat(extension);
    }
}