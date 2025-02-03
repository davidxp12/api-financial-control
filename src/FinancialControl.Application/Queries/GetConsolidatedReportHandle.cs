using Ardalis.GuardClauses;
using AutoMapper;
using FinancialControl.Application.Dtos;
using MediatR;
using ProductCatalogue.Application.Dtos;
using ProductCatalogue.Application.Queries;
using ProductCatalogue.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FinancialControl.Application.Queries
{
	public class GetConsolidatedReportHandle : IRequestHandler<GetConsolidatedReportQuery, ConsolidatedReportDto>
	{
		private readonly IProductsRepository _productsRepository;
		private readonly IMapper _mapper;

		public GetConsolidatedReportHandle(IProductsRepository productsRepository, IMapper mapper)
		{
			this._productsRepository = Guard.Against.Null(productsRepository, nameof(productsRepository));
			this._mapper = Guard.Against.Null(mapper, nameof(mapper));
		}
		public async Task<ConsolidatedReportDto> Handle(GetConsolidatedReportQuery request, CancellationToken cancellationToken)
		{
			Guard.Against.Null(request, nameof(request));

			var product = await this._productsRepository.GetBySkuAsync(request.TenantId, request.Sku, cancellationToken);
			return this._mapper.Map<ProductDto>(product);
		}
	}
}
