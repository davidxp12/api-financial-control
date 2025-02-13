﻿using Ardalis.GuardClauses;
using FinancialControl.Domain.Enumerator;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialControl.Application.Commands
{
	public class RegisterTransactionCommand : IRequest<bool>
	{
		public RegisterTransactionCommand(DateTime Date, decimal amount, string category)
		{
			this.TransactionId = Guid.NewGuid();
			this.Date = Date.ToString("yyyy-MM-dd");
			this.Amount = Amount;
			this.Category = category;
			this.Type = amount > 0 ? EnumTypeTransaction.Credit : EnumTypeTransaction.Debit;
		}

		public Guid TransactionId { get; set; }
		public string Date { get; set; }
		public decimal Amount { get; set; }
		public EnumTypeTransaction Type { get; set; }
		public string Category { get; set; }
	}
}