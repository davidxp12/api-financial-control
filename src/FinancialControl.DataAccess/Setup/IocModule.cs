using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.SQS;
using FinancialControl.Domain.Consolidate;
using FinancialControl.Domain.Helper;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalogue.Domain.Repositories;
using System;
using System.Transactions;

namespace ProductCatalogue.Persistence.Setup
{
	public static class IocModule
	{
		public static IServiceCollection AddPersistenceServices(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddTransient<IRepository<ConsolidatedReport>, Repository<ConsolidatedReport>>();
			serviceCollection.AddTransient<IRepository<Transaction>, Repository<Transaction>>();

			return serviceCollection;
		}

		public static IServiceCollection AddAwsServices(this IServiceCollection serviceCollection)
		{
			string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

			if (environment == "dev")
			{
				serviceCollection.AddSingleton<IDynamoDBContext>(sp =>
				{
					return new DynamoDBContext(DynamoHelper.CreateAmazonDynamoDB(null, null, ConfigurationManager.AppSettings[$"appsettings:database:dynamo:url"]), new DynamoDBContextConfig() { TableNamePrefix = environment });
				});

				serviceCollection.AddSingleton<IAmazonDynamoDB>(sp =>
				{
					var config = new AmazonDynamoDBConfig
					{
						ServiceURL = ConfigurationManager.AppSettings[$"appsettings:database:dynamo:url"]
					};
					return new AmazonDynamoDBClient(config);
				});
			}
			else
			{
				serviceCollection.AddSingleton<IDynamoDBContext>(sp =>
				{
					return new DynamoDBContext(new AmazonDynamoDBClient(Amazon.RegionEndpoint.USEast1), new DynamoDBContextConfig() { TableNamePrefix = environment });
				});

				serviceCollection.AddSingleton<IAmazonDynamoDB>(sp =>
				{
					return new AmazonDynamoDBClient(Amazon.RegionEndpoint.USEast1);
				});
			}

			serviceCollection.AddSingleton<IAmazonSQS, AmazonSQSClient>();

			return serviceCollection; 
		}
	}
}
