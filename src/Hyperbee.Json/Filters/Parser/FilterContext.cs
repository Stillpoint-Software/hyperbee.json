using System.Linq.Expressions;
using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Filters.Parser;

public record FilterContext(
    Expression Current,
    Expression Root,
    ITypeDescriptor Descriptor
);
