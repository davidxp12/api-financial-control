using Amazon.DynamoDBv2.DataModel;
using FinancialControl.Domain.Enumerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialControl.Domain.Transaction
{
	[DynamoDBTable("Transaction")]
	public class Transaction
	{
		public Transaction()
		{
				
		}
		[DynamoDBHashKey(AttributeName = "TransactionId")]
		public long TransactionId { get; set; }

		[DynamoDBRangeKey(AttributeName = "Data")]
		public string Data { get; set; } // patter yyyy-mm-dd
		[DynamoDBRangeKey(AttributeName = "Amount")]
		public decimal Amount { get; set; }

		[DynamoDBRangeKey(AttributeName = "Type")]
		public EnumTypeTransaction Type { get; set; }

		[DynamoDBRangeKey(AttributeName = "Category")]
		public string Category { get; set; }

	}
}
