using System;
using System.Collections.Generic;

public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

public interface ITransactionProcessor
{
    void Process(Transaction transaction);
}

public class BankTransferProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[Bank Transfer] Processed {transaction.Amount} GHC for {transaction.Category}");
    }
}

public class MobileMoneyProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[Mobile Money] Processed {transaction.Amount} GHC for {transaction.Category}");
    }
}

public class CryptoWalletProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[Crypto Wallet] Processed {transaction.Amount} GHC for {transaction.Category}");
    }
}

public class Account
{
    public string AccountNumber { get; }
    public decimal Balance { get; protected set; }

    public Account(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }

    public virtual void ApplyTransaction(Transaction transaction)
    {
        Balance -= transaction.Amount;
        Console.WriteLine($"Applied transaction of {transaction.Amount}. New Balance: {Balance}");
    }
}

public sealed class SavingsAccount : Account
{
    public SavingsAccount(string accountNumber, decimal initialBalance) 
        : base(accountNumber, initialBalance) { }

    public override void ApplyTransaction(Transaction transaction)
    {
        if (transaction.Amount > Balance)
        {
            Console.WriteLine("Insufficient funds");
        }
        else
        {
            base.ApplyTransaction(transaction);
        }
    }
}

public class FinanceApp
{
    private List<Transaction> _transactions = new List<Transaction>();

    public void Run()
    {
        var account = new SavingsAccount("SA123456", 1000);

        var t1 = new Transaction(1, DateTime.Now, 200, "Groceries");
        var t2 = new Transaction(2, DateTime.Now, 150, "Utilities");
        var t3 = new Transaction(3, DateTime.Now, 300, "Entertainment");

        ITransactionProcessor p1 = new MobileMoneyProcessor();
        ITransactionProcessor p2 = new BankTransferProcessor();
        ITransactionProcessor p3 = new CryptoWalletProcessor();

        p1.Process(t1);
        account.ApplyTransaction(t1);
        _transactions.Add(t1);

        p2.Process(t2);
        account.ApplyTransaction(t2);
        _transactions.Add(t2);

        p3.Process(t3);
        account.ApplyTransaction(t3);
        _transactions.Add(t3);

        Console.WriteLine("\nAll Transactions:");
        foreach (var t in _transactions)
        {
            Console.WriteLine($"{t.Id}: {t.Category} - {t.Amount} GHC on {t.Date.ToShortDateString()}");
        }
    }
}

class Program
{
    static void Main()
    {
        var app = new FinanceApp();
        app.Run();
    }
}
