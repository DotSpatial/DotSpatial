// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite, which is a C# port of the Java Topology Suite.
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;

namespace DotSpatial.Topology.Utilities
{
    /// <summary>
    /// Executes a transformation function on each element of a collection
    /// and returns the results in a new List.
    /// </summary>
    public class CollectionUtil
    {
        #region Delegates

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public delegate T FunctionDelegate<T>(T obj);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public delegate TResult FunctionDelegate< T,  TResult>(T obj);

        #endregion

        #region Methods

        /// <summary>
        /// Executes a function on each item in a <see cref="ICollection" /> 
        /// but does not accumulate the result.
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="func"></param>
        public static void Apply(ICollection coll, FunctionDelegate<object> func)
        {
            foreach (object obj in coll)
                func(obj);
        }

        /// <summary>
        /// Executes a function on each item in a <see cref="IEnumerable{T}" /> 
        /// but does not accumulate the result.
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="func"></param>
        public static void Apply<T>(IEnumerable<T> coll, FunctionDelegate<T> func)
        {
            foreach (var obj in coll)
                func(obj);
        }

        /// <summary>
        /// Executes a function on each item in a <see cref="ICollection" />
        /// and returns the results in a new <see cref="IList" />.
        /// </summary>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static IList<T> Cast<T>(ICollection coll)
        {
            IList<T> result = new List<T>(coll.Count);
            foreach (var obj in coll)
                result.Add((T)obj);
            return result;
        }

        /// <summary>
        /// Executes a function on each item in a <see cref="ICollection{TIn}" />
        /// and returns the results in a new <see cref="IList{TOut}" />.
        /// </summary>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static IList<TOut> Cast<TIn, TOut>(ICollection<TIn> coll)
            where TIn: class
            where TOut : class
        {
            IList<TOut> result = new List<TOut>(coll.Count);
            foreach (var obj in coll)
                result.Add(obj as TOut);
            return result;
        }

        /// <summary>
        /// Copies <typeparamref name="T"/>s in an array to an object array
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="array">the source array</param>
        /// <returns>An array of objects</returns>
        public static TOut[] Cast<TIn,TOut>(TIn[] array)
        {
            var res = new TOut[array.Length];
            Array.Copy(array, res, array.Length);
            return res;
        }

        /// <summary>
        /// Executes a function on each item in a <see cref="ICollection" />
        /// and collects all the entries for which the result
        /// of the function is equal to <c>true</c>.
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IList Select(ICollection coll, FunctionDelegate<object, bool> func)
        {
            IList result = new ArrayList();            
            foreach (object obj in coll)
                if (func(obj))
                    result.Add(obj);                            
            return result;
        }

        /// <summary>
        /// Executes a function on each item in a <see cref="ICollection" />
        /// and collects all the entries for which the result
        /// of the function is equal to <c>true</c>.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IList<T> Select<T>(IEnumerable<T> items, FunctionDelegate<T, bool> func)
        {
            IList<T> result = new List<T>();
            foreach (var obj in items)
                if (func(obj)) result.Add(obj);
            return result;
        }

        /// <summary>
        /// Executes a function on each item in a <see cref="ICollection" />
        /// and returns the results in a new <see cref="IList" />.
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IList Transform(ICollection coll, FunctionDelegate<object> func)
        {
            IList result = new ArrayList();
            foreach(object obj in coll)           
                result.Add(func(obj));
            return result;
        }

        /// <summary>
        /// Executes a function on each item in a <see cref="IList{T}" />
        /// and returns the results in a new <see cref="IList{T}" />.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static IList<T> Transform<T>(IList<T> list, FunctionDelegate<T> function)
        {
            IList<T> result = new List<T>(list.Count);
            foreach (T item in list)
                result.Add(function(item));
            return result;
        }

        /// <summary>
        /// Executes a function on each item in a <see cref="IList{T}" />
        /// and returns the results in a new <see cref="IList{TResult}" />.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static IList<TResult> Transform<T, TResult>(IList<T> list, FunctionDelegate<T, TResult> function)
        {
            IList<TResult> result = new List<TResult>(list.Count);
            foreach (T item in list)
                result.Add(function(item));
            return result;
        }

        #endregion
    }
}