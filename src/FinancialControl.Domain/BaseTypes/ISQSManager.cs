using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialControl.Domain.BaseTypes
{
	public interface ISQSManager
	{
		bool Send(string _message, string _serviceURL, string awsAccessKeyId, string awsSecretAccessKey, int _retry);
		bool DeleteMessage(string _serviceURL, string ReceiptHandle);
		Task<string> GetQueueUrlAsync(string queueName);
	}
}
