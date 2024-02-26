namespace SharedScans.Interfaces;

public class HookContainer<TFunction>
{
    public string Id { get; init; }

    public string Owner { get; init; }

    public TFunction HookFunction { get; init; }

    public nint Address { get; set; }

    public object? HookInstance { get; set; }

    public TFunction OriginalFunction { get; set; }
}
