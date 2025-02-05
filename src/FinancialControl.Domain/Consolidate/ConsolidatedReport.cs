using Amazon.DynamoDBv2.DataModel;
using ProductCatalogue.Domain.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialControl.Domain.Consolidate
{
	[DynamoDBTable("ConsolidatedReport")]
	public class ConsolidatedReport : AggregateRoot
	{
		[DynamoDBHashKey(AttributeName = "Data")]
		public string Data { get; set; }
		[DynamoDBRangeKey(AttributeName = "TotalCredits")]
		public decimal TotalCredits { get; set; }
		[DynamoDBRangeKey(AttributeName = "TotalDebits")]
		public decimal TotalDebits { get; set; }
		[DynamoDBRangeKey(AttributeName = "FinalBalance")]
		public decimal FinalBalance {

			get { 
			
				return TotalCredits - TotalDebits;
			}
		}
	}
}
