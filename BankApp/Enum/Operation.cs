using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Enum
{
    internal enum Operation
    {
        CreateAccount=1,
        DepositMoney, 
        WithdrawMoney, 
        ListTransactions, 
        ListAccounts,
        TransferMoney, 
        CurrencyConversion,
        Exit


    }
}
