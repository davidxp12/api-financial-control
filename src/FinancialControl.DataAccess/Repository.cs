using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ProductCatalogue.Persistence.Setup;

namespace ProductCatalogue.Persistence
{
	public class Repository<T> : IRepository<T> where T : class
	{
		protected AmazonDynamoDBClient _client { get; set; }
		protected IDynamoDBContext _context;
		protected string _environment { get; private set; }
		public Repository(IDynamoDBContext context)
		{
			_context = context;
		}
		public Repository(IDynamoDBContext context, AmazonDynamoDBClient client, string environment)
		{
			_context = context;
			_client = client;
			_environment = environment;
		}
		public void Delete(T document)
		{
			_context.DeleteAsync(document).Wait();
		}
		public T Get(object hashKey, object rangeKey)
		{
			return _context.LoadAsync<T>(hashKey, rangeKey).Result;
		}
		public T Get(object hashKey)
		{
			return _context.LoadAsync<T>(hashKey).Result;
		}

		public List<T> GetAllObjectsByKey(object hashKey)
		{
			return _context.QueryAsync<T>(hashKey).GetNextSetAsync().Result;
		}

		public void Insert(T document)
		{
			_context.SaveAsync<T>(document).Wait();
		}

		public void Save(T document)
		{
			_context.SaveAsync(document).Wait();
		}
		public List<T> Get(IList<ScanCondition> conditions)
		{
			return _context.ScanAsync<T>(conditions).GetNextSetAsync().Result;
		}

		public IList<string> GetIdByIndex<TModelClass>(string indexName, string keyConditionExpression, Dictionary<string, AttributeValue> expressionAttributeValues, bool scanIndexForward = true)
		{
			List<string> _resultItems = new List<string>();
			Dictionary<string, AttributeValue> startKey = null;
			QueryRequest queryRequest = new QueryRequest
			{
				TableName = string.Format("{0}{1}", this._environment, typeof(TModelClass).GetCustomAttribute<DynamoDBTableAttribute>().TableName),
				IndexName = indexName,
				ScanIndexForward = scanIndexForward,
				KeyConditionExpression = keyConditionExpression,
				ExpressionAttributeValues = expressionAttributeValues
			};

			Console.WriteLine($"Begin Search by Index - {indexName}");

			do
			{
				var result = _client.QueryAsync(queryRequest).Result;
				foreach (var item in result.Items)
					foreach (string attr in item.Keys)
					{
						if (attr == typeof(TModelClass).GetCustomAttribute<DynamoDBHashKeyAttribute>().AttributeName)
						{
							if (!string.IsNullOrEmpty(item[attr].N) && !_resultItems.Contains(item[attr].N))
								_resultItems.Add(item[attr].N);

							if (!string.IsNullOrEmpty(item[attr].S) && !_resultItems.Contains(item[attr].S))
								_resultItems.Add(item[attr].S);
						}
					}

				startKey = result.LastEvaluatedKey;
			} while (startKey.Count != 0);

			Console.WriteLine($"End Search by Index - {indexName} Total -  {_resultItems.Count}");

			return _resultItems;
		}

		public QueryResponse Query(QueryRequest queryRequest)
		{
			return _client.QueryAsync(queryRequest).Result;
		}
		public ScanResponse Query(ScanRequest queryRequest)
		{
			return _client.ScanAsync(queryRequest).Result;
		}
		public AsyncSearch<T> FromScanAsync(ScanOperationConfig scanOperationConfig, DynamoDBOperationConfig dynamoDBOperationConfig = null)
		{
			return _context.FromScanAsync<T>(scanOperationConfig, dynamoDBOperationConfig);
		}
	}
}

