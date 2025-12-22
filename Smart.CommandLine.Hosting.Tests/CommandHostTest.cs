namespace Smart.CommandLine.Hosting;

public sealed class CommandHostTest
{
    [Fact]
    public void Test1()
    {
        var builder = CommandHost.CreateBuilder([]);

        Assert.NotNull(builder);
    }
}
