namespace Hyperbee.Json.Memory;

[Flags]
internal enum SpanSplitOptions
{
    None = 0,
    RemoveEmptyEntries = 1,
    Reverse = 2
}
