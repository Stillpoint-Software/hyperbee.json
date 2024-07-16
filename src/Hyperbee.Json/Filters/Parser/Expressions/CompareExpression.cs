using System.Linq.Expressions;
using System.Reflection;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Filters.Parser.Expressions;

public static class CompareExpression<TNode>
{
    // Expressions

    public static Expression Equal( Expression left, Expression right, Expression comparer ) 
        => Expression.Call( AreEqualMethod, left, right, comparer );

    public static Expression NotEqual( Expression left, Expression right, Expression comparer ) 
        => Expression.Call( AreNotEqualMethod, left, right, comparer );
 
    public static Expression LessThan( Expression left, Expression right, Expression comparer ) 
        => Expression.Call( IsLessThanMethod, left, right, comparer );

    public static Expression LessThanOrEqual( Expression left, Expression right, Expression comparer ) 
        => Expression.Call( IsLessThanOrEqualMethod, left, right, comparer );
 
    public static Expression GreaterThan( Expression left, Expression right, Expression comparer ) 
        => Expression.Call( IsGreaterThanMethod, left, right, comparer );

    public static Expression GreaterThanOrEqual( Expression left, Expression right, Expression comparer ) 
        => Expression.Call( IsGreaterThanOrEqualMethod, left, right, comparer );

    public static Expression And( Expression left, Expression right, Expression comparer ) 
        => Expression.Call( AndAlsoMethod, left, right, comparer );
    
    public static Expression Or( Expression left, Expression right, Expression comparer ) 
        => Expression.Call( OrElseMethod, left, right, comparer );
    
    public static Expression Not( Expression expression, Expression comparer ) 
        => Expression.Call( NotMethod, expression, comparer );

    // MethodInfo

    private const BindingFlags BindingAttr = BindingFlags.Static | BindingFlags.NonPublic;

    private static readonly MethodInfo AreEqualMethod = typeof(CompareExpression<TNode>).GetMethod( nameof(AreEqual), BindingAttr );
    private static readonly MethodInfo AreNotEqualMethod = typeof(CompareExpression<TNode>).GetMethod( nameof(AreNotEqual), BindingAttr );
    private static readonly MethodInfo IsLessThanMethod = typeof(CompareExpression<TNode>).GetMethod( nameof(IsLessThan), BindingAttr );
    private static readonly MethodInfo IsLessThanOrEqualMethod = typeof(CompareExpression<TNode>).GetMethod( nameof(IsLessThanOrEqual), BindingAttr );
    private static readonly MethodInfo IsGreaterThanMethod = typeof(CompareExpression<TNode>).GetMethod( nameof(IsGreaterThan), BindingAttr );
    private static readonly MethodInfo IsGreaterThanOrEqualMethod = typeof(CompareExpression<TNode>).GetMethod( nameof(IsGreaterThanOrEqual), BindingAttr );
    private static readonly MethodInfo AndAlsoMethod = typeof(CompareExpression<TNode>).GetMethod( nameof(AndAlso), BindingAttr );
    private static readonly MethodInfo OrElseMethod = typeof(CompareExpression<TNode>).GetMethod( nameof(OrElse), BindingAttr );
    private static readonly MethodInfo NotMethod = typeof(CompareExpression<TNode>).GetMethod( nameof(NotBoolean), BindingAttr );

    // Methods

    private static bool AreEqual( IValueType left, IValueType right, IValueTypeComparer comparer )
    {
        return comparer.Compare( left, right, Operator.Equals ) == 0;
    }

    private static bool AreNotEqual( IValueType left, IValueType right, IValueTypeComparer comparer )
    {
        return comparer.Compare( left, right, Operator.NotEquals ) != 0;
    }

    private static bool IsLessThan( IValueType left, IValueType right, IValueTypeComparer comparer )
    {
        return comparer.Compare( left, right, Operator.LessThan ) < 0;
    }

    private static bool IsLessThanOrEqual( IValueType left, IValueType right, IValueTypeComparer comparer )
    {
        return comparer.Compare( left, right, Operator.LessThanOrEqual ) <= 0;
    }

    private static bool IsGreaterThan( IValueType left, IValueType right, IValueTypeComparer comparer )
    {
        return comparer.Compare( left, right, Operator.GreaterThan ) > 0;
    }

    private static bool IsGreaterThanOrEqual( IValueType left, IValueType right, IValueTypeComparer comparer )
    {
        return comparer.Compare( left, right, Operator.GreaterThanOrEqual ) >= 0;
    }

    private static bool AndAlso( IValueType left, IValueType right, IValueTypeComparer comparer )
    {
        if ( left is ScalarValue<bool> leftBoolValue && right is ScalarValue<bool> rightBoolValue )
            return leftBoolValue.Value && rightBoolValue.Value;

        return comparer.Exists( left ) && comparer.Exists( right );
    }

    private static bool OrElse( IValueType left, IValueType right, IValueTypeComparer comparer )
    {
        if ( left is ScalarValue<bool> leftBoolValue && right is ScalarValue<bool> rightBoolValue )
            return leftBoolValue.Value || rightBoolValue.Value;

        return comparer.Exists( left ) || comparer.Exists( right );
    }

    private static bool NotBoolean( IValueType value, IValueTypeComparer comparer )
    {
        if ( value is ScalarValue<bool> { Value: false } )
            return true;

        return !comparer.Exists( value );
    }
}
