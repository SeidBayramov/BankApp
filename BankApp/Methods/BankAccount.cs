using BankApp.Enum;
using BankApp.Exceptionlar;
using BankApp.Interfeys;

class BankAccount : IAccount
{
    public int AccountId { get; private set; }
    public decimal Balance { get; private set; }
    public AccountType AccountType { get; private set; }
    public CurrencyType CurrencyType { get; private set; }

    private Transaction[] transactions;
    private int transactionCount;

    public BankAccount(int accountId, AccountType accountType, CurrencyType currencyType)
    {
        AccountId = accountId;
        AccountType = accountType;
        CurrencyType = currencyType;
        Balance = 0;
        transactions = new Transaction[100]; 
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidAmountException("Amount must be greater than zero.");

        Balance += amount;
        transactions[transactionCount++] = new Transaction(TransactionType.Deposit, amount);
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidAmountException("Amount must be greater than zero.");

        if (amount > Balance)
        {
            throw new InsufficientFundsException("Insufficient funds to withdraw.");
        }

        Balance -= amount;
        transactions[transactionCount++] = new Transaction(TransactionType.Withdraw, amount);
    }

    public Transaction[] GetTransactions()
    {
       
        Transaction[] result = new Transaction[transactionCount];
        Array.Copy(transactions, result, transactionCount);

        return result;
    }
}
