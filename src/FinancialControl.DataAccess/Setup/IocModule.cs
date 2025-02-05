using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.SQS;
using FinancialControl.Domain.Consolidate;
using FinancialControl.Domain.Helper;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalogue.Domain.Repositories;
using System;

namespace ProductCatalogue.Persistence.Setup
{
    public static class IocModule
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection serviceCollection)
        {
			serviceCollection.AddTransient<IRepository<ConsolidatedReport>, Repository<ConsolidatedReport>>();

			return serviceCollection;
        }

		public static IServiceCollection AddAwsServices(this IServiceCollection serviceCollection)
		{
			string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

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

			serviceCollection.AddSingleton<IAmazonSQS, AmazonSQSClient>();

			return serviceCollection;
		}
	}
}
