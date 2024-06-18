﻿using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Element.Functions;

public class SearchElementFunction( string methodName, ParseExpressionContext context ) 
    : FilterExtensionFunction( methodName, 2, context )
{
    public const string Name = "search";

    private static readonly MethodInfo SearchMethod;

    static SearchElementFunction()
    {
        SearchMethod = typeof( SearchElementFunction ).GetMethod( nameof( Search ), [typeof( IEnumerable<JsonElement> ), typeof( string )] );
    }

    public override Expression GetExtensionExpression( string methodName, Expression[] arguments, ParseExpressionContext context )
    {
        if ( arguments.Length != 2 )
        {
            return Expression.Throw( Expression.Constant( new ArgumentException( $"{Name} function has invalid parameter count." ) ) );
        }

        return Expression.Call( SearchMethod, arguments[0], arguments[1] );
    }

    public static bool Search( IEnumerable<JsonElement> elements, string regex )
    {
        var elementValue = elements.FirstOrDefault().GetString();
        if ( elementValue == null )
        {
            return false;
        }

        var regexPattern = new Regex( regex.Trim( '\"', '\'' ) );
        return regexPattern.IsMatch( elementValue );
    }
}
