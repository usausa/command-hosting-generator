namespace Smart.CommandLine.Hosting;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public abstract class FilterAttribute : Attribute
{
    public int Order { get; set; }

    public abstract Type FilterType { get; }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class FilterAttribute<TFilter> : FilterAttribute
    where TFilter : ICommandFilter
{
    public override Type FilterType => typeof(TFilter);
}
