using Amazon;
using Amazon.SQS;
using FinancialControl.Domain.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialControl.Domain.Queue
{
	public class SQSManager : IQueueSender
	{
		public static AmazonSQSClient clientSQS { get; private set; }
		public int retry { get; set; }
		public bool Send(string _message, string _serviceURL, string awsAccessKeyId = null, string awsSecretAccessKey = null, int _retry = 0)
		{
			bool result = false;

			try
			{
				if (!string.IsNullOrWhiteSpace(awsAccessKeyId) && !string.IsNullOrWhiteSpace(awsSecretAccessKey))
					clientSQS = new AmazonSQSClient(awsAccessKeyId, awsSecretAccessKey, RegionEndpoint.USEast1);
				else
					clientSQS = new AmazonSQSClient(RegionEndpoint.USEast1);

				result = clientSQS.SendMessageAsync(_serviceURL, _message).Result.HttpStatusCode == System.Net.HttpStatusCode.OK;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"SQSManager {ex.Message}");

				if (retry > 4)
					return false;
				else
				{
					result = Send(_message, _serviceURL, awsAccessKeyId, awsSecretAccessKey, retry++);
				}
			}
			Console.WriteLine($"SQSManager result {result}");

			return result;
		}
	}
}
