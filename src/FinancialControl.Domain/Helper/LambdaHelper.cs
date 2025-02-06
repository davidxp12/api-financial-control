using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialControl.Domain.Helper
{
	public static class LambdaHelper
	{
		#region TDD
		public static APIGatewayProxyRequest LambdaTestBuildRequest<TModelClass>(TModelClass modelClass) where TModelClass : class
		{
			APIGatewayProxyRequest request = new APIGatewayProxyRequest();
			request.Body = JsonHelper.SerializeObject(modelClass);
			request.Headers = new Dictionary<string, string>();
			request.Headers.Add("Authorization", String.Empty);
			return request;
		}
		public static TestLambdaContext LambdaTestBuildContext()
		{
			TestLambdaContext context = new TestLambdaContext();
			context.InvokedFunctionArn = "dev";
			Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", context.InvokedFunctionArn);
			return context;
		}
		#endregion
	}
}
