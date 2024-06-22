using System.Linq.Expressions;
using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Filters.Parser;

public record FilterExecutionContext(
    Expression Current,
    Expression Root,
    ITypeDescriptor Descriptor
);
