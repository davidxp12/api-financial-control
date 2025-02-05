using AutoMapper;
using FinancialControl.Application.Dtos;

namespace ProductCatalogue.Application.Setup
{
	public class AutoMapperConfig : Profile
	{
		public AutoMapperConfig()
		{
			this.CreateMap<TransactionDto, ConsolidatedReportDto>();
		}
	}
}
