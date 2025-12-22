namespace Smart.CommandLine.Hosting;

public sealed class CommandFilterOptions
{
    public FilterCollection GlobalFilters { get; } = new();

    public bool IncludeBaseClassFilters { get; set; } = true;

    public int DefaultFilterOrder { get; set; }
}

#pragma warning disable CA1711
public sealed class FilterCollection
#pragma warning restore CA1711
{
    internal List<FilterDescriptor>? Descriptors { get; set; }

    public void Add<TFilter>(int order = 0)
        where TFilter : ICommandFilter
    {
        Descriptors ??= new List<FilterDescriptor>();
        Descriptors.Add(new FilterDescriptor
        {
            FilterType = typeof(TFilter),
            Order = order
        });
    }

    public void Add(Type filterType, int order = 0)
    {
        if (!typeof(ICommandFilter).IsAssignableFrom(filterType))
        {
            throw new ArgumentException($"Type must implement '{typeof(ICommandFilter).FullName}' interface.", nameof(filterType));
        }

        Descriptors ??= new List<FilterDescriptor>();
        Descriptors.Add(new FilterDescriptor
        {
            FilterType = filterType,
            Order = order
        });
    }
}

internal sealed class FilterDescriptor
{
    public Type FilterType { get; set; } = default!;

    public int Order { get; set; }
}
