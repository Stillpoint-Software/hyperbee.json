namespace Hyperbee.Json.Evaluators.Parser;

public delegate FilterExpressionFunction FunctionCreator(
    string methodName,
    IList<string> arguments,
    ParseExpressionContext context = null );
