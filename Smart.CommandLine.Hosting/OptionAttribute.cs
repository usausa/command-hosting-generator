namespace Smart.CommandLine.Hosting;

internal static class OptionPosition
{
    public const int Auto = -1;
}

#pragma warning disable CA1711
public interface IOptionAttribute
{
    int GetPosition();

    string GetName();

    string? GetDescription();

    bool GetIsRequired();

    object? GetDefaultValue();
}
#pragma warning restore CA1711

[AttributeUsage(AttributeTargets.Property)]
public abstract class BaseOptionAttribute : Attribute, IOptionAttribute
{
    public int Position { get; }

    public string Name { get; }

    public string? Description { get; set; }

    public bool IsRequired { get; set; } = true;

    protected BaseOptionAttribute(int position, string name)
    {
        Position = position;
        Name = name;
    }

#pragma warning disable CA1033
    int IOptionAttribute.GetPosition() => Position;

    string IOptionAttribute.GetName() => Name;

    string? IOptionAttribute.GetDescription() => Description;

    bool IOptionAttribute.GetIsRequired() => IsRequired;

    object? IOptionAttribute.GetDefaultValue() => ResolveDefaultValue();

    protected abstract object? ResolveDefaultValue();
#pragma warning restore CA1033
}

public sealed class OptionAttribute : BaseOptionAttribute
{
    public OptionAttribute(string name)
        : base(OptionPosition.Auto, name)
    {
    }

    public OptionAttribute(int position, string name)
        : base(position, name)
    {
    }

    protected override object? ResolveDefaultValue() => null;
}

public sealed class OptionAttribute<T> : BaseOptionAttribute
{
    public T? DefaultValue { get; set; }

    public OptionAttribute(string name)
        : base(OptionPosition.Auto, name)
    {
    }

    public OptionAttribute(int position, string name)
        : base(position, name)
    {
    }

    protected override object? ResolveDefaultValue() => DefaultValue;
}
