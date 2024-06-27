﻿using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal interface IExpressionFactory
{
    static abstract bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, FilterContext<TNode> context );
}