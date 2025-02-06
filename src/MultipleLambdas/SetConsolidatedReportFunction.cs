using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.SQS.Model;
using Ardalis.GuardClauses;
using FinancialControl.Application.Commands;
using FinancialControl.Domain.Helper;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleLambdas
{
	public class SetConsolidatedReportFunction : FunctionBase
	{
		public SetConsolidatedReportFunction()
		{

		}

		public async void FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
		{
			context.SetEnvironment();
			this.Logger.SetLoggerContext(context.Logger);
			this.Logger.LogInfo($"FunctionHandler SetConsolidatedReportFunction begin");

			foreach (var record in sqsEvent.Records)
			{
				await ProcessMessage(record, context);
			}

			this.Logger.LogInfo($"FunctionHandler SetConsolidatedReportFunction finished");
		}
		public async Task<bool> ProcessMessage(SQSEvent.SQSMessage message, ILambdaContext context)
		{
			try
			{
				this.Logger.LogInfo($"ProcessMessage: {message.Body}");

				Guard.Against.Null(message, nameof(message));
				var command = JsonConvert.DeserializeObject<ConsolidatedReportCommand>(message.ToJson());

				var cmdResult = await this.Mediator.Send(command);

				this._SQSManager.DeleteMessage(_SQSManager.GetQueueUrlAsync(ConfigurationManager.AppSettings[$"appsettings:sqs:QueueConsolidated"]).Result, message.ReceiptHandle);
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
