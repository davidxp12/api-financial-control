using Amazon.DynamoDBv2.DataModel;
using FinancialControl.Domain.Enumerator;
using ProductCatalogue.Domain.BaseTypes;
using ProductCatalogue.Domain.Products.Events;
using ProductCatalogue.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinancialControl.Domain.Products.Events;

namespace FinancialControl.Domain.Transaction
{
	[DynamoDBTable("Transaction")]
	public class Transaction : AggregateRoot
	{
		public Transaction()
		{
				
		}
		[DynamoDBHashKey(AttributeName = "TransactionId")]
		public long TransactionId { get; set; }

		[DynamoDBGlobalSecondaryIndexHashKey(AttributeName = "Data")]
		public string Data { get; set; } // patter yyyy-mm-dd
		[DynamoDBRangeKey(AttributeName = "Amount")]
		public decimal Amount { get; set; }

		[DynamoDBRangeKey(AttributeName = "Type")]
		public EnumTypeTransaction Type { get; set; }

		[DynamoDBRangeKey(AttributeName = "Category")]
		public string Category { get; set; }


		public void SendConsolidation()
		{
			this.QueueEvent(new RegisterConsolidationEvent(this));
		}
	}
}
