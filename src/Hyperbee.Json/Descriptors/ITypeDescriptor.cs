﻿using Hyperbee.Json.Filters;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Descriptors;

public delegate FilterExtensionFunction FunctionCreator();

public interface ITypeDescriptor
{
    public FunctionRegistry Functions { get; }
}

public interface ITypeDescriptor<TNode> : ITypeDescriptor
{
    public IValueAccessor<TNode> Accessor { get; }
    public IFilterEvaluator<TNode> FilterEvaluator { get; }

    public INodeTypeComparer Comparer { get; }
    bool CanUsePointer { get; }

    public void Deconstruct( out IValueAccessor<TNode> valueAccessor, out IFilterEvaluator<TNode> filterEvaluator )
    {
        valueAccessor = Accessor;
        filterEvaluator = FilterEvaluator;
    }
}
