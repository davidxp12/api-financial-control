using Ardalis.GuardClauses;
using AutoMapper;
using FinancialControl.Application.Queries;
using FinancialControl.Domain.Consolidate;
using FinancialControl.Domain.Enumerator;
using FinancialControl.Domain.Transaction;
using MediatR;
using ProductCatalogue.Persistence.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FinancialControl.Application.Commands
{
	public class ConsolidatedReportCommandHandle : IRequestHandler<ConsolidatedReportCommand, bool>
	{
		private readonly ProductCatalogue.Domain.Repositories.IUnitOfWork _unitOfWork;
		private readonly IRepository<ConsolidatedReport> _consolidatedReportRepository;
		private readonly IRepository<Transaction> _transactionRepository;
		private readonly IMapper _mapper;
		private GetTransactionQuery _transactionQuery;

		public GetTransactionQuery GetTransactionQuery()
		{
			if (_transactionQuery == null)
				_transactionQuery = new GetTransactionQuery(_transactionRepository);

			return _transactionQuery;
		}

		public ConsolidatedReportCommandHandle(ProductCatalogue.Domain.Repositories.IUnitOfWork unitOfWork, IMapper mapper, IRepository<ConsolidatedReport> consolidatedReportRepository, IRepository<Transaction> transactionRepository)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_consolidatedReportRepository = consolidatedReportRepository;
			_transactionRepository = transactionRepository;
		}
		public async Task<bool> Handle(ConsolidatedReportCommand cmd, CancellationToken cancellationToken)
		{
			Guard.Against.Null(cmd, nameof(cmd));

			var consolidatedReport = this._mapper.Map<ConsolidatedReport>(cmd);

			if (consolidatedReport != null && !string.IsNullOrEmpty(consolidatedReport.Date))
			{
				consolidatedReport.TotalDebits = 0;
				consolidatedReport.TotalCredits = 0;

				var TransactionList = GetTransactionQuery().SearchBySecondaryIndexByDate(consolidatedReport.Date);

				if (TransactionList.Count > 0)
				{
					consolidatedReport.TotalDebits = TransactionList.Where(x => x.Type == EnumTypeTransaction.Debit).Select(r=> r.Amount).Sum();
					consolidatedReport.TotalCredits = TransactionList.Where(x => x.Type == EnumTypeTransaction.Credit).Select(r => r.Amount).Sum();

					_consolidatedReportRepository.Save(consolidatedReport);
					_unitOfWork.Commit(consolidatedReport);
				}
			}

			return true;
		}

	}
}
