namespace Hyperbee.Json.Tests.TestSupport;

public record JsonPathPair
{
    public string Path { get; init; }
    public dynamic Value { get; init; }
}