namespace Smart.CommandLine.Hosting;

public interface ICommandHost
{
    Task<int> RunAsync();
}
