using System.Collections.Immutable;
using Hyperbee.Json.Tokenizer;

namespace Hyperbee.Json.Extensions;

internal static class ImmutableStackExtensions
{
    internal static IImmutableStack<JsonPathToken> Push(this IImmutableStack<JsonPathToken> stack, string selector, SelectorKind kind)
    {
        return stack.Push(new JsonPathToken(selector, kind));
    }
}