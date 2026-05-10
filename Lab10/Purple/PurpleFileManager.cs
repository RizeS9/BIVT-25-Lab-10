namespace Lab10.Purple;

public abstract class PurpleFileManager<T>:MyFileManager,ISerializer<T> where T:Lab9.Purple.Purple
{
    public PurpleFileManager(string name) : base(name) {}
    public PurpleFileManager(string name, string folderPath, string fileName, string fileExtension = "txt") : base(name,folderPath,fileName,fileExtension){}

    public virtual void EditFile(string text)
    {
        if (!File.Exists(FullPath)) return;
        base.EditFile(text);
    }
    public virtual void ChangeFileExtension(string newExtension)
    {
        if (!File.Exists(FullPath)) return;
        base.ChangeFileExtension(newExtension);
    }

    public abstract void Serialize(T obj);
    public abstract T Deserialize();
    
}