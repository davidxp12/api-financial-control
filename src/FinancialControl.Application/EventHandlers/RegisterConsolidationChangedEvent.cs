using FinancialControl.Domain.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FinancialControl.Application.EventHandlers
{
	public class RegisterConsolidationChangedEvent : INotificationHandler<RegisterConsolidationEvent>
	{
		public Task Handle(RegisterConsolidationEvent notification, CancellationToken cancellationToken)
		{
			// Republish to an event bus
			// Do something interesting
			// Send an email
			// Whatever.
			System.Diagnostics.Debug.WriteLine($"{nameof(RegisterConsolidationChangedEvent)} called.");
			return Task.CompletedTask;
		}
	}
}
