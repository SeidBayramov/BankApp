using BankApp.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Interfeys
{
    internal interface IAccount
    {
        int AccountId { get; }
        decimal Balance { get; }
        AccountType AccountType { get; }
        CurrencyType CurrencyType { get; }
        void Deposit(decimal amount);
        void Withdraw(decimal amount);
    }
}