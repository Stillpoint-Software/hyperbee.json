using System.Linq.Expressions;

namespace Hyperbee.Json.Evaluators.Parser;

//public record ParseExpressionContext<TType>( Expression Current, Expression Root, IJsonPathScriptEvaluator<TType> Evaluator, string BasePath = "" );
public record ParseExpressionContext<TType>( Expression Current, Expression Root, IJsonPathScriptEvaluator<TType> Evaluator, Expression BasePath );
