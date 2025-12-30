namespace Smart.CommandLine.Hosting;

internal interface IFilterAttribute
{
    int GetOrder();

    Type GetFilterType();
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class FilterAttribute<TFilter> : Attribute, IFilterAttribute
    where TFilter : ICommandFilter
{
    public int Order { get; set; }

    public Type FilterType => typeof(TFilter);

    int IFilterAttribute.GetOrder() => Order;

    Type IFilterAttribute.GetFilterType() => FilterType;
}
