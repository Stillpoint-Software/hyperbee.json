using System.Linq.Expressions;
using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Filters.Parser;

internal record FilterContext(
    Expression Current,
    Expression Root,
    FilterExpressionFactory SelectFactory,
    ITypeDescriptor Descriptor
);
