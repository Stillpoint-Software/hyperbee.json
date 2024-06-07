using System.Linq.Expressions;

namespace Hyperbee.Json.Evaluators.Parser;

public record ParseExpressionContext<TType>( Expression Current, Expression Root, IJsonPathFilterEvaluator<TType> Evaluator );
