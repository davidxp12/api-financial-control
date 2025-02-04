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
	}
}
