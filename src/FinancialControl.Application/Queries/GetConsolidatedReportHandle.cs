using Ardalis.GuardClauses;
using AutoMapper;
using FinancialControl.Application.Dtos;
using FinancialControl.Domain.Consolidate;
using MediatR;
using ProductCatalogue.Persistence.Setup;
using System.Threading;
using System.Threading.Tasks;

namespace FinancialControl.Application.Queries
{
	public class GetConsolidatedReportHandle : IRequestHandler<GetConsolidatedReportQuery, ConsolidatedReportDto>
	{
		private readonly IRepository<ConsolidatedReport> _consolidatedReportRepository;
		private readonly IMapper _mapper;

		public GetConsolidatedReportHandle(IRepository<ConsolidatedReport> consolidatedReportRepository, IMapper mapper)
		{
			this._consolidatedReportRepository = Guard.Against.Null(consolidatedReportRepository, nameof(consolidatedReportRepository));
			this._mapper = Guard.Against.Null(mapper, nameof(mapper));
		}
		public async Task<ConsolidatedReportDto> Handle(GetConsolidatedReportQuery request, CancellationToken cancellationToken)
		{
			Guard.Against.Null(request, nameof(request));

			var consolidatedReportDto = this._consolidatedReportRepository.Get(request.Date);
			return this._mapper.Map<ConsolidatedReportDto>(consolidatedReportDto);
		}
	}
}
