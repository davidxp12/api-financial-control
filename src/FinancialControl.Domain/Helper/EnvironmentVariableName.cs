using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialControl.Domain.Helper
{
	public static class EnvironmentVariableName
	{
		public const string EnvironmentName = "ASPNETCORE_ENVIRONMENT";
		public const string Development = "dev";
		public const string Homologation = "hml";
		public const string Production = "prd";
		public const string Blue = "azl";
	}
}
