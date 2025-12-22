namespace Smart.CommandLine.Hosting;

public sealed class CommandContext
{
    public IDictionary<string, object?> Items
    {
        get
        {
            field ??= new Dictionary<string, object?>();
            return field;
        }
    }

    public ICommand Command { get; internal set; } = default!;

    public Type CommandType { get; internal set; } = default!;

    public CancellationToken CancellationToken { get; internal set; }

    public int ExitCode { get; set; }
}
