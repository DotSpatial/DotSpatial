using System;
using System.Linq.Expressions;

namespace DotSpatial.Data.MiscUtil
{
    /// <summary>
    /// Provides standard operators (such as addition) over a single type
    /// </summary>
    /// <typeparam name="T">The type of the operator.</typeparam>
    /// <seealso cref="Operator"/>
    /// <seealso cref="Operator&lt;TValue,TResult&gt;"/>
    public static class Operator<T>
    {
        #region Constructors

        static Operator()
        {
            Add = ExpressionUtil.CreateExpression<T, T, T>(Expression.Add);
            Subtract = ExpressionUtil.CreateExpression<T, T, T>(Expression.Subtract);
            Divide = ExpressionUtil.CreateExpression<T, T, T>(Expression.Divide);
            Multiply = ExpressionUtil.CreateExpression<T, T, T>(Expression.Multiply);

            GreaterThan = ExpressionUtil.CreateExpression<T, T, bool>(Expression.GreaterThan);
            GreaterThanOrEqual = ExpressionUtil.CreateExpression<T, T, bool>(Expression.GreaterThanOrEqual);
            LessThan = ExpressionUtil.CreateExpression<T, T, bool>(Expression.LessThan);
            LessThanOrEqual = ExpressionUtil.CreateExpression<T, T, bool>(Expression.LessThanOrEqual);
            Equal = ExpressionUtil.CreateExpression<T, T, bool>(Expression.Equal);
            NotEqual = ExpressionUtil.CreateExpression<T, T, bool>(Expression.NotEqual);

            Negate = ExpressionUtil.CreateExpression<T, T>(Expression.Negate);
            And = ExpressionUtil.CreateExpression<T, T, T>(Expression.And);
            Or = ExpressionUtil.CreateExpression<T, T, T>(Expression.Or);
            Not = ExpressionUtil.CreateExpression<T, T>(Expression.Not);
            Xor = ExpressionUtil.CreateExpression<T, T, T>(Expression.ExclusiveOr);

            Type typeT = typeof(T);
            if (typeT.IsValueType && typeT.IsGenericType && (typeT.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                // get the *inner* zero (not a null Nullable<TValue>, but default(TValue))
                Type nullType = typeT.GetGenericArguments()[0];
                Zero = (T)Activator.CreateInstance(nullType);
                NullOp = (INullOp<T>)Activator.CreateInstance(typeof(StructNullOp<>).MakeGenericType(nullType));
            }
            else
            {
                Zero = default(T);
                if (typeT.IsValueType)
                {
                    NullOp = (INullOp<T>)Activator.CreateInstance(typeof(StructNullOp<>).MakeGenericType(typeT));
                }
                else
                {
                    NullOp = (INullOp<T>)Activator.CreateInstance(typeof(ClassNullOp<>).MakeGenericType(typeT));
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a delegate to evaluate binary addition (+) for the given type; this delegate will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        public static Func<T, T, T> Add { get; }

        /// <summary>
        /// Gets a delegate to evaluate bitwise and (&amp;) for the given type; this delegate will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        public static Func<T, T, T> And { get; }

        /// <summary>
        /// Gets a delegate to evaluate binary division (/) for the given type; this delegate will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        public static Func<T, T, T> Divide { get; }

        /// <summary>
        /// Gets a delegate to evaluate binary equality (==) for the given type; this delegate will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        public static Func<T, T, bool> Equal { get; }

        /// <summary>
        /// Gets a delegate to evaluate binary greater-then (&gt;) for the given type; this delegate will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        public static Func<T, T, bool> GreaterThan { get; }

        /// <summary>
        /// Gets a delegate to evaluate binary greater-than-or-equal (&gt;=) for the given type; this delegate will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        public static Func<T, T, bool> GreaterThanOrEqual { get; }

        /// <summary>
        /// Gets a delegate to evaluate binary less-than (&lt;) for the given type; this delegate will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        public static Func<T, T, bool> LessThan { get; }

        /// <summary>
        /// Gets a delegate to evaluate binary less-than-or-equal (&lt;=) for the given type; this delegate will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        public static Func<T, T, bool> LessThanOrEqual { get; }

        /// <summary>
        /// Gets a delegate to evaluate binary multiplication (*) for the given type; this delegate will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        public static Func<T, T, T> Multiply { get; }

        /// <summary>
        /// Gets a delegate to evaluate unary negation (-) for the given type; this delegate will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        public static Func<T, T> Negate { get; }

        /// <summary>
        /// Gets a delegate to evaluate bitwise not (~) for the given type; this delegate will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        public static Func<T, T> Not { get; }

        /// <summary>
        /// Gets a delegate to evaluate binary inequality (!=) for the given type; this delegate will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        public static Func<T, T, bool> NotEqual { get; }

        /// <summary>
        /// Gets a delegate to evaluate bitwise or (|) for the given type; this delegate will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        public static Func<T, T, T> Or { get; }

        /// <summary>
        /// Gets a delegate to evaluate binary subtraction (-) for the given type; this delegate will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        public static Func<T, T, T> Subtract { get; }

        /// <summary>
        /// Gets a delegate to evaluate bitwise xor (^) for the given type; this delegate will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        public static Func<T, T, T> Xor { get; }

        /// <summary>
        /// Gets the zero value for value-types (even full Nullable&lt;TInner&gt;) - or null for reference types
        /// </summary>
        public static T Zero { get; }

        /// <summary>
        /// Gets the NullOp.
        /// </summary>
        internal static INullOp<T> NullOp { get; }

        #endregion
    }
}