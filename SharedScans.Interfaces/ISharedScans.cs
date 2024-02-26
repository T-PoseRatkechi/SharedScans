namespace SharedScans.Interfaces;

public interface ISharedScans
{
    void AddScan<TFunction>(string id, string pattern);

    void AddScan<TFunction>(string pattern);

    HookContainer<TFunction> CreateHook<TFunction>(string id, Action<nint> success);

    HookContainer<TFunction> CreateHook<TFunction>(string modName, TFunction hookFunc);
}

public class HookContainer<TFunction>
{
    public string Id { get; set; }

    public string Pattern { get; set; }

    public nint Address { get; set; }

    public object? HookInstance { get; set; }

    public TFunction Method { get; set; }
}
