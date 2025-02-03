using Ardalis.GuardClauses;
using FinancialControl.Application.Dtos;
using MediatR;
using ProductCatalogue.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialControl.Application.Queries
{
	public class GetConsolidatedReportQuery : IRequest<ConsolidatedReportDto>
	{
		public GetConsolidatedReportQuery()
		{
				
		}

		public GetConsolidatedReportQuery(DateTime date)
		{
			this.Date = date.Date.ToString("yyyy-MM-dd");
		}

		public string Date { get; set; }

	}

}
