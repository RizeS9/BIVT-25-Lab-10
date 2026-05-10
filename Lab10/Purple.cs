namespace Lab10.Purple;

public class Purple<T> where T:Lab9.Purple.Purple
{
    private PurpleFileManager<T> _manager;
    private T[] _tasks;
    public PurpleFileManager<T> Manager => _manager;
    public T[] Tasks => _tasks;

    public Purple(T[] tasks)
    {
        _manager = null;
        _tasks = tasks;
    }

    public Purple(PurpleFileManager<T> manager, T[] tasks = null)
    {
        _manager = manager;
        if (tasks == null)
        {
            _tasks = new T[0];
        }
        else
        {
            _tasks = tasks;
        }
    }
    public Purple()
    {
        _manager = null;
        _tasks = new T[0];
    }

    public void Add(T obj)
    {
        if (_tasks == null) return;
        if (obj == null) return;
        Array.Resize(ref _tasks,_tasks.Length+1);
        _tasks[_tasks.Length - 1] = obj;
    }

    public void Add(T[] tasks)
    {
        if (tasks == null) return;
        for (int i = 0; i < tasks.Length; i++)
        {
            Add(tasks[i]);
        }
    }

    public void Remove(T obj)
    {
        if (_tasks == null) return;
        if (obj == null) return;
        _tasks = _tasks.Where(x => x != obj).ToArray();
    }

    public void Clear()
    {
        if (_tasks == null) return;
        foreach (var t in _tasks)
        {
            Remove(t);
        }

        if (!Directory.Exists(_manager.FolderPath)) return;
        Directory.Delete(_manager.FolderPath,true);
    }

    public void SaveTasks()
    {
        if (_tasks == null) return;
        for (int i = 0; i < _tasks.Length; i++)
        {
            _manager.ChangeFileName($"Task {i + 1}");
            _manager.Serialize(_tasks[i]);
        }
    }

    public void LoadTasks()
    {
        var tasks = new List<T>();
    
        for (int i = 0; ; i++)
        {
            _manager.ChangeFileName($"Task {i + 1}");
            if (!File.Exists(_manager.FullPath)) break;
            T obj = _manager.Deserialize();
            tasks.Add(obj);
        }
    
        _tasks = tasks.ToArray();
    }

    public void ChangeManager(PurpleFileManager<T> manager)
    {
        if (manager == null) return;
        _manager = manager;
        if (!Directory.Exists(_manager.Name))
        {
            Directory.CreateDirectory(_manager.Name);
            _manager.SelectFolder(_manager.Name);
        }
        else
        {
            _manager.SelectFolder(_manager.Name);
        }
    }
}