using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Hyperbee.Json.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CSharp.RuntimeBinder;

namespace Hyperbee.Json.Evaluators;


public abstract partial class JsonPathCSharpEvaluator<TType> : IJsonPathScriptEvaluator<TType>
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly ConcurrentDictionary<string, Script<object>> Compiled = new();

    protected static Regex ThisPropertyRegex => PropertyRegex();
    protected static Regex ThisReservedRegex => ReservedRegex();

    [GeneratedRegex( "@[A-Za-z_][A-Za-z0-9_]*" )]
    private static partial Regex ReservedRegex();

    [GeneratedRegex( "@\\.[A-Za-z_][A-Za-z0-9_]*" )]
    private static partial Regex PropertyRegex();

    public object Evaluator( string script, TType current, TType root, string context )
    {
        var compiled = Compiled.GetOrAdd( script, key =>
        {
            var normalizedScript = TransformExpression( script );

            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile( typeof(RuntimeBinderException).GetTypeInfo().Assembly.Location ),
                MetadataReference.CreateFromFile( typeof(System.Runtime.CompilerServices.DynamicAttribute).GetTypeInfo().Assembly.Location )
            };

            var code = CSharpScript.Create( normalizedScript, ScriptOptions.Default.AddReferences( references ), typeof( Globals ) );
            code.Compile();

            return code;
        } );

        try
        {
            var globals = ActivateGlobals( current, context );

            var result = AsyncCurrentThreadHelper.RunSync(
                async () => await compiled.RunAsync( globals ).ConfigureAwait( true )
            );

            return result.ReturnValue;
        }
        catch ( RuntimeBinderException )
        {
            return null; // missing members should act falsy
        }
        catch ( Exception ex )
        {
            throw new JsonPathEvaluatorException( "Error compiling JsonPath expression.", ex );
        }
    }

    protected abstract Globals ActivateGlobals( TType current, string context );

    protected virtual string TransformExpression( string expression )
    {
        var result = expression;

        result = ThisPropertyRegex.Replace( result, x => $"This.{x.Value[2..]}" ); // '@.' to 'This.'
        result = ThisReservedRegex.Replace( result, x => $"This.{x.Value[1..]}()" ); // '@path' to 'This.path()'

        return result;
    }
}

// we would have liked to declare Globals as a nested type within the evaluator
// but the roslyn script compiler doesn't like it when the parent type is generic.

public sealed class Globals
{
    internal Globals()
    {
    }

    public dynamic This { get; internal set; }
}

public class JsonPathCSharpElementEvaluator : JsonPathCSharpEvaluator<JsonElement>
{
    protected override Globals ActivateGlobals( JsonElement current, string context ) => new() { This = current.ToDynamic( context ) };
}

public class JsonPathCSharpNodeEvaluator : JsonPathCSharpEvaluator<JsonNode>
{
    protected override Globals ActivateGlobals( JsonNode current, string context ) => new() { This = current.ToDynamic() };
}
