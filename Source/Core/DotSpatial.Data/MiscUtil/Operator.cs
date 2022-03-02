// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data.MiscUtil
{
    // Required Attribution under Creative Commons Attribution-Noncommercial-Share Alike 3.0 United States License;
    // http://www.codeproject.com/KB/cs/GenericArithmeticUtil.aspx

    /// <summary>
    /// The Operator class provides easy access to the standard operators
    /// (addition, etc) for generic types, using type inference to simplify usage.
    /// </summary>
    public static class Operator
    {
        #region Methods

        /// <summary>
        /// Evaluates binary addition (+) for the given type; this will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <typeparam name="T">The type of both values.</typeparam>
        /// <returns>The result of the operation.</returns>
        public static T Add<T>(T value1, T value2)
        {
            return Operator<T>.Add(value1, value2);
        }

        /// <summary>
        /// Evaluates binary addition (+) for the given type(s); this will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <typeparam name="TArg1">Type of the first value.</typeparam>
        /// <typeparam name="TArg2">Type of the second value.</typeparam>
        /// <returns>The result of the operation in the type of the first argument.</returns>
        public static TArg1 AddAlternative<TArg1, TArg2>(TArg1 value1, TArg2 value2)
        {
            return Operator<TArg2, TArg1>.Add(value1, value2);
        }

        /// <summary>
        /// Increments the accumulator only if the value is non-null. If the accumulator is null,
        /// then the accumulator is given the new value; otherwise the accumulator and value are added.
        /// </summary>
        /// <param name="accumulator">The current total to be incremented (can be null).</param>
        /// <param name="value">The value to be tested and added to the accumulator.</param>
        /// <typeparam name="T">Type of the objects.</typeparam>
        /// <returns>True if the value is non-null, else false - i.e. "has the accumulator been updated?".</returns>
        public static bool AddIfNotNull<T>(ref T accumulator, T value)
        {
            return Operator<T>.NullOp.AddIfNotNull(ref accumulator, value);
        }

        /// <summary>
        /// Evaluates bitwise and (&amp;) for the given type; this will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <typeparam name="T">The type of both values.</typeparam>
        /// <returns>The result of the operation.</returns>
        public static T And<T>(T value1, T value2)
        {
            return Operator<T>.And(value1, value2);
        }

        /// <summary>
        /// Performs a conversion between the given types; this will throw
        /// an InvalidOperationException if the type T does not provide a suitable cast, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this cast.
        /// </summary>
        /// <typeparam name="TFrom">Type of the value.</typeparam>
        /// <typeparam name="TTo">Type of the result.</typeparam>
        /// <param name="value">The value that gets converted from its own type to the new type.</param>
        /// <returns>The value with the new type.</returns>
        public static TTo Convert<TFrom, TTo>(TFrom value)
        {
            return Operator<TFrom, TTo>.Convert(value);
        }

        /// <summary>
        /// Evaluates binary division (/) for the given type; this will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <typeparam name="T">The type of both values.</typeparam>
        /// <returns>The result of the operation.</returns>
        public static T Divide<T>(T value1, T value2)
        {
            return Operator<T>.Divide(value1, value2);
        }

        /// <summary>
        /// Evaluates binary division (/) for the given type(s); this will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <typeparam name="TArg1">Type of the first value.</typeparam>
        /// <typeparam name="TArg2">Type of the second value.</typeparam>
        /// <returns>The result of the operation in the type of the first argument.</returns>
        public static TArg1 DivideAlternative<TArg1, TArg2>(TArg1 value1, TArg2 value2)
        {
            return Operator<TArg2, TArg1>.Divide(value1, value2);
        }

        /// <summary>
        /// Evaluates integer division (/) for the given type; this will throw an InvalidOperationException if the
        /// type T does not provide this operator, or for Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        /// <param name="value">The value that gets devided.</param>
        /// <param name="divisor">The divisor.</param>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <returns>The result in the type of the value.</returns>
        /// <remarks>This operation is particularly useful for computing averages and similar aggregates.</remarks>
        public static T DivideInt32<T>(T value, int divisor)
        {
            return Operator<int, T>.Divide(value, divisor);
        }

        /// <summary>
        /// Evaluates binary equality (==) for the given type; this will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <typeparam name="T">The type of both values.</typeparam>
        /// <returns>True if the objects are equal.</returns>
        public static bool Equal<T>(T value1, T value2)
        {
            return Operator<T>.Equal(value1, value2);
        }

        /// <summary>
        /// Evaluates binary greater-than (&gt;) for the given type; this will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <typeparam name="T">The type of both values.</typeparam>
        /// <returns>True if the first value is greater than the second.</returns>
        public static bool GreaterThan<T>(T value1, T value2)
        {
            return Operator<T>.GreaterThan(value1, value2);
        }

        /// <summary>
        /// Evaluates binary greater-than-on-eqauls (&gt;=) for the given type; this will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <typeparam name="T">The type of both values.</typeparam>
        /// <returns>True if the first value is greater than or equal to the second.</returns>
        public static bool GreaterThanOrEqual<T>(T value1, T value2)
        {
            return Operator<T>.GreaterThanOrEqual(value1, value2);
        }

        /// <summary>
        /// Indicates if the supplied value is non-null, for reference-types or Nullable&lt;T&gt;.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <returns>True for non-null values, else false.</returns>
        public static bool HasValue<T>(T value)
        {
            return Operator<T>.NullOp.HasValue(value);
        }

        /// <summary>
        /// Evaluates binary less-than (&lt;) for the given type; this will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <typeparam name="T">The type of both values.</typeparam>
        /// <returns>True if the first value is less than to the second.</returns>
        public static bool LessThan<T>(T value1, T value2)
        {
            return Operator<T>.LessThan(value1, value2);
        }

        /// <summary>
        /// Evaluates binary less-than-or-equal (&lt;=) for the given type; this will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <typeparam name="T">The type of both values.</typeparam>
        /// <returns>True if the first value is less than or equal to the second.</returns>
        public static bool LessThanOrEqual<T>(T value1, T value2)
        {
            return Operator<T>.LessThanOrEqual(value1, value2);
        }

        /// <summary>
        /// Evaluates binary multiplication (*) for the given type; this will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <typeparam name="T">The type of both values.</typeparam>
        /// <returns>The result of the operation.</returns>
        public static T Multiply<T>(T value1, T value2)
        {
            return Operator<T>.Multiply(value1, value2);
        }

        /// <summary>
        /// Evaluates binary multiplication (*) for the given type(s); this will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <typeparam name="TArg1">Type of the first value.</typeparam>
        /// <typeparam name="TArg2">Type of the second value.</typeparam>
        /// <returns>The result of the operation in the type of the first argument.</returns>
        public static TArg1 MultiplyAlternative<TArg1, TArg2>(TArg1 value1, TArg2 value2)
        {
            return Operator<TArg2, TArg1>.Multiply(value1, value2);
        }

        /// <summary>
        /// Evaluates unary negation (-) for the given type; this will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        /// <param name="value">Value that gets negated.</param>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <returns>The result of the negation.</returns>
        public static T Negate<T>(T value)
        {
            return Operator<T>.Negate(value);
        }

        /// <summary>
        /// Evaluates bitwise not (~) for the given type; this will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <returns>The result of the evaluation.</returns>
        public static T Not<T>(T value)
        {
            return Operator<T>.Not(value);
        }

        /// <summary>
        /// Evaluates binary inequality (!=) for the given type; this will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <typeparam name="T">The type of both values.</typeparam>
        /// <returns>True if the values are not equal.</returns>
        public static bool NotEqual<T>(T value1, T value2)
        {
            return Operator<T>.NotEqual(value1, value2);
        }

        /// <summary>
        /// Evaluates bitwise or (|) for the given type; this will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <typeparam name="T">The type of both values.</typeparam>
        /// <returns>The result of the evaluation.</returns>
        public static T Or<T>(T value1, T value2)
        {
            return Operator<T>.Or(value1, value2);
        }

        /// <summary>
        /// Evaluates binary subtraction (-) for the given type; this will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <typeparam name="T">The type of both values.</typeparam>
        /// <returns>The result of the operation.</returns>
        public static T Subtract<T>(T value1, T value2)
        {
            return Operator<T>.Subtract(value1, value2);
        }

        /// <summary>
        /// Evaluates binary subtraction(-) for the given type(s); this will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <typeparam name="TArg1">Type of the first value.</typeparam>
        /// <typeparam name="TArg2">Type of the second value.</typeparam>
        /// <returns>The result of the operation in the type of the first argument.</returns>
        public static TArg1 SubtractAlternative<TArg1, TArg2>(TArg1 value1, TArg2 value2)
        {
            return Operator<TArg2, TArg1>.Subtract(value1, value2);
        }

        /// <summary>
        /// Evaluates bitwise xor (^) for the given type; this will throw
        /// an InvalidOperationException if the type T does not provide this operator, or for
        /// Nullable&lt;TInner&gt; if TInner does not provide this operator.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <typeparam name="T">The type of both values.</typeparam>
        /// <returns>The result of the evaluation.</returns>
        public static T Xor<T>(T value1, T value2)
        {
            return Operator<T>.Xor(value1, value2);
        }

        #endregion
    }
}