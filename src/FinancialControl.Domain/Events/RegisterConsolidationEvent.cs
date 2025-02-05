using FinancialControl.Domain.Helper;
using FinancialControl.Domain.Queue;
using ProductCatalogue.Domain.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialControl.Domain.Events
{
	public class RegisterConsolidationEvent : IDomainEvent
	{
		public Transaction.Transaction Transaction { get; }
		public RegisterConsolidationEvent(Transaction.Transaction transaction)
		{
			new SQSManager().Send(transaction.ToJson(), ConfigurationManager.AppSettings[$"appsettings:sqs:urlConsolidation"]);
		}
	}
}
