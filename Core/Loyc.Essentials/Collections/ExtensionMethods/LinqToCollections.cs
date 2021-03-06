// Generated from LinqToCollections.ecs by LeMP custom tool. LeMP version: 1.7.5.0
// Note: you can give command-line arguments to the tool via 'Custom Tool Namespace':
// --no-out-header       Suppress this message
// --verbose             Allow verbose messages (shown by VS as 'warnings')
// --timeout=X           Abort processing thread after X seconds (default: 10)
// --macros=FileName.dll Load macros from FileName.dll, path relative to this file 
// Use #importMacros to use macros in a given namespace, e.g. #importMacros(Loyc.LLPG);
using System;
using System.Collections.Generic;
using System.Linq;
namespace Loyc.Collections
{
	/// <summary>
	/// This class enhances LINQ-to-Objects with extension methods that preserve the
	/// interface (e.g. Take(List&lt;int>) returns IList&lt;int>) and/or have higher
	/// performance than the ones in System.Linq.Enumerable.
	/// </summary><remarks>
	/// For example, the <see cref="Enumerable.Last(IEnumerable{T})"/> extension 
	/// method scans the entire list before returning the last item, while 
	/// <see cref="Last(IReadOnlyList{T})"/> and <see cref="Last(IList{T})"/> simply
	/// return the last item directly.
	/// </remarks>
	public static class LinqToCollections
	{
		public static int Count<T>(this IList<T> list)
		{
			return list.Count;
		}
		public static int Count<T>(this IReadOnlyCollection<T> list)
		{
			return list.Count;
		}
		public static int Count<T>(this INegListSource<T> list)
		{
			return list.Count;
		}

		public static T FirstOrDefault<T>(this IList<T> list, T defaultValue = default(T))
		{
			if (list.Count > 0)
				return list[0];
			return defaultValue;
		}
		public static T FirstOrDefault<T>(this IListAndListSource<T> list, T defaultValue = default(T))
		{
			return FirstOrDefault((IListSource<T>)list, defaultValue);
		}
		public static T FirstOrDefault<T>(this IListSource<T> list)
		{
			bool _;
			return list.TryGet(0, out _);
		}
		public static T FirstOrDefault<T>(this IListSource<T> list, T defaultValue)
		{
			bool fail;
			var result = list.TryGet(0, out fail);
			if (fail)
				return defaultValue;
			return result;
		}

		public static T Last<T>(this IList<T> list)
		{
			int last = list.Count - 1;
			if (last < 0)
				throw new EmptySequenceException();
			return list[last];
		}
		public static T LastOrDefault<T>(this IList<T> list, T defaultValue = default(T))
		{
			int last = list.Count - 1;
			return last < 0 ? defaultValue : list[last];
		}
		public static T Last<T>(this IReadOnlyList<T> list)
		{
			int last = list.Count - 1;
			if (last < 0)
				throw new EmptySequenceException();
			return list[last];
		}
		public static T LastOrDefault<T>(this IReadOnlyList<T> list, T defaultValue = default(T))
		{
			int last = list.Count - 1;
			return last < 0 ? defaultValue : list[last];
		}
		public static T Last<T>(this IListAndListSource<T> list) { return Last((IList<T>)list); }
		public static T LastOrDefault<T>(this IListAndListSource<T> list, T defaultValue = default(T))
			{ return LastOrDefault((IList<T>)list, defaultValue); }
		public static T Last<T>(this INegListSource<T> list)
		{
			int last = list.Max;
			if (last < list.Min)
				throw new EmptySequenceException();
			return list[last];
		}
		public static T LastOrDefault<T>(this INegListSource<T> list, T defaultValue = default(T))
		{
			int last = list.Max;
			return last < list.Min ? defaultValue : list[last];
		}

		public static IList<T> Skip<T>(this IList<T> list, int start)
		{
			return list.Slice(start);
		}
		public static IList<T> Take<T>(this IList<T> list, int count)
		{
			return list.Slice(0, count);
		}
		public static IListSource<T> Skip<T>(this IListSource<T> list, int start)
		{
			return list.Slice(start);
		}
		public static IListSource<T> Take<T>(this IListSource<T> list, int count)
		{
			return list.Slice(0, count);
		}
		public static IListSource<T> Skip<T>(this IListAndListSource<T> list, int start)
		{
			return list.Slice(start);
		}
		public static IListSource<T> Take<T>(this IListAndListSource<T> list, int count)
		{
			return list.Slice(0, count);
		}
		public static NegListSlice<T> Skip<T>(this INegListSource<T> list, int count)
		{
			CheckParam.IsNotNegative("count", count);
			return new NegListSlice<T>(list, checked(list.Min + count), int.MaxValue);
		}
		public static NegListSlice<T> Take<T>(this INegListSource<T> list, int count)
		{
			CheckParam.IsNotNegative("count", count);
			return new NegListSlice<T>(list, list.Min, count);
		}

		/// <summary>Copies the contents of an IListSource or IReadOnlyList to an array.</summary>
		public static T[] ToArray<T>(this IReadOnlyList<T> c)
		{
			var array = new T[c.Count];
			for (int i = 0; i < array.Length; i++)
				array[i] = c[i];
			return array;
		}
		/// <summary>Copies the contents of an <see cref="INegListSource{T}"/> to an array.</summary>
		public static T[] ToArray<T>(this INegListSource<T> c)
		{
			var array = new T[c.Count];
			int min = c.Min;
			for (int i = 0; i < array.Length; i++)
				array[i] = c[i + min];
			return array;
		}

		public static IListSource<TResult> Select<T, TResult>(this IListSource<T> source, Func<T, TResult> selector)
		{
			return new SelectListSource<T, TResult>(source, selector);
		}
		public static IList<TResult> Select<T, TResult>(this IList<T> source, Func<T, TResult> selector)
		{
			return new SelectList<T, TResult>(source, selector);
		}
		public static IListSource<TResult> Select<T, TResult>(this IListAndListSource<T> source, Func<T, TResult> selector)
		{
			return new SelectListSource<T, TResult>(source, selector);
		}

		// TODO:
		// public static IEnumerable<TSource> Reverse<TSource>(this IEnumerable<TSource> source);
		// public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector);
		//     Projects each element of a sequence into a new form by incorporating the element's index.
		//   source:
		//     A sequence of values to invoke a transform function on.
		//   selector:
		//     A transform function to apply to each source element; the second parameter of
		//     the function represents the index of the source element.
		// public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector);
	}
}
