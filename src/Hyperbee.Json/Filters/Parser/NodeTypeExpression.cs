using System.Linq.Expressions;
using System.Reflection;
using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Filters.Parser;

public static class NodeTypeComparerBinderExpression<TNode>
{
    private static readonly Expression BindComparerExpressionConst = Expression.Constant( (Func<FilterParserContext<TNode>, INodeType, INodeType>) BindComparer );
    internal static Expression BindComparerExpression( FilterParserContext<TNode> parserContext, Expression expression )
    {
        if ( expression == null )
            return null;

        var parserContextExp = Expression.Constant( parserContext );

        return Expression.Invoke( BindComparerExpressionConst, parserContextExp,
            Expression.Convert( expression, typeof( INodeType ) ) );
    }

    internal static INodeType BindComparer( FilterParserContext<TNode> parserContext, INodeType item )
    {
        item.SetComparer( parserContext.Descriptor.Comparer );
        return item;
    }

}

public static class NodeTypeExpression<TNode>
{
    private static readonly MethodInfo AreEqualMethodInfo = typeof( NodeTypeExpression<TNode> ).GetMethod( nameof( AreEqual ) );
    private static readonly MethodInfo AreNotEqualMethodInfo = typeof( NodeTypeExpression<TNode> ).GetMethod( nameof( AreNotEqual ) );
    private static readonly MethodInfo IsLessThanMethodInfo = typeof( NodeTypeExpression<TNode> ).GetMethod( nameof( IsLessThan ) );
    private static readonly MethodInfo IsLessThanOrEqualMethodInfo = typeof( NodeTypeExpression<TNode> ).GetMethod( nameof( IsLessThanOrEqual ) );
    private static readonly MethodInfo IsGreaterThanMethodInfo = typeof( NodeTypeExpression<TNode> ).GetMethod( nameof( IsGreaterThan ) );
    private static readonly MethodInfo IsGreaterThanOrEqualMethodInfo = typeof( NodeTypeExpression<TNode> ).GetMethod( nameof( IsGreaterThanOrEqual ) );

    public static Expression Equal( Expression left, Expression right ) => Expression.Call( AreEqualMethodInfo, left, right );
    public static Expression NotEqual( Expression left, Expression right ) => Expression.Call( AreNotEqualMethodInfo, left, right );
    public static Expression LessThan( Expression left, Expression right ) => Expression.Call( IsLessThanMethodInfo, left, right );
    public static Expression LessThanOrEqual( Expression left, Expression right ) => Expression.Call( IsLessThanOrEqualMethodInfo, left, right );
    public static Expression GreaterThan( Expression left, Expression right ) => Expression.Call( IsGreaterThanMethodInfo, left, right );
    public static Expression GreaterThanOrEqual( Expression left, Expression right ) => Expression.Call( IsGreaterThanOrEqualMethodInfo, left, right );


    public static bool AreEqual( INodeType left, INodeType right )
    {
        return left switch
        {
            // Symmetric compares
            ValueType<string> leftNode when right is ValueType<string> rightNode => leftNode.Value == rightNode.Value,
            ValueType<bool> leftNode when right is ValueType<bool> rightNode => leftNode.Value == rightNode.Value,
            ValueType<float> leftNode when right is ValueType<float> rightNode => leftNode == rightNode, // TODO: should be pass through for tolerance check
            NodesType<TNode> leftNode when right is NodesType<TNode> rightNode => leftNode == rightNode,
            Nothing leftNode when right is Nothing rightNode => true,
            Null leftNode when right is Null rightNode => true,

            // Asymmetric compares
            ValueType<string> leftNode when right is NodesType<TNode> rightNode => leftNode == rightNode,
            ValueType<bool> leftNode when right is NodesType<TNode> rightNode => leftNode == rightNode,
            ValueType<float> leftNode when right is NodesType<TNode> rightNode => leftNode == rightNode,
            NodesType<TNode> leftNode => leftNode == right,

            _ => false
        };
    }

    public static bool AreNotEqual( INodeType left, INodeType right )
    {
        return !AreEqual( left, right );
    }

