using FinancialControl.Domain.Enumerator;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialControl.Application.Commands
{
	public class ConsolidatedReportCommand : IRequest<bool>
	{
		public Guid TransactionId { get; set; }
		public string Data { get; set; }
		public decimal Amount { get; set; }
		public EnumTypeTransaction Type { get; set; }
		public string Category { get; set; }
	}
}
