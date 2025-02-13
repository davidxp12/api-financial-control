﻿using Ardalis.GuardClauses;
using AutoMapper;
using FinancialControl.Domain.Transaction;
using MediatR;
using ProductCatalogue.Persistence.Setup;
using System.Threading;
using System.Threading.Tasks;

namespace FinancialControl.Application.Commands
{
	public class RegisterTransactionCommandHandler : IRequestHandler<RegisterTransactionCommand, bool>
	{
		private readonly ProductCatalogue.Domain.Repositories.IUnitOfWork _unitOfWork;
		private readonly IRepository<Transaction> _transactionRepository;
		private readonly IMapper _mapper;
		public RegisterTransactionCommandHandler(ProductCatalogue.Domain.Repositories.IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;

		}
		public async Task<bool> Handle(RegisterTransactionCommand cmd, CancellationToken cancellationToken)
		{
			Guard.Against.Null(cmd, nameof(cmd));

			var transaction = this._mapper.Map<Transaction>(cmd);

			if (transaction.Amount > 0)
			{
				_transactionRepository.Save(transaction);

				transaction.SendConsolidation(); // send to SQS queue to consolidate the transaction

				_unitOfWork.Commit(transaction);
			}

			return true;
		}
	}
}
