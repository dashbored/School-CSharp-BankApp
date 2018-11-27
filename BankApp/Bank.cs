using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace BankApp
{
    public class Bank
    {
        public List<Customer> Customers { get; private set; }
        ReadWrite textParser = new ReadWrite();
        const decimal DELTA = 0.01m;

        public Bank()
        {
            Customers = null;
        }

        public Bank(List<Customer> customers)
        {
            Customers = customers;
        }

        public decimal TotalBalanceOfAllAccounts()
        {
            decimal result = 0.0m;

            foreach (var customer in Customers)
            {
                foreach (var account in customer.Accounts)
                {
                    result += account.Balance;
                }
            }
            return result;
        }

        public bool Transfer(Account withdrawAccount, Account depositAccount, decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Summan måste vara störren än 0.");
                return false;
            }
            if ((Math.Round(withdrawAccount.Balance, 2) - amount) >= DELTA && (amount > 0))
            {
                withdrawAccount.Transfer(depositAccount, amount);
                return true;
            }
            else if (withdrawAccount.AlmostEqual(withdrawAccount.Balance, amount, DELTA))
            {
                withdrawAccount.Transfer(depositAccount, withdrawAccount.Balance);
                return true;
            }
            else if (withdrawAccount.CreditAccount && ((withdrawAccount.Balance - amount) > -withdrawAccount.CreditLimit))
            {
                withdrawAccount.Transfer(depositAccount, amount);
                return true;
            }
            else if (!(withdrawAccount.Balance >= amount))
            {
                Console.WriteLine("Det finns inte tillräckligt med pengar på det kontot.");
                return false;
            }
            else
            {
                Console.WriteLine("Det går inte att göra det.");
                return false;
            }


            /*
            public void Transfer(Account withdrawAccount, Account depositAccount, decimal amount)
            {

                if ((withdrawAccount.Balance >= amount) && (amount > 0))
                {
                    withdrawAccount.Withdraw(amount.ToString());
                    depositAccount.Deposit(amount.ToString());
                    Console.WriteLine("{0:C} har överförts från konto {1} till {2}.", amount.ToString("C", new CultureInfo("en-SE")), withdrawAccount.AccountNumber, depositAccount.AccountNumber);
                    //textParser.TransactionNoter(amount, depositAccount, withdrawAccount);
                    withdrawAccount.AddTransaction(new Transaction(amount, "transfer", withdrawAccount, depositAccount));
                    depositAccount.AddTransaction(new Transaction(amount, "transfer", withdrawAccount, depositAccount));
                    //transactions.Add(new Transaction(amount, "transfer", withdrawAccount, depositAccount));
                    return;
                }

                if (!(withdrawAccount.Balance >= amount))
                {
                    Console.WriteLine("Det finns inte tillräckligt med pengar på det kontot.");
                }

                if (!(amount > 0))
                {
                    Console.WriteLine("Mängden måste vara större än 0.");
                }
            }
            */
        }

        public void AddCustomer(Customer customer)
        {
            Customers.Add(customer);
        }

        public void RemoveCustomer(Customer customer)
        {
            Customers.Remove(customer);
        }

    }
}
