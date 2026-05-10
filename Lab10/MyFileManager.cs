namespace Lab10;

public abstract class MyFileManager:IFileManager,IFileLifeController
{
    public string Name { get; private set; }
    public string FolderPath { get; private set; }
    public string FileName { get; private set; }
    public string FileExtension { get; private set; }

    public string FullPath => Path.Combine(FolderPath, FileName) + "." + FileExtension;

    public MyFileManager(string name)
    {
        Name = name;
        FolderPath = "";
        FileName = "";
        FileExtension = "";
    }

    public MyFileManager(string name, string folderPath, string fileName, string fileExtension = "txt")
    {
        Name = name;
        FolderPath = folderPath;
        FileName = fileName;
        FileExtension = fileExtension;
    }

    public void SelectFolder(string folder)
    {
        FolderPath = folder;
    }

    public void ChangeFileName(string newName)
    {
        FileName = newName;
    }

    public void SelectFile(string file)
    {
        return;
    }

    public void ChangeFormat(string newFormat)
    {
        return;
    }

    public void ChangeFileFormat(string newFormat)
    {
        DeleteFile();
        FileExtension = newFormat;
        CreateFile();
    }

    public void CreateFile()
    {
        if (!Directory.Exists(FolderPath))
        {
            Directory.CreateDirectory(FolderPath);
        }

        if (!File.Exists(FullPath))
        {
            File.Create(FullPath).Close();
        }
    }

    public void DeleteFile()
    {
        if (File.Exists(FullPath))
        {
            File.Delete(FullPath);
        }
    }

    public void EditFile(string text)
    {
        File.WriteAllText(FullPath, text);
    }

    public void ChangeFileExtension(string newExtension)
    {
        string text = File.ReadAllText(FullPath);
        ChangeFileFormat(newExtension);
        EditFile(text);
    }
}