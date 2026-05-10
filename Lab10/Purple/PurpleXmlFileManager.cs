using System.Xml.Serialization;

namespace Lab10.Purple;

public class PurpleXmlFileManager<T>:PurpleFileManager<T> where T:Lab9.Purple.Purple
{
    public PurpleXmlFileManager(string name) : base(name) {}
    public PurpleXmlFileManager(string name, string folderPath, string fileName, string fileExtension = "txt") : base(name,folderPath,fileName,fileExtension){}

    public override void EditFile(string text)
    {
        T obj = Deserialize();
        obj.ChangeText(text);
        Serialize(obj);
    }
    public override void Serialize(T obj)
    {
        if (obj == null) return;

        var type = obj.GetType().Name;
        var input = obj.Input;
        var output = obj.ToString();

        DTOPurple dto = type switch
        {
            "Task1" or "Task2" => new DTOPurple(type, input, output),
            "Task3" => new DTOPurple(type, input, output, (obj as Lab9.Purple.Task3).Codes),
            "Task4" => new DTOPurple(type, input, output, (obj as Lab9.Purple.Task4).Codes),
            _ => null
        };

        if (dto == null) return;

        ChangeFileFormat("xml");
        using (var fs = new FileStream(FullPath, FileMode.OpenOrCreate))
        {
            new XmlSerializer(dto.GetType()).Serialize(fs, dto);
        }
    }

    public override T Deserialize()
    {
        if (!File.Exists(FullPath) || FileExtension != "xml") 
            return null;

        DTOPurple dto = null;

        try
        {
            using (var fs = new FileStream(FullPath, FileMode.OpenOrCreate))
            {
                dto = new XmlSerializer(typeof(DTOPurple)).Deserialize(fs) as DTOPurple;
            }
        }
        catch (Exception)
        {
            dto = new DTOPurple("Task1", File.ReadAllText(FullPath), "");
        }

        T obj = dto.Type switch
        {
            "Task1" => new Lab9.Purple.Task1(dto.Input) as T,
            "Task2" => new Lab9.Purple.Task2(dto.Input) as T,
            "Task3" => new Lab9.Purple.Task3(dto.Input) as T,
            "Task4" => new Lab9.Purple.Task4(dto.Input, dto.Codes) as T,
            _ => null
        };

        obj.Review();
        return obj;
    }
    public override void ChangeFileExtension(string extension = "xml")
    {
        T obj = Deserialize(); 
        ChangeFileFormat(extension);
        Serialize(obj);
    }
}

public class DTOPurple
{
    public string Type { get; set; }
    public string Input { get; set; }
    public string Output { get; set; }
    public (string, char)[] Codes { get; set; }

    public DTOPurple(string type, string input, string output)
    {
        Type = type;
        Input = input;
        Output = output;
        Codes = null;
    }

    public DTOPurple(string type, string input, string output, (string, char)[] codes)
    {
        Type = type;
        Input = input;
        Output = output;
        Codes = codes;
    }

    public DTOPurple()
    {
        Type = null;
        Input = null;
        Output = null;
        Codes = null;
    }
}