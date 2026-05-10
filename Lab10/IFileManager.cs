namespace Lab10;

public interface IFileManager
{
    string FolderPath { get; }
    string FileName { get; }
    string FileExtension { get; }
    string FullPath { get; }
    void SelectFolder(string folder);
    void SelectFile(string file);
    void ChangeFormat(string newFormat);
    void ChangeFileName(string newName);
    void ChangeFileFormat(string newFormat);
}