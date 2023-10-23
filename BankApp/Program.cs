using System;
using BankApp.Enum;
using BankApp.Exceptionlar;
using BankApp.Interfeys;

class Program
{
    static void Main(string[] args)
    {
        Bank bank = new Bank();
        IAccount[] accounts = new IAccount[100];
        Transaction[][] accountTransactions = new Transaction[100][];

        Console.WriteLine("Welcome to the Bank!");

        while (true)
        {
            Console.WriteLine("1. Create an Account");
            Console.WriteLine("2. Deposit Money");
            Console.WriteLine("3. Withdraw Money");
            Console.WriteLine("4. List Transactions");
            Console.WriteLine("5. List All Accounts");
            Console.WriteLine("6. Transfer between accounts");
            Console.WriteLine("7. Currency Conversion");
            Console.WriteLine("8. EXIT");

            Console.Write("Enter the operation number: ");
            int choice = int.Parse(Console.ReadLine());

            try
            {
                switch ((Operation)choice)
                {
                    case Operation.CreateAccount:
                        Console.Write("Select account type (1 - Checking, 2 - Savings, 3 - Business): ");
                        AccountType accountType = (AccountType)int.Parse(Console.ReadLine());

                        Console.Write("Select currency type (1 - USD, 2 - AZN, 3 - EUR): ");
                        CurrencyType currencyType = (CurrencyType)int.Parse(Console.ReadLine());

                        IAccount newAccount = bank.CreateAccount(accountType, currencyType);

                        
                        int accountIndex = Bank.FindEmptyAccountIndex(accounts);
                        accounts[accountIndex] = newAccount;
                        accountTransactions[accountIndex] = new Transaction[100]; 

                        Console.WriteLine($"New account created. Account ID: {newAccount.AccountId}");
                        break;

                    case Operation.DepositMoney:
                        Console.Write("Enter the account ID: ");
                        int depositAccountId = int.Parse(Console.ReadLine());

                        Console.Write("Enter the amount to deposit: ");
                        decimal depositAmount = decimal.Parse(Console.ReadLine());

                        int depositAccountIndex = Bank.FindAccountIndexById(accounts, depositAccountId);

                        if (depositAccountIndex >= 0)
                        {
                            IAccount depositAccount = accounts[depositAccountIndex];
                            depositAccount.Deposit(depositAmount);

                           
                            Bank.RecordTransaction(accountTransactions[depositAccountIndex], TransactionType.Deposit, depositAmount);

                            Console.WriteLine("Money successfully deposited.");
                        }
                        else
                        {
                            Console.WriteLine("The account ID you entered does not exist.");
                        }
                        break;

                    case Operation.WithdrawMoney:
                        Console.Write("Enter the account ID: ");
                        int withdrawAccountId = int.Parse(Console.ReadLine());

                        Console.Write("Enter the amount to withdraw: ");
                        decimal withdrawAmount = decimal.Parse(Console.ReadLine());

              
                        int withdrawAccountIndex = Bank.FindAccountIndexById(accounts, withdrawAccountId);

                        if (withdrawAccountIndex >= 0)
                        {
                            IAccount withdrawAccount = accounts[withdrawAccountIndex];
                            try
                            {
                                withdrawAccount.Withdraw(withdrawAmount);

                               Bank.RecordTransaction(accountTransactions[withdrawAccountIndex], TransactionType.Withdraw, withdrawAmount);

                                Console.WriteLine("Money successfully withdrawn.");
                                Console.WriteLine($"Remaining balance: {withdrawAccount.Balance} {withdrawAccount.CurrencyType}");
                            }
                            catch (InvalidAmountException)
                            {
                                Console.WriteLine("The amount you entered should be greater than zero and less than the balance.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("The account ID you entered does not exist.");
                        }
                        break;

                    case Operation.ListTransactions:
                        Console.Write("Enter the account ID: ");
                        int listTransactionsAccountId = int.Parse(Console.ReadLine());

                        int listTransactionsAccountIndex = Bank.FindAccountIndexById(accounts, listTransactionsAccountId);

                        if (listTransactionsAccountIndex >= 0 && accounts[listTransactionsAccountIndex] is BankAccount)
                        {
                            IAccount listTransactionsAccount = accounts[listTransactionsAccountIndex];
                            BankAccount bankAccount = (BankAccount)listTransactionsAccount;

                            Transaction[] transactions = accountTransactions[listTransactionsAccountIndex];
                            Console.WriteLine("List of transactions:");
                            foreach (Transaction transaction in transactions)
                            {
                                if (transaction != null)
                                {
                                    Console.WriteLine($"{transaction.TransactionId}: {transaction.TransactionType} - {transaction.Amount} {bankAccount.CurrencyType} - {transaction.TransactionDate}");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("The account ID you entered does not exist or does not support listing transactions.");
                        }
                        break;

                    case Operation.ListAccounts:
                        Console.WriteLine("List of all accounts:");
                        foreach (IAccount account in accounts)
                        {
                            if (account != null)
                            {
                                Console.WriteLine($"Account ID: {account.AccountId}, Balance: {account.Balance} {account.CurrencyType}");
                            }
                        }
                        break;
                    case Operation.TransferMoney:
                        Console.Write("Enter the source account ID: ");
                        int fromAccountId = int.Parse(Console.ReadLine());

                        Console.Write("Enter the target account ID: ");
                        int toAccountId = int.Parse(Console.ReadLine());

                        Console.Write("Enter the amount to transfer: ");
                        decimal transferAmount = decimal.Parse(Console.ReadLine());

                        int fromAccountIndex = Bank.FindAccountIndexById(accounts, fromAccountId);
                        int toAccountIndex = Bank.FindAccountIndexById(accounts, toAccountId);

                        if (fromAccountIndex >= 0 && toAccountIndex >= 0)
                        {
                            IAccount fromAccount = accounts[fromAccountIndex];
                            IAccount toAccount = accounts[toAccountIndex];

                            try
                            {
                               
                                if (fromAccount.CurrencyType == toAccount.CurrencyType)
                                {
                                    fromAccount.Withdraw(transferAmount);
                                    toAccount.Deposit(transferAmount);

                                   
                                    Bank.RecordTransaction(accountTransactions[fromAccountIndex], TransactionType.Transfer, -transferAmount);
                                    Bank.RecordTransaction(accountTransactions[toAccountIndex], TransactionType.Transfer, transferAmount);

                                    Console.WriteLine("Money successfully transferred.");
                                    Console.WriteLine($"Remaining balance in source account: {fromAccount.Balance} {fromAccount.CurrencyType}");
                                    Console.WriteLine($"Balance in target account: {toAccount.Balance} {toAccount.CurrencyType}");
                                }
                                else
                                {
                                    Console.WriteLine("Currency conversion is required for the transfer.");
                                }
                            }
                            catch (InvalidAmountException)
                            {
                                Console.WriteLine("The amount you entered should be greater than zero and less than the balance.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("One or both of the account IDs you entered do not exist.");
                        }
                        break;

                    case Operation.CurrencyConversion:
                        Console.Write("Enter the source account ID: ");
                        int sourceAccountId = int.Parse(Console.ReadLine());

                        Console.Write("Enter the target currency (1 - USD, 2 - AZN, 3 - EUR): ");
                        CurrencyType targetCurrency = (CurrencyType)int.Parse(Console.ReadLine());

                        int sourceAccountIndex = Bank.FindAccountIndexById(accounts, sourceAccountId);

                        if (sourceAccountIndex >= 0)
                        {
                            IAccount sourceAccount = accounts[sourceAccountIndex];
                            decimal sourceBalance = sourceAccount.Balance;

      
                            decimal conversionRate = Bank.GetExchangeRate(sourceAccount.CurrencyType, targetCurrency);

                            if (conversionRate > 0)
                            {
                                decimal convertedBalance = sourceBalance * conversionRate;

                                Console.WriteLine($"Current balance: {sourceBalance} {sourceAccount.CurrencyType}");
                                Console.WriteLine($"Converted balance to {targetCurrency}: {convertedBalance} {targetCurrency}");
                            }
                            else
                            {
                                Console.WriteLine("Currency conversion is not supported for the selected currencies.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("The source account ID you entered does not exist.");
                        }
                        break;

                    case Operation.Exit:
                     
                        Console.WriteLine("Exiting the program.");
                        return;

                    default:
                        Console.WriteLine("You entered an incorrect operation number.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}
