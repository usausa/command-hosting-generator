namespace Smart.CommandLine.Hosting;

using System.CommandLine;

// ReSharper disable once UnusedType.Global
#pragma warning disable CA1711
public delegate ValueTask CommandActionDelegate(
    ICommandHandler command,
    ParseResult parseResult,
    CommandContext commandContext);
#pragma warning disable CA1711
