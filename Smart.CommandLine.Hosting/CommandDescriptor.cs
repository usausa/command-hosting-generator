namespace Smart.CommandLine.Hosting;

internal sealed class CommandDescriptor
{
    public Type CommandType { get; }

    public List<CommandDescriptor> SubCommands
    {
        get
        {
            field ??= [];
            return field;
        }
    }

    public Action<CommandActionBuilderContext>? ActionBuilder { get; }

    public CommandDescriptor(Type commandType, Action<CommandActionBuilderContext>? builder = null)
    {
        CommandType = commandType;
        ActionBuilder = builder;
    }
}
