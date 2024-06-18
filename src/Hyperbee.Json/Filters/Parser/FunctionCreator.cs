namespace Hyperbee.Json.Filters.Parser;

public delegate FilterExtensionFunction FunctionCreator(
    string methodName,
    ParseExpressionContext context = null );
