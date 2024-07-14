﻿using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Filters.Parser
{
    public abstract class FilterExtensionFunction
    {
        private readonly int _argumentCount;
        private readonly MethodInfo _methodInfo;

        private static readonly MethodInfo __validateArgumentMethod =
            typeof( FilterExtensionFunction ).GetMethod( nameof( ValidateArgument ), BindingFlags.Public | BindingFlags.Static );

        private static readonly ConcurrentDictionary<Type, MethodInfo> CachedGenericMethods = new();

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
                    IsArgument = true,
                    Terminal = i == _argumentCount - 1
                        ? FilterParser.ArgClose
                        : FilterParser.ArgComma
                };

                if ( localState.EndOfBuffer )
                    throw new NotSupportedException( $"Invalid arguments for filter: \"{state.Buffer}\"." );

                var argument = FilterParser<TNode>.Parse( ref localState, parserContext );
                arguments[i] = ArgumentExpression<TNode>( expectNormalized, argument );
            }

            // Call the method and cast the result to INodeType
            var callExpression = Expression.Call( _methodInfo, arguments );
            var castExpression = Expression.Convert( callExpression, typeof( IValueType ) );

            return castExpression;
        }

        private Expression ArgumentExpression<TNode>( bool expectNormalized, Expression argument )
        {
            if ( expectNormalized )
            {
                var validateArgumentMethod = CachedGenericMethods.GetOrAdd( typeof( TNode ),
                    type => __validateArgumentMethod.MakeGenericMethod( type ) );

                // Create expression that throws if not normalized.
                return Expression.Call(
                    validateArgumentMethod,
                    Expression.Constant( _methodInfo.Name ),
                    Expression.Convert( argument, typeof( IValueType ) )
                );
            }

            return Expression.Convert( argument, typeof( IValueType ) );
        }

        public static IValueType ValidateArgument<TNode>( string methodName, IValueType argument )
        {
            if ( argument is NodeList<TNode> { IsNormalized: false } )
                throw new NotSupportedException( $"Function {methodName} does not support non-singular arguments." );

            return argument;
        }

        protected static MethodInfo GetMethod<T>( string methodName ) =>
            typeof( T ).GetMethod( methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static );
    }
}
