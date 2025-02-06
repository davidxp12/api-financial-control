using AutoMapper;
using FinancialControl.Application.Commands;
using FinancialControl.Application.Dtos;
using FinancialControl.Domain.Consolidate;
using FinancialControl.Domain.Transaction;

namespace ProductCatalogue.Application.Setup
{
	public class AutoMapperConfig : Profile
	{
		public AutoMapperConfig()
		{
			this.CreateMap<TransactionDto, ConsolidatedReportDto>();
			this.CreateMap<RegisterTransactionCommand, Transaction>();
			this.CreateMap<ConsolidatedReportCommand, ConsolidatedReport>();
		}
	}
}
