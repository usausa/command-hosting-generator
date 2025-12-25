namespace Smart.CommandLine.Hosting;

public interface ICommandHost : IAsyncDisposable
{
    IServiceProvider Services { get; }

    ValueTask<int> RunAsync();
}
