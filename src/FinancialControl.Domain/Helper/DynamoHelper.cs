using Amazon.DynamoDBv2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialControl.Domain.Helper
{
	public class DynamoHelper
	{
		public static IAmazonDynamoDB CreateAmazonDynamoDB(string accessKey = null, string secretKey = null, string serviceURL = null)
		{
			if (!string.IsNullOrEmpty(serviceURL))
				return new AmazonDynamoDBClient(new AmazonDynamoDBConfig() { ServiceURL = serviceURL });
			else
				return new AmazonDynamoDBClient(Amazon.RegionEndpoint.USEast1);
		}
	}
}
