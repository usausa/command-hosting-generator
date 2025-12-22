namespace Smart.CommandLine.Hosting;

public interface ICommandHost : IAsyncDisposable
{
    ValueTask<int> RunAsync();
}
