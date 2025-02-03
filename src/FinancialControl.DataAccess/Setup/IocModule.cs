using Amazon.DynamoDBv2.DataModel;
using FinancialControl.Domain.Consolidate;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalogue.Domain.Repositories;

namespace ProductCatalogue.Persistence.Setup
{
    public static class IocModule
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IProductsRepository, ProductsRepository>();
			
          //  serviceCollection.AddTransient<IDynamoDBContext, ProductsRepository>();

			serviceCollection.AddTransient<IRepository<ConsolidatedReport>, Repository<ConsolidatedReport>>();
			return serviceCollection;
        }

		public static IServiceCollection AddDatabaseServices(this IServiceCollection serviceCollection)
		{
			//serviceCollection.AddTransient<IDynamoDBContext, ProductsRepository>();

			return serviceCollection;
		}
	}
}
