using System.Linq.Expressions;
using System.Reflection;
using Hyperbee.Json.Filters.Values;
using Hyperbee.Json.Internal;

namespace Hyperbee.Json.Filters.Parser;

public abstract class FilterExtensionFunction
{
    private readonly int _argumentCount;
    private readonly MethodInfo _methodInfo;

    public FilterExtensionInfo FunctionInfo { get; }

    protected FilterExtensionFunction( MethodInfo methodInfo, FilterExtensionInfo filterInfo )
    {
        _argumentCount = methodInfo.GetParameters().Length;
        _methodInfo = methodInfo;

        FunctionInfo = filterInfo;
    }

    internal Expression GetExpression<TNode>( ref ParserState state, FilterParserContext<TNode> parserContext )
    {
        var arguments = new Expression[_argumentCount];
        var expectNormalized = FunctionInfo.HasFlag( FilterExtensionInfo.ExpectNormalized );

        for ( var i = 0; i < _argumentCount; i++ )
        {
            var localState = state with
            {
                Item = [],
                Terminal = i == _argumentCount - 1
                    ? FilterParser.ArgClose
                    : FilterParser.ArgComma,
                IsArgument = true
            };

            if ( localState.EndOfBuffer )
                throw new NotSupportedException( $"Invalid arguments for filter: \"{state.Buffer}\"." );

            var argument = FilterParser<TNode>.Parse( ref localState, parserContext );

            if ( expectNormalized )
            {
                if ( QueryHelper.IsNonSingular( localState.Item ) )
                    throw new NotSupportedException( $"Function {_methodInfo.Name} does not support non-singular arguments. Error in Parameter {i + 1}." );
            }

            arguments[i] = Expression.Convert( argument, typeof( INodeType ) );
        }

        return Expression.Call( _methodInfo, arguments );
    }

    protected static MethodInfo GetMethod<T>( string methodName ) => typeof( T ).GetMethod( methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static );
}
