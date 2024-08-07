﻿using System.Linq.Expressions;
using System.Reflection;
using Hyperbee.Json.Path.Filters.Values;

namespace Hyperbee.Json.Path.Filters.Parser;

public abstract class ExtensionFunction
{
    private readonly int _argumentCount;
    private readonly MethodInfo _methodInfo;

    public CompareConstraint CompareConstraint { get; }

    protected ExtensionFunction( MethodInfo methodInfo, CompareConstraint compareConstraint )
    {
        _argumentCount = methodInfo.GetParameters().Length;
        _methodInfo = methodInfo;

        CompareConstraint = compareConstraint;
    }

    internal Expression GetExpression<TNode>( ref ParserState state )
    {
        var arguments = new Expression[_argumentCount];
        var expectNormalized = CompareConstraint.HasFlag( CompareConstraint.ExpectNormalized );

        for ( var i = 0; i < _argumentCount; i++ )
        {
            var localState = state with
            {
                Item = [],
                IsArgument = true,
                TerminalCharacter = i == _argumentCount - 1
                    ? FilterParser.ArgClose
                    : FilterParser.ArgComma
            };

            if ( localState.EndOfBuffer )
                throw new NotSupportedException( $"Invalid arguments for filter: \"{state.Buffer}\"." );

            var argument = FilterParser<TNode>.Parse( ref localState );
            arguments[i] = ArgumentExpression<TNode>( expectNormalized, argument );
        }

        // Call the method and cast the result to support covariant returns

        return Expression.Convert(
            Expression.Call( _methodInfo, arguments ),
            typeof( IValueType )
        );
    }

    private Expression ArgumentExpression<TNode>( bool expectNormalized, Expression argument )
    {
        if ( expectNormalized )
        {
            // Create expression that throws if not normalized.
            return Expression.Call(
                ExpressionHelper<TNode>.ValidateArgumentMethod,
                Expression.Constant( _methodInfo.Name ),
                Expression.Convert( argument, typeof( IValueType ) )
            );
        }

        return Expression.Convert( argument, typeof( IValueType ) );
    }

    protected static MethodInfo GetMethod<T>( string methodName ) =>
        typeof( T ).GetMethod( methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static );

    private static class ExpressionHelper<TNode> // helper removes need for MakeGenericMethod
    {
        public static readonly MethodInfo ValidateArgumentMethod =
            typeof( ExpressionHelper<TNode> )
                .GetMethod( nameof( ValidateArgument ), BindingFlags.NonPublic | BindingFlags.Static );

        private static IValueType ValidateArgument( string methodName, IValueType argument )
        {
            if ( argument is NodeList<TNode> { IsNormalized: false } )
                throw new NotSupportedException( $"Function {methodName} does not support non-singular arguments." );

            return argument;
        }
    }
}
