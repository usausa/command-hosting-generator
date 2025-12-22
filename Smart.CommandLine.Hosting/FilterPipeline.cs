namespace Smart.CommandLine.Hosting;

using System.Reflection;

using Microsoft.Extensions.Options;

internal sealed class FilterPipeline
{
    private readonly IServiceProvider serviceProvider;

    private readonly CommandFilterOptions options;

    public FilterPipeline(IServiceProvider serviceProvider, IOptions<CommandFilterOptions> options)
    {
        this.serviceProvider = serviceProvider;
        this.options = options.Value;
    }

    public ValueTask ExecuteAsync(CommandContext context, Func<CommandContext, ValueTask> action)
    {
        // Collect filters
        var filters = options.GlobalFilters.Descriptors is not null
            ? new List<FilterDescriptor>(options.GlobalFilters.Descriptors)
            : [];

        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var attribute in context.CommandType.GetCustomAttributes<FilterAttribute>())
        {
            filters.Add(new FilterDescriptor(attribute.FilterType, attribute.Order));
        }

        if (filters.Count == 0)
        {
            // Without pipeline
            return action(context);
        }

        filters.Sort(static (x, y) => x.Order.CompareTo(y.Order));

        // Create pipeline
        CommandDelegate pipeline = ctx => action(ctx);
        for (var i = filters.Count - 1; i >= 0; i--)
        {
            if (serviceProvider.GetService(filters[i].FilterType) is ICommandFilter commandFilter)
            {
                var next = pipeline;
                pipeline = ctx => commandFilter.ExecuteAsync(ctx, next);
            }
        }

        // With pipeline
        return pipeline(context);
    }
}
