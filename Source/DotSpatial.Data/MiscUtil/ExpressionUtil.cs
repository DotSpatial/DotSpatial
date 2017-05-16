﻿// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Linq.Expressions;

namespace DotSpatial.Data.MiscUtil
{
    // Required Attribution under Creative Commons Attribution-Noncommercial-Share Alike 3.0 United States License;
    // http://www.codeproject.com/KB/cs/GenericArithmeticUtil.aspx

    /// <summary>
    /// General purpose Expression utilities
    /// </summary>
    public static class ExpressionUtil
    {
        #region Methods

        /// <summary>
        /// Create a function delegate representing a unary operation
        /// </summary>
        /// <typeparam name="TArg1">The parameter type</typeparam>
        /// <typeparam name="TResult">The return type</typeparam>
        /// <param name="body">Body factory</param>
        /// <returns>Compiled function delegate</returns>
        public static Func<TArg1, TResult> CreateExpression<TArg1, TResult>(Func<Expression, UnaryExpression> body)
        {
            ParameterExpression inp = Expression.Parameter(typeof(TArg1), "inp");
            try
            {
                return Expression.Lambda<Func<TArg1, TResult>>(body(inp), inp).Compile();
            }
            catch (Exception ex)
            {
                string msg = ex.Message; // avoid capture of ex itself
                return arg => { throw new InvalidOperationException(msg); };
            }
        }

        /// <summary>
        /// Create a function delegate representing a binary operation
        /// </summary>
        /// <typeparam name="TArg1">The first parameter type</typeparam>
        /// <typeparam name="TArg2">The second parameter type</typeparam>
        /// <typeparam name="TResult">The return type</typeparam>
        /// <param name="body">Body factory</param>
        /// <returns>Compiled function delegate</returns>
        public static Func<TArg1, TArg2, TResult> CreateExpression<TArg1, TArg2, TResult>(Func<Expression, Expression, BinaryExpression> body)
        {
            return CreateExpression<TArg1, TArg2, TResult>(body, false);
        }

        /// <summary>
        /// Create a function delegate representing a binary operation
        /// </summary>
        /// <typeparam name="TArg1">The first parameter type</typeparam>
        /// <typeparam name="TArg2">The second parameter type</typeparam>
        /// <typeparam name="TResult">The return type</typeparam>
        /// <param name="body">Body factory</param>
        /// <param name="castArgsToResultOnFailure">If no matching operation is possible, attempt to convert
        /// TArg1 and TArg2 to TResult for a match? For example, there is no
        /// "decimal operator /(decimal, int)", but by converting TArg2 (int) to
        /// TResult (decimal) a match is found.</param>
        /// <returns>
        /// Compiled function delegate
        /// </returns>
        public static Func<TArg1, TArg2, TResult> CreateExpression<TArg1, TArg2, TResult>(Func<Expression, Expression, BinaryExpression> body, bool castArgsToResultOnFailure)
        {
            ParameterExpression lhs = Expression.Parameter(typeof(TArg1), "lhs");
            ParameterExpression rhs = Expression.Parameter(typeof(TArg2), "rhs");
            try
            {
                try
                {
                    return Expression.Lambda<Func<TArg1, TArg2, TResult>>(body(lhs, rhs), lhs, rhs).Compile();
                }
                catch (InvalidOperationException)
                {
                    if (castArgsToResultOnFailure && !( // if we show retry
                                                          typeof(TArg1) == typeof(TResult) && // and the args aren't
                                                          typeof(TArg2) == typeof(TResult)))
                    {
                        // already "TValue, TValue, TValue"...
                        // convert both lhs and rhs to TResult (as appropriate)
                        Expression castLhs = typeof(TArg1) == typeof(TResult) ? lhs : (Expression)Expression.Convert(lhs, typeof(TResult));
                        Expression castRhs = typeof(TArg2) == typeof(TResult) ? rhs : (Expression)Expression.Convert(rhs, typeof(TResult));

                        return Expression.Lambda<Func<TArg1, TArg2, TResult>>(body(castLhs, castRhs), lhs, rhs).Compile();
                    }

                    throw;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message; // avoid capture of ex itself
                return (arg1, arg2) => { throw new InvalidOperationException(msg); };
            }
        }

        #endregion
    }
}