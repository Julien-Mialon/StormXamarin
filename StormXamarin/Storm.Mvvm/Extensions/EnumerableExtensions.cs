using System;
using System.Collections.Generic;

namespace Storm.Mvvm.Extensions
{
	public static class EnumerableExtensions
	{
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> apply)
		{
			foreach (T item in source)
			{
				apply(item);
			}
		}

		public static void RemoveWhere<T>(this List<T> source, Predicate<T> whereClause)
		{
			for (int i = 0; i < source.Count; ++i)
			{
				if (whereClause(source[i]))
				{
					source.RemoveAt(i);
					i--;
				}
			}
		}
	}
}
