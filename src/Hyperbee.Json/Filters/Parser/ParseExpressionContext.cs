using System.Linq.Expressions;
using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Filters.Parser;

public record ParseExpressionContext(
    Expression Current,
    Expression Root,
    IJsonTypeDescriptor Descriptor
);
