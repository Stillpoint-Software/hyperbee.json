using System.Collections.Concurrent;
using System.Linq.Expressions;
using static Hyperbee.Json.Evaluators.Parser.JsonPathExpression;

namespace Hyperbee.Json.Evaluators.Parser.Functions;

public class ParserFunction<TType>
{
    public delegate ParserExpressionFunction<TType> FunctionCreator( string methodName, IList<string> arguments, ParseExpressionContext<TType> context = null );

    private readonly ParserFunction<TType> _implementation;
    private static readonly ConcurrentDictionary<string, FunctionCreator> ExpressionFunctions;

    static ParserFunction()
    {
        ExpressionFunctions = new ConcurrentDictionary<string, FunctionCreator>(
        [
            new KeyValuePair<string, FunctionCreator>( JsonPathCountFunction<TType>.Name, ( name, arguments, context ) => new JsonPathCountFunction<TType>( name, arguments, context ) ),
            new KeyValuePair<string, FunctionCreator>( JsonPathLengthFunction<TType>.Name, ( name, arguments, context ) => new JsonPathLengthFunction<TType>( name, arguments, context ) ),
            new KeyValuePair<string, FunctionCreator>( JsonPathMatchFunction<TType>.Name, ( name, arguments, context ) => new JsonPathMatchFunction<TType>( name, arguments, context ) ),
            new KeyValuePair<string, FunctionCreator>( JsonPathSearchFunction<TType>.Name, ( name, arguments, context ) => new JsonPathSearchFunction<TType>( name, arguments, context ) ),
            new KeyValuePair<string, FunctionCreator>( JsonPathValueFunction<TType>.Name, ( name, arguments, context ) => new JsonPathValueFunction<TType>( name, arguments, context ) ),
            new KeyValuePair<string, FunctionCreator>( JsonPathPathFunction<TType>.Name, ( name, arguments, context ) => new JsonPathPathFunction<TType>( name, arguments, context ) ),
        ] );
    }

    public ParserFunction()
    {
        _implementation = this;
    }

    internal ParserFunction( ReadOnlySpan<char> item, FilterTokenType? type, ParseExpressionContext<TType> context )
    {
        if ( item.Length == 0 && type == FilterTokenType.OpenParen )
        {
            // There is no function, just an expression in parentheses.
            _implementation = new ParenFunction<TType>( context );
            return;
        }

        var currentPath = item[0] == '@';
        var rootPath = item[0] == '$';

        if ( item.Length > 0 && (currentPath || rootPath) )
        {
            // There is a JsonPath sub query.
            _implementation = rootPath
                ? new JsonPathElementFunction<TType>( context with { Current = context.Root } ) // Current becomes root
                : new JsonPathElementFunction<TType>( context );

            return;
        }

        if ( TryGetExpressionFunction( item, context, out _implementation ) )
        {
            // Methods based on spec
            return;
        }

        // Function not found, will try to parse this as a literal value.
        var literalFunction = new LiteralFunction<TType>();
        _implementation = literalFunction;
    }

    public Expression GetExpression( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        return _implementation.Evaluate( data, item, ref start, ref from );
    }

    public static void AddFunction( string name, FunctionCreator creator )
    {
        ExpressionFunctions[name] = creator;
    }

    protected virtual Expression Evaluate( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        // The real implementation will be in the derived classes.
        return Expression.Throw( Expression.Constant( new NotImplementedException() ) );
    }

    private static bool TryGetExpressionFunction( ReadOnlySpan<char> item, ParseExpressionContext<TType> context, out ParserFunction<TType> function )
    {
        var match = JsonPathFilterTokenizerRegex.RegexFunction().Match( item.ToString() );

        if ( match.Groups.Count != 3 )
        {
            function = null;
            return false;
        }

        var method = match.Groups[1].Value;
        var arguments = match.Groups[2].Value.Split( ',', options: StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries );

        if ( ExpressionFunctions.TryGetValue( method.ToLowerInvariant(), out var creator ) )
        {
            function = creator( method, arguments, context );
            return true;
        }

        function = null;
        return false;
    }
}
