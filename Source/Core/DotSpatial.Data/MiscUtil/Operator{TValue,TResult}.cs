// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Linq.Expressions;

namespace DotSpatial.Data.MiscUtil
{
    /// <summary>
    /// Provides standard operators (such as addition) that operate over operands of different types.
    /// For operators, the return type is assumed to match the first operand.
    /// </summary>
    /// <typeparam name="TValue">Type of the value.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    /// <seealso cref="Operator&lt;T&gt;"/>
    /// <seealso cref="Operator"/>
    public static class Operator<TValue, TResult>
    {
        #region Constructors

        static Operator()
        {
            Convert = ExpressionUtil.CreateExpression<TValue, TResult>(body => Expression.Convert(body, typeof(TResult)));
            Add = ExpressionUtil.CreateExpression<TResult, TValue, TResult>(Expression.Add, true);
            Subtract = ExpressionUtil.CreateExpression<TResult, TValue, TResult>(Expression.Subtract, true);
            Multiply = ExpressionUtil.CreateExpression<TResult, TValue, TResult>(Expression.Multiply, true);
            Divide = ExpressionUtil.CreateExpression<TResult, TValue, TResult>(Expression.Divide, true);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a delegate to evaluate binary addition (+) for the given types; this delegate will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        public static Func<TResult, TValue, TResult> Add { get; }

        /// <summary>
        /// Gets a delegate to convert a value between two types; this delegate will throw
        /// an InvalidOperationException if the type T does not provide a suitable cast, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this cast.
        /// </summary>
        public static Func<TValue, TResult> Convert { get; }

        /// <summary>
        /// Gets a delegate to evaluate binary division (/) for the given types; this delegate will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        public static Func<TResult, TValue, TResult> Divide { get; }

        /// <summary>
        /// Gets a delegate to evaluate binary multiplication (*) for the given types; this delegate will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        public static Func<TResult, TValue, TResult> Multiply { get; }

        /// <summary>
        /// Gets a delegate to evaluate binary subtraction (-) for the given types; this delegate will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        public static Func<TResult, TValue, TResult> Subtract { get; }

        #endregion
    }
}