using Reloaded.Hooks.Definitions;
using SharedScans.Interfaces;

namespace SharedScans.Reloaded.Scans;

internal class SharedScans : ISharedScans
{
    private readonly IReloadedHooks hooks;
    private readonly List<object> listeners = new();

    public SharedScans(IReloadedHooks hooks)
    {
        this.hooks = hooks;
    }

    public void AddScan<TFunction>(string id, string pattern)
    {
        ScanHooks.Add(id, pattern, (hooks, result) =>
        {
            foreach (var listener in this.listeners)
            {
                if (listener is Listener<TFunction> list && list.Id == id)
                {
                    list.Hook.Pattern = pattern;
                    list.Hook.Address = result;
                    list.Hook.HookInstance = this.hooks.CreateHook(list.Hook.Method, result).Activate();

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
        };

        var listener = new Listener<TFunction>(modName, id, hook);
        this.listeners.Add(listener);
        return listener.Hook;
    }

    private record Listener<TFunction>(string Name, string Id, HookContainer<TFunction> Hook);
}
