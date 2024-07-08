﻿using System.Linq.Expressions;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class SearchNodeFunction() : FilterExtensionFunction( argumentCount: 2 )
{
    public const string Name = "search";
    private static readonly Expression SearchExpression = Expression.Constant( (Func<IEnumerable<JsonNode>, string, bool>) Search );

    protected override Expression GetExtensionExpression( Expression[] arguments, bool[] argumentInfo )
    {
        return Expression.Invoke( SearchExpression, arguments[0], arguments[1] );
    }

    public static bool Search( IEnumerable<JsonNode> nodes, string regex )
    {
        var value = nodes.FirstOrDefault()?.GetValue<string>();

        if ( value == null )
        {
            return false;
        }

        var regexPattern = new Regex( regex.Trim( '\"', '\'' ) );
        return regexPattern.IsMatch( value );
    }
}
