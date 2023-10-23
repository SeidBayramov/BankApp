using BankApp.Enum;

class Transaction
{
    public int TransactionId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public TransactionType TransactionType { get; private set; }

    private static int transactionIdCounter = 1;
    private DateTime now;

    public Transaction(TransactionType transactionType, decimal amount)
    {
        TransactionId = transactionIdCounter++;
        TransactionType = transactionType;
        Amount = amount;
        TransactionDate = DateTime.Now;
    }

    public Transaction(TransactionType transactionType, decimal amount, DateTime now) : this(transactionType, amount)
    {
        this.now = now;
    }
}