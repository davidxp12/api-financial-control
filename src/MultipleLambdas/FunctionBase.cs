using System;
using FinancialControl.Domain.Queue;
using LambdaLogger;
using LambdaLogger.Setup;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalogue.Application.Setup;

namespace MultipleLambdas
{
    /// <summary>
    /// Base class for functions to centralise the ioc and mediatr setup
    /// </summary>
    public class FunctionBase
    {
        private readonly IServiceCollection _serviceCollection;
        private readonly ServiceProvider _serviceProvider;
        private readonly Lazy<IMediator> _mediatr;
        private readonly Lazy<ILogger> _logger;
        public readonly SQSManager _SQSManager;
		private Guid _CorrelationId { get; set; }

		public FunctionBase()
        {
            this._serviceCollection = new ServiceCollection()
                .AddApplicationServices()
				.AddLoggingService();
            this._serviceProvider = this._serviceCollection.BuildServiceProvider();

            this._mediatr = new Lazy<IMediator>(() => this._serviceProvider.GetRequiredService<IMediator>());
            this._logger = new Lazy<ILogger>(() => this._serviceProvider.GetRequiredService<ILogger>());
			this._CorrelationId = Guid.Parse(Guid.NewGuid().ToString());
            this._SQSManager = new SQSManager();
		}

        protected ILogger Logger => this._logger.Value;
        protected IMediator Mediator => this._mediatr.Value;
    }
}
