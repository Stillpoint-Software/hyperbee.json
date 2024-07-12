using System.Linq.Expressions;
using System.Reflection;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Filters.Parser;

public abstract class FilterExtensionFunction
{
    private readonly int _argumentCount;
    private readonly MethodInfo _methodInfo;
    private MethodInfo _throwIfNotNormalizedMethodInfo;

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

            // Create expression that throws if not normalized.
            if ( expectNormalized )
            {
                _throwIfNotNormalizedMethodInfo ??= GetMethod<FilterExtensionFunction>( nameof( ThrowIfNotNormalized ) )
                    .MakeGenericMethod( typeof( TNode ) );

                arguments[i] = Expression.Call( _throwIfNotNormalizedMethodInfo,
                        Expression.Constant( _methodInfo.Name ),
                        Expression.Convert( argument, typeof( INodeType ) ) );
            }
            else
            {
                arguments[i] = Expression.Convert( argument, typeof( INodeType ) );
            }
        }

        return Expression.Call( _methodInfo, arguments );
    }

    public static INodeType ThrowIfNotNormalized<TNode>( string methodName, INodeType node )
    {
        if ( node is NodesType<TNode> { IsNormalized: false } )
            throw new NotSupportedException( $"Function {methodName} does not support non-singular arguments." );

        return node;
    }

    protected static MethodInfo GetMethod<T>( string methodName ) => typeof( T ).GetMethod( methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static );
}
