namespace Smart.CommandLine.Hosting;

internal sealed class FilterDescriptor
{
    public Type FilterType { get; }

    public int Order { get; }

    public FilterDescriptor(Type filterType, int order)
    {
        FilterType = filterType;
        Order = order;
    }
}
