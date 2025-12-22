namespace Smart.CommandLine.Hosting;

internal static class OptionPosition
{
    public const int Auto = -1;
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class OptionAttribute : Attribute
{
    public int Position { get; }

    public string Name { get; }

    public string? Description { get; set; }

    public bool IsRequired { get; set; } = true;

    public OptionAttribute(string name)
    {
        Position = OptionPosition.Auto;
        Name = name;
    }

    public OptionAttribute(int position, string name)
    {
        Position = position;
        Name = name;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class OptionAttribute<T> : Attribute
{
    public int Position { get; }

    public string Name { get; }

    public string? Description { get; set; }

    public bool IsRequired { get; set; } = true;

    public T? DefaultValue { get; set; }

    public OptionAttribute(string name)
    {
        Position = OptionPosition.Auto;
        Name = name;
    }

    public OptionAttribute(int position, string name)
    {
        Position = position;
        Name = name;
    }
}
