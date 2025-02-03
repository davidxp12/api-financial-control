using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogue.Persistence.Setup
{
	public interface IRepository<T> where T : class
	{
		T Get(object hashKey);
		T Get(object hashKey, object rangeKey);
		List<T> Get(IList<ScanCondition> conditions);
		void Insert(T document);
		void Save(T document);
		void Delete(T document);
		IList<string> GetIdByIndex<TModelClass>(string indexName, string keyConditionExpression, Dictionary<string, AttributeValue> expressionAttributeValues, bool scanIndexForward = true);
		List<T> GetAllObjectsByKey(object hashKey);
		QueryResponse Query(QueryRequest queryRequest);
		ScanResponse Query(ScanRequest queryRequest);
		AsyncSearch<T> FromScanAsync(ScanOperationConfig scanOperationConfig, DynamoDBOperationConfig dynamoDBOperationConfig = null);
	}
}
