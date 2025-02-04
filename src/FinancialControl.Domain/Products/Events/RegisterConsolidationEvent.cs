using FinancialControl.Domain.Helper;
using FinancialControl.Domain.Queue;
using ProductCatalogue.Domain.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialControl.Domain.Products.Events
{
	public class RegisterConsolidationEvent : IDomainEvent
	{
		public FinancialControl.Domain.Transaction.Transaction Transaction { get; }
		public RegisterConsolidationEvent(FinancialControl.Domain.Transaction.Transaction transaction)
		{
			new SQSManager().Send(transaction.ToJson(),ConfigurationManager.AppSettings[$"appsettings:sqs:urlConsolidation"]);
		}
	}
}
