using System.Linq.Expressions;
using System.Reflection;
using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Filters.Parser.Expressions;

public static class MathExpression<TNode>
{
    // Expressions

    public static Expression Add( Expression left, Expression right )
        => Expression.Call( AddMethod, left, right );

    public static Expression Subtract( Expression left, Expression right )
        => Expression.Call( SubtractMethod, left, right );

    public static Expression Multiply( Expression left, Expression right )
        => Expression.Call( MultiplyMethod, left, right );

    public static Expression Divide( Expression left, Expression right )
        => Expression.Call( DivideMethod, left, right );

    // MethodInfo

    private const BindingFlags BindingAttr = BindingFlags.Static | BindingFlags.NonPublic;

    private static readonly MethodInfo AddMethod = typeof( CompareExpression<TNode> ).GetMethod( nameof( Add ), BindingAttr );
    private static readonly MethodInfo SubtractMethod = typeof( CompareExpression<TNode> ).GetMethod( nameof( Subtract ), BindingAttr );
    private static readonly MethodInfo MultiplyMethod = typeof( CompareExpression<TNode> ).GetMethod( nameof( Multiply ), BindingAttr );
    private static readonly MethodInfo DivideMethod = typeof( CompareExpression<TNode> ).GetMethod( nameof( Divide ), BindingAttr );

    // Methods

    private static IValueType Add( IValueType left, IValueType right )
    {
        return Scalar.Nothing;
    }

    private static IValueType Subtract( IValueType left, IValueType right )
    {
        return Scalar.Nothing;
    }

    private static IValueType Multiply( IValueType left, IValueType right )
    {
        return Scalar.Nothing;
    }

    private static IValueType Divide( IValueType left, IValueType right )
    {
        return Scalar.Nothing;
    }

    // Helpers

    private static bool IsFloatToIntOperation( IValueType left, IValueType right ) =>
        left is ScalarValue<int> && right is ScalarValue<float> || left is ScalarValue<float> && right is ScalarValue<int>;

    private static bool TryGetValueType( IValueAccessor<TNode> accessor, TNode node, out IValueType nodeType )
    {
        if ( accessor.TryGetValueFromNode( node, out var itemValue ) )
        {
            nodeType = itemValue switch
            {
                string itemString => Scalar.Value( itemString ),
                bool itemBool => Scalar.Value( itemBool ),
                float itemFloat => Scalar.Value( itemFloat ),
                int itemInt => Scalar.Value( itemInt ),
                null => Scalar.Null,
                _ => throw new NotSupportedException( "Unsupported value type." )
            };
            return true;
        }

        nodeType = null;
        return false;
    }
}
