using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    public class Account
    {
        public int AccountNumber { get; private set; }
        public int CustomerNumber { get; private set; }
        public decimal Balance { get; private set; }
        public List<Transaction> Transactions { get; private set; }
        public decimal SavingsRate { get; private set; }
        const int DAYSINYEAR = 365;
        public bool CreditAccount { get; private set; }
        public decimal CreditLimit { get; private set; }
        public decimal CreditRate { get; private set; }

        const decimal DELTA = 0.01m;

        public Account()
        {
            Transactions = new List<Transaction>();
            Balance = 0.0m;
            SavingsRate = 0.03m;
            CreditRate = 0.0m;
            CreditAccount = false;
            CreditLimit = 0.0m;
        }

        public Account(int accountNumber, int customerNumber)
        {
            AccountNumber = accountNumber;
            CustomerNumber = customerNumber;
            Transactions = new List<Transaction>();
            Balance = 0.0m;
            SavingsRate = 0.0m;
            CreditRate = 0.0m;
            CreditAccount = false;
            CreditLimit = 0.0m;
        }

        public void SetAccountNumber(int accountNumber)
        {
            AccountNumber = accountNumber;
        }

        public void SetCustomerNumber(int customerNumber)
        {
            CustomerNumber = customerNumber;
        }

        public void AddStartBalance(decimal amount)
        {
            Balance += amount;
        }

        public bool Deposit(decimal amount)
        {
            if ((Balance - amount) <= DELTA && (Balance - amount) >= -DELTA)
            {
                Balance = 0.0m;
                return true;
            }
            else if (amount > 0)
            {
                Balance += amount;
                AddTransaction(new Transaction(amount, "deposit", this));
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Withdraw(decimal amount)
        {
            if (amount < 0)
            {
                Console.WriteLine("Du måste ange en summa större än 0.");
                return false;
            }
            if ((Math.Round(Balance, 2) - amount) >= DELTA)
            {
                Balance -= amount;
                AddTransaction(new Transaction(amount, "withdraw", this));

                return true;
            }
            else if (AlmostEqual(Balance, amount, DELTA))
            {
                Balance = 0;
                AddTransaction(new Transaction(amount, "withdraw", this));

                return true;
            }
            else
            {
                if (CreditAccount && ((Balance - amount) > -CreditLimit))
                {
                    Balance -= amount;
                    AddTransaction(new Transaction(amount, "withdraw", this));

                    return true;
                }
                Console.WriteLine("Det finns inte tillräckligt med pengar på kontot.");
                return false;
            }
        }

        public void Transfer(Account depositAccount, decimal amount)
        {
            Balance -= amount;
            depositAccount.TransferDeposit(amount, this);
            AddTransaction(new Transaction(amount, "transfer", this, depositAccount));
        }

        public void TransferDeposit(decimal amount, Account withdrawAccount)
        {
            Balance += amount;
            AddTransaction(new Transaction(amount, "transfer", withdrawAccount, this, false));
        }

        public bool AlmostEqual(decimal balance, decimal amount, decimal DELTA)
        {
            if (Math.Sqrt(Math.Pow(((double)(balance - amount)), 2)) <= (double)DELTA)
            {
                return true;
            }
            return false;
        }

        public decimal AddInterest()
        {
            if (Balance >= 0)
            {
                decimal interest = (Balance * SavingsRate) / DAYSINYEAR;
                Balance += interest;

                return interest;
            }
            else
            {
                decimal interest = (Balance * CreditRate) / DAYSINYEAR;
                Balance += interest;

                return interest;
            }
        }

        public void SetSavingsRate(decimal rate)
        {
            rate = rate / 100.0m;
            SavingsRate = rate;
        }

        public void MakeCreditAccount()
        {
            CreditAccount = true;
        }

        public void SetCreditRate(decimal rate)
        {
            rate = rate / 100.0m;
            CreditRate = rate;
        }

        public void SetCreditLimit(decimal creditLimit)
        {
            CreditLimit = creditLimit;
        }

        public void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
        }
    }
}
