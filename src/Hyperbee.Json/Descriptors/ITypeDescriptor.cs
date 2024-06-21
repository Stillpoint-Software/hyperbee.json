using System.Linq.Expressions;
using Hyperbee.Json.Filters;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors;

public delegate FilterExtensionFunction FunctionCreator( ParseExpressionContext context );

public interface IJsonTypeDescriptor
{
    public Dictionary<string, FunctionCreator> Functions { get; }

    public FilterFunction GetSelectFunction( ParseExpressionContext context );

    public Expression GetValueExpression( Expression context );
}

public interface ITypeDescriptor<TNode> : IJsonTypeDescriptor
{
    public IValueAccessor<TNode> Accessor { get; }
    public IFilterEvaluator<TNode> FilterEvaluator { get; }

    public void Deconstruct( out IValueAccessor<TNode> valueAccessor, out IFilterEvaluator<TNode> filterEvaluator )
    {
        valueAccessor = Accessor;
        filterEvaluator = FilterEvaluator;
    }
}
