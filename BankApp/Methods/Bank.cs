using BankApp.Enum;
using BankApp.Exceptionlar;
using BankApp.Interfeys;
using System;

class Bank
{
    private IAccount[] accounts;
    private int accountCount;

    public Bank()
    {
        accounts = new IAccount[100];
        accountCount = 0;
    }

    public IAccount CreateAccount(AccountType accountType, CurrencyType currencyType)
    {
        if (accountCount >= accounts.Length)
        {
            Console.WriteLine("Account limit reached. You cannot create more accounts.");
            return null;
        }

        int accountId = accountCount + 1;
        IAccount account = new BankAccount(accountId, accountType, currencyType);
        accounts[accountCount] = account;
        accountCount++;
        return account;
    }

    public void DepositMoney(int accountId, decimal amount)
    {
        IAccount account = GetAccountById(accountId);
        account.Deposit(amount);
    }

    public void WithdrawMoney(int accountId, decimal amount)
    {
        IAccount account = GetAccountById(accountId);
        account.Withdraw(amount);
    }

    public void TransferMoney(int fromAccountId, int toAccountId, decimal amount)
    {
        IAccount fromAccount = GetAccountById(fromAccountId);
        IAccount toAccount = GetAccountById(toAccountId);

        if (fromAccount.CurrencyType != toAccount.CurrencyType)
        {
            throw new Exception("Currency types must match for the transfer.");
        }

        fromAccount.Withdraw(amount);
        toAccount.Deposit(amount);
    }

    public IAccount[] GetAllAccounts()
    {
        IAccount[] result = new IAccount[accountCount];
        for (int i = 0; i < accountCount; i++)
        {
            result[i] = accounts[i];
        }

        return result;
    }

    public decimal CurrencyConversion(int accountId, CurrencyType toCurrencyType)
    {
       
        throw new NotImplementedException("Currency conversion is not supported.");
    }

    private IAccount GetAccountById(int accountId)
    {
        for (int i = 0; i < accountCount; i++)
        {
            if (accounts[i].AccountId == accountId)
            {
                return accounts[i];
            }
        }

        throw new AccountNotFoundException("Account not found.");
    }

    public static int FindEmptyAccountIndex(IAccount[] accounts)
    {
        for (int i = 0; i < accounts.Length; i++)
        {
            if (accounts[i] == null)
            {
                return i;
            }
        }
        throw new Exception("Account limit reached.");
    }

     public static int FindAccountIndexById(IAccount[] accounts, int accountId)
    {
        for (int i = 0; i < accounts.Length; i++)
        {
            if (accounts[i] != null && accounts[i].AccountId == accountId)
            {
                return i;
            }
        }
        return -1; 
    }

    public static void RecordTransaction(Transaction[] transactions, TransactionType transactionType, decimal amount)
    {
        for (int i = 0; i < transactions.Length; i++)
        {
            if (transactions[i] == null)
            {
                transactions[i] = new Transaction(transactionType, amount, DateTime.Now);
                break;
            }
        }
    }
    public static decimal GetExchangeRate(CurrencyType sourceCurrency, CurrencyType targetCurrency)
    {
        if (sourceCurrency == CurrencyType.USD && targetCurrency == CurrencyType.AZN)
        {
            return 1.7m; 
        }
        else if (sourceCurrency == CurrencyType.USD && targetCurrency == CurrencyType.EUR)
        {
            return 0.85m; 
        }
        else if (sourceCurrency == CurrencyType.AZN && targetCurrency == CurrencyType.USD)
        {
            return 0.59m; 
        }
        else if (sourceCurrency == CurrencyType.AZN && targetCurrency == CurrencyType.EUR)
        {
            return 0.5m; 
        }
        else if (sourceCurrency == CurrencyType.EUR && targetCurrency == CurrencyType.USD)
        {
            return 1.18m; 
        }
        else if (sourceCurrency == CurrencyType.EUR && targetCurrency == CurrencyType.AZN)
        {
            return 2.0m; 
        }
        else
        {
            return -1; 
        }
    }

}
