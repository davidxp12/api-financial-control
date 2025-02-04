using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Ardalis.GuardClauses;
using FinancialControl.Application.Commands;
using FinancialControl.Domain.Helper;
using FinancialControl.Domain.Queue;
using Newtonsoft.Json;
using ProductCatalogue.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MultipleLambdas
{
	public class SetTransactionFuncion : FunctionBase
	{
		public SetTransactionFuncion()
		{

		}

		public async void FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
		{
			this.Logger.SetLoggerContext(context.Logger);
			this.Logger.LogInfo($"FunctionHandler SetTransactionFuncion begin");

			var tasks = sqsEvent.Records.Select(message => ProcessMessage(message, context)).ToList();

			var results = await Task.WhenAll(tasks);

			this.Logger.LogInfo($"FunctionHandler SetTransactionFuncion finished");
		}

		public async Task<bool> ProcessMessage(SQSEvent.SQSMessage message, ILambdaContext context)
		{
			try
			{
				this.Logger.LogInfo($"ProcessMessage: {message.Body}");

				Guard.Against.Null(message, nameof(message));
				var command = JsonConvert.DeserializeObject<RegisterTransactionCommand>(message.ToJson());

				var cmdResult = await this.Mediator.Send(command);

				this._SQSManager.DeleteMessage(_SQSManager.GetQueueUrlAsync(ConfigurationManager.AppSettings[$"appsettings:sqs:QueueTransaction"]).Result, message.ReceiptHandle);
				this.Logger.LogInfo($"Mensage {message.MessageId} process success!");

				return await Task.FromResult(cmdResult);
			}
			catch (Exception ex)
			{
				this.Logger.LogError($"Error mensagem {message.MessageId}: {ex.Message}");
				return false;
			}
		}
	}
}
