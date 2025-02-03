using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialControl.Domain.BaseTypes
{
	public interface IQueueSender
	{
		bool Send(string _message, string _serviceURL, string awsAccessKeyId, string awsSecretAccessKey, int _retry);
	}
}
