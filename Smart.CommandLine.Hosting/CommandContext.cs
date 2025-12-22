namespace Smart.CommandLine.Hosting;

public sealed class CommandContext
{
    public Dictionary<string, object?> Items
    {
        get
        {
            field ??= [];
            return field;
        }
    }

    public ICommand Command { get; internal set; } = default!;

    public Type CommandType { get; internal set; } = default!;

    public int ExitCode { get; set; }
}
