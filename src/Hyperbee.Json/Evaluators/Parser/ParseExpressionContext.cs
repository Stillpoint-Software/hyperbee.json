using System.Linq.Expressions;

namespace Hyperbee.Json.Evaluators.Parser;

public record ParseExpressionContext(
    Expression Current,
    Expression Root,
    IJsonTypeDescriptor Descriptor
);
