using SharedScans.Interfaces;

namespace SharedScans.Reloaded.Scans;

internal class SharedScansService : ISharedScans
{
    private readonly ScansManager scansManager;
    private readonly List<object> listeners = new();

    public SharedScansService(ScansManager scansManager)
    {
        this.scansManager = scansManager;
    }

    public void AddScan<TFunction>(string id, string pattern)
    {
        this.scansManager.Add(id, pattern, (hooks, result) =>
        {
            foreach (var listener in this.listeners)
            {
                if (listener is Listener<TFunction> list && list.Id == id)
                {
                    list.Hook.Pattern = pattern;
                    list.Hook.Address = result;
                    var hook = hooks.CreateHook(list.Hook.Method, result).Activate();
                    list.Hook.HookInstance = hook;
                    list.Hook.OriginalFunction = hook.OriginalFunction;

                    Log.Information($"Hook created || {id} || For: {list.Name}");
                }
            }
        });
    }

    public void AddScan<TFunction>(string pattern)
        => this.AddScan<TFunction>(typeof(TFunction).Name, pattern);

    public HookContainer<TFunction> CreateHook<TFunction>(string id, Action<nint> success)
    {
        throw new NotImplementedException();
    }

    public HookContainer<TFunction> CreateHook<TFunction>(string modName, TFunction hookFunc)
    {
        var id = typeof(TFunction).Name;
        var hook = new HookContainer<TFunction>()
        {
            Id = id,
            Method = hookFunc,
        };

        var listener = new Listener<TFunction>(modName, id, hook);
        this.listeners.Add(listener);
        return listener.Hook;
    }

    private record Listener<TFunction>(string Name, string Id, HookContainer<TFunction> Hook);
}
