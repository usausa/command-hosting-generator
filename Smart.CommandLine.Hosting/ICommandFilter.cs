namespace Smart.CommandLine.Hosting;

public interface ICommandFilter
{
    int Order { get; }

    ValueTask ExecuteAsync(CommandContext context, CommandDelegate next);
}
