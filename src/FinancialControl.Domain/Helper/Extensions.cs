using Amazon.Lambda.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialControl.Domain.Helper
{
	public static class Extensions
	{
		public static string ToJson(this object obj)
		{
			if (obj == null) return null;
			return JsonHelper.SerializeObject(obj);
		}

		public static IEnumerable<TSource> ExceptBy<TSource, TKey>(
		this IEnumerable<TSource> first, IEnumerable<TSource> second,
		Func<TSource, TKey> keySelector,
		IEqualityComparer<TKey> keyComparer = null)
		{
			if (first == null) throw new ArgumentNullException("first");
			if (second == null) throw new ArgumentNullException("second");
			if (keySelector == null) throw new ArgumentNullException("keySelector");

			return first.ExceptByIterator(second, keySelector, keyComparer);
		}

		private static IEnumerable<TSource> ExceptByIterator<TSource, TKey>(
			this IEnumerable<TSource> first, IEnumerable<TSource> second,
			Func<TSource, TKey> keySelector, IEqualityComparer<TKey> keyComparer)
		{
			var keys = new HashSet<TKey>(second.Select(keySelector), keyComparer);

			foreach (TSource item in first)
			{
				if (keys.Add(keySelector(item)))
					yield return item;
			}
		}

		public static void SetEnvironment(this ILambdaContext context)
		{
			string functionArn = context != null && !string.IsNullOrWhiteSpace(context.InvokedFunctionArn) ? context.InvokedFunctionArn : "dev ";
			if (functionArn.EndsWith(EnvironmentVariableName.Homologation))
				Environment.SetEnvironmentVariable(EnvironmentVariableName.EnvironmentName, EnvironmentVariableName.Homologation);
			else if (functionArn.EndsWith(EnvironmentVariableName.Blue))
				Environment.SetEnvironmentVariable(EnvironmentVariableName.EnvironmentName, EnvironmentVariableName.Production);
			else if (functionArn.EndsWith(EnvironmentVariableName.Production))
				Environment.SetEnvironmentVariable(EnvironmentVariableName.EnvironmentName, EnvironmentVariableName.Production);
			else
				Environment.SetEnvironmentVariable(EnvironmentVariableName.EnvironmentName, EnvironmentVariableName.Development);
		}
		public static string GetEnvironment(this ILambdaContext context)
		{
			context.SetEnvironment();
			return Environment.GetEnvironmentVariable(EnvironmentVariableName.EnvironmentName);
		}
	}
}
