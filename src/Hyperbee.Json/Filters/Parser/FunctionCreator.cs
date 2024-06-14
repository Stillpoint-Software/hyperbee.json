namespace Hyperbee.Json.Filters.Parser;

public delegate FilterExtensionFunction FunctionCreator(
    string methodName,
    IList<string> arguments,
    ParseExpressionContext context = null );
