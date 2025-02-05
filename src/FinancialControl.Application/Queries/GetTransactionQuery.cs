using Amazon.DynamoDBv2.Model;
using AutoMapper;
using FinancialControl.Domain.Consolidate;
using FinancialControl.Domain.Transaction;
using ProductCatalogue.Persistence.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialControl.Application.Queries
{
	public class GetTransactionQuery
	{
		private readonly IRepository<Transaction> _transactionRepository;
		public GetTransactionQuery(IRepository<Transaction> transactionRepository)
		{
			_transactionRepository = transactionRepository;
		}

		public List<Transaction> SearchBySecondaryIndexByData(string data)
		{
			List<Transaction> results = new List<Transaction>();

			if (!string.IsNullOrEmpty(data))
			{
				Dictionary<string, AttributeValue> _expressionAttributeValues = new Dictionary<string, AttributeValue>();

				_expressionAttributeValues.Add(":v_data", new AttributeValue
				{
					S = data
				});

				IList<string> _listIdKey = _transactionRepository.GetIdByIndex<Transaction>("index_data", "Data = :v_data", _expressionAttributeValues);

				if (_listIdKey != null)
				{
					foreach (var id in _listIdKey)
					{
						if (!string.IsNullOrEmpty(id))
						{
							var item = _transactionRepository.GetAllObjectsByKey(id);

							if (item != null && item.Count > 0)
							{
								results.AddRange(item);
							}
						}
					}
				}
			}

			return results;
		}
	}
}