    public static bool IsLessThan( INodeType left, INodeType right )
    {
        return left switch
        {
            // Direct compare
            ValueType<string> leftNode when right is ValueType<string> rightNode => leftNode.Value == rightNode.Value,
            ValueType<bool> leftNode when right is ValueType<bool> rightNode => leftNode.Value == rightNode.Value,
            ValueType<float> leftNode when right is ValueType<float> rightNode => leftNode < rightNode, // TODO: should be pass through for tolerance check
            NodesType<TNode> leftNode when right is NodesType<TNode> rightNode => leftNode < rightNode,

            // Asymmetric compares
            ValueType<string> leftNode when right is NodesType<TNode> rightNode => leftNode < rightNode,
            ValueType<bool> leftNode when right is NodesType<TNode> rightNode => leftNode < rightNode,
            ValueType<float> leftNode when right is NodesType<TNode> rightNode => leftNode < rightNode,
            NodesType<TNode> leftNode => leftNode < right,

            _ => false
        };
    }

    public static bool IsGreaterThan( INodeType left, INodeType right )
    {
        return left switch
        {
            // Direct compare
            ValueType<string> leftNode when right is ValueType<string> rightNode => leftNode.Value == rightNode.Value,
            ValueType<bool> leftNode when right is ValueType<bool> rightNode => leftNode.Value == rightNode.Value,
            ValueType<float> leftNode when right is ValueType<float> rightNode => leftNode > rightNode, // TODO: should be pass through for tolerance check
            NodesType<TNode> leftNode when right is NodesType<TNode> rightNode => leftNode > rightNode,

            // Asymmetric compares
            ValueType<string> leftNode when right is NodesType<TNode> rightNode => leftNode > rightNode,
            ValueType<bool> leftNode when right is NodesType<TNode> rightNode => leftNode > rightNode,
            ValueType<float> leftNode when right is NodesType<TNode> rightNode => leftNode > rightNode,
            NodesType<TNode> leftNode => leftNode > right,

            _ => false
        };
    }

    public static bool IsLessThanOrEqual( INodeType left, INodeType right )
    {
        return left switch
        {
            // Direct compare
            ValueType<string> leftNode when right is ValueType<string> rightNode => leftNode.Value == rightNode.Value,
            ValueType<bool> leftNode when right is ValueType<bool> rightNode => leftNode.Value == rightNode.Value,
            ValueType<float> leftNode when right is ValueType<float> rightNode => leftNode <= rightNode, // TODO: should be pass through for tolerance check
            NodesType<TNode> leftNode when right is NodesType<TNode> rightNode => leftNode <= rightNode,

            // Asymmetric compares
            ValueType<string> leftNode when right is NodesType<TNode> rightNode => leftNode <= rightNode,
            ValueType<bool> leftNode when right is NodesType<TNode> rightNode => leftNode <= rightNode,
            ValueType<float> leftNode when right is NodesType<TNode> rightNode => leftNode <= rightNode,
            NodesType<TNode> leftNode => leftNode <= right,

            _ => false
        };
    }

    public static bool IsGreaterThanOrEqual( INodeType left, INodeType right )
    {
        return left switch
        {
            // Direct compare
            ValueType<string> leftNode when right is ValueType<string> rightNode => leftNode.Value == rightNode.Value,
            ValueType<bool> leftNode when right is ValueType<bool> rightNode => leftNode.Value == rightNode.Value,
            ValueType<float> leftNode when right is ValueType<float> rightNode => leftNode >= rightNode, // TODO: should be pass through for tolerance check
            NodesType<TNode> leftNode when right is NodesType<TNode> rightNode => leftNode >= rightNode,

            // Asymmetric compares
            ValueType<string> leftNode when right is NodesType<TNode> rightNode => leftNode >= rightNode,
            ValueType<bool> leftNode when right is NodesType<TNode> rightNode => leftNode >= rightNode,
            ValueType<float> leftNode when right is NodesType<TNode> rightNode => leftNode >= rightNode,
            NodesType<TNode> leftNode => leftNode >= right,

            _ => false
        };
    }
}
