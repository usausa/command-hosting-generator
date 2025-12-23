namespace Smart.CommandLine.Hosting;

public interface ICommandHandler
{
    ValueTask ExecuteAsync(CommandContext context);
}
