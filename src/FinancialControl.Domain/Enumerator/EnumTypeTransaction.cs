using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialControl.Domain.Enumerator
{
	public enum EnumTypeTransaction
	{
		[Description("Debit")]
		Debit,
		[Description("Credit")]
		credit
	}
}
