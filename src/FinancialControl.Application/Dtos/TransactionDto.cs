using Amazon.DynamoDBv2.DataModel;
using FinancialControl.Domain.Enumerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialControl.Application.Dtos
{
	public class TransactionDto
	{
		public long TransactionId { get; set; }

		public string Data { get; set; }
		public decimal Amount { get; set; }

		public EnumTypeTransaction Type { get; set; }

		public string Category { get; set; }
	}
}
