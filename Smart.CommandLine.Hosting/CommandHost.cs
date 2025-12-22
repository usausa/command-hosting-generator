namespace Smart.CommandLine.Hosting;

public static class CommandHost
{
    public static ICommandHostBuilder CreateDefaultBuilder(string[] args) =>
        new CommandHostBuilder(args);

    public static ICommandHostBuilder CreateBuilder(string[] args) =>
        new CommandHostBuilder(args, false);
}
