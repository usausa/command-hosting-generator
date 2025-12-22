namespace Smart.CommandLine.Hosting;

public interface ICommand
{
    ValueTask ExecuteAsync(CommandContext context);
}
