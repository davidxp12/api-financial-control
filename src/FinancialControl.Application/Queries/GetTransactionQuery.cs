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

		public List<Transaction> SearchBySecondaryIndexByDate(string date)
		{
			List<Transaction> results = new List<Transaction>();

			if (!string.IsNullOrEmpty(date))
			{
				Dictionary<string, AttributeValue> _expressionAttributeValues = new Dictionary<string, AttributeValue>();

				_expressionAttributeValues.Add(":v_date", new AttributeValue
				{
					S = date
				});

				IList<string> _listIdKey = _transactionRepository.GetIdByIndex<Transaction>("index_date", "Date = :v_date", _expressionAttributeValues);

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
