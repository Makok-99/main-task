using System;
using System.Collections.Generic;
using System.Security.Principal;

public static class AccountUtil
{
    public static void Deposit(List<Account> accounts, double amount)
    {
        Console.WriteLine("\n=== Depositing =================");
        foreach (var acc in accounts)
            Console.WriteLine(acc.Deposit(amount)
                ? $"Deposited {amount} to {acc.Name}"
                : $"Failed Deposit to {acc.Name}");
    }

    public static void Withdraw(List<Account> accounts, double amount)
    {
        Console.WriteLine("\n=== Withdrawing ================");
        foreach (var acc in accounts)
            Console.WriteLine(acc.Withdraw(amount)
                ? $"Withdrew {amount} from {acc.Name}"
                : $"Failed Withdrawal from {acc.Name}");
    }
}
public class Account
{
    public string Name { get; set; }
    public double Balance { get; set; }

    public Account(string name = "Unnamed Account", double balance = 0.0)
    {
        Name = name;
        Balance = balance;
    }

    public virtual bool Deposit(double amount)
    {
        if (amount <= 0) return false;
        Balance += amount;
        return true;
    }

    public virtual bool Withdraw(double amount)
    {
        if (amount <= Balance)
        {
            Balance -= amount;
            return true;
        }
        return false;
    }
}
public class SavingsAccount : Account
{
    public double InterestRate { get; set; }

    public SavingsAccount(string name = "Unnamed Savings", double balance = 0.0,double interestRate = 0.0): base(name, balance)
    {
        InterestRate = interestRate;
    }

    public override bool Deposit(double amount)
    {
        amount += amount * (InterestRate / 100);
        return base.Deposit(amount);
    }
}

public class CheckingAccount : Account
{
    public CheckingAccount(string name, double balance):base(name, balance) 
    {
      this.Name = name;
        this.Balance = balance;
    }

 
    public override bool Deposit(double amount)
    {
        Balance = Balance + amount;
        return true;
    }

  
    public override bool Withdraw(double amount)
    {
        Balance = Balance - amount - 1.5;
        return true;
    }
}
public class TrustAccount : SavingsAccount
{
    private int withdrawals = 0;
    private const int MaxWithdrawals = 3;

    public TrustAccount(string name = "Unnamed Trust",
                        double balance = 0.0,
                        double interestRate = 0.0)
        : base(name, balance, interestRate) { }

    public override bool Deposit(double amount)
    {
        if (amount >= 5000)
            amount += 50;

        return base.Deposit(amount);
    }

    public override bool Withdraw(double amount)
    {
        if (withdrawals >= MaxWithdrawals)
            return false;

        if (amount > Balance * 0.2)
            return false;

        withdrawals++;
        return base.Withdraw(amount);
    }
}

class Program
{
    static void Main()
    {
        // Accounts
        var accounts = new List<Account>();
        accounts.Add(new Account());
        accounts.Add(new Account("Larry"));
        accounts.Add(new Account("Moe", 2000));
        accounts.Add(new Account("Curly", 5000));

        AccountUtil.Deposit(accounts, 1000);
        AccountUtil.Withdraw(accounts, 2000);

        // Savings
        var savAccounts = new List<Account>();
        accounts.Add(new SavingsAccount());
        accounts.Add(new SavingsAccount("Superman"));
        accounts.Add(new SavingsAccount("Batman", 2000));
        accounts.Add(new SavingsAccount("Wonderwoman", 5000, 5.0));

        AccountUtil.Deposit(accounts, 1000);
        AccountUtil.Withdraw(accounts, 2000);

        //// Checking
        var checAccounts = new List<Account>();
     
        accounts.Add(new CheckingAccount("Larry2",26));
        accounts.Add(new CheckingAccount("Moe2", 2000));
        accounts.Add(new CheckingAccount("Curly2", 5000));

        AccountUtil.Deposit(accounts, 1000);
        AccountUtil.Withdraw(accounts, 2000);
        AccountUtil.Withdraw(accounts, 2000);

        var trustAccounts = new List<Account>
    {
        new TrustAccount(),
        new TrustAccount("Superman2"),
        new TrustAccount("Batman2", 2000),
        new TrustAccount("Wonderwoman2", 5000, 5)
    };

        AccountUtil.Deposit(trustAccounts, 6000);
        AccountUtil.Withdraw(trustAccounts, 500);
        AccountUtil.Withdraw(trustAccounts, 500);
        AccountUtil.Withdraw(trustAccounts, 500);
        AccountUtil.Withdraw(trustAccounts, 500);

        Console.WriteLine();
    }
}

