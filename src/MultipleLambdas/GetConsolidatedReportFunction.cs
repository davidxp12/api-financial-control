using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Ardalis.GuardClauses;
using FinancialControl.Application.Dtos;
using FinancialControl.Application.Queries;
using FinancialControl.Domain.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MultipleLambdas
{
	public class GetConsolidatedReportFunction : FunctionBase
	{
		public GetConsolidatedReportFunction()
		{

		}

		public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
		{
			context.SetEnvironment();
			this.Logger.SetLoggerContext(context.Logger);
			this.Logger.LogInfo($"Fetching consolidatedReport by date");


			try
			{
				Guard.Against.Null(request, nameof(request));
				var queryParams = request.QueryStringParameters ?? new Dictionary<string, string>();
				DateTime date = DateTime.MinValue;

				if (queryParams.ContainsKey("date"))
					DateTime.TryParse(queryParams["date"], out date);

				this.Logger.LogInfo($"Fetching consolidatedReport by date in query: date: {date}");

				var queryResult = await this.Mediator.Send(new GetConsolidatedReportQuery(date));

				// return result
				return new APIGatewayProxyResponse
				{
					StatusCode = (int)HttpStatusCode.OK,
					Body = JsonConvert.SerializeObject(queryResult)
				};
			}
			catch (Exception ex)
			{
				this.Logger.LogError($"exception; {ex.Message}");
				return new APIGatewayProxyResponse
				{
					StatusCode = (int)HttpStatusCode.InternalServerError
				};
			}
		}
	}
}
