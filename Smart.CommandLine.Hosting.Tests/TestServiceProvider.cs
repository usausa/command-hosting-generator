namespace Smart.CommandLine.Hosting;

public sealed class TestServiceProvider : IServiceProvider
{
    public Dictionary<Type, object> Services { get; } = [];

    public Func<Type, object?>? GetServiceFunc { get; set; }

    public void AddService(Type serviceType, object service)
    {
        Services[serviceType] = service;
    }

    public object? GetService(Type serviceType)
    {
        if (GetServiceFunc is not null)
        {
            return GetServiceFunc(serviceType);
        }

        return Services.TryGetValue(serviceType, out var service) ? service : null;
    }
}
