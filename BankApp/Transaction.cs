using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    public class Transaction
    {
        public string TransactionType { get; private set; }
        public decimal Amount { get; private set; }
        public Account WithdrawAccount { get; private set; }
        public Account DepositAccount { get; private set; }
        public string TimeOfTransaction { get; private set; }
        ReadWrite textParser = new ReadWrite();

        public Transaction(decimal amount, string type, Account account)
        {
            Amount = amount;
            TransactionType = type;
            TimeOfTransaction = DateTime.Now.ToString("yyyy/MM/dd-HH:mm");

            if (amount < 0)
            {
                WithdrawAccount = account;
                DepositAccount = null;
            }
            else
            {
                WithdrawAccount = null;
                DepositAccount = account;
            }
            textParser.TransactionNoter(amount, type, account);
        }

        public Transaction(decimal amount, string type, Account withdrawAccount, Account depositAccount)
        {
            Amount = amount;
            TransactionType = type;
            WithdrawAccount = withdrawAccount;
            DepositAccount = depositAccount;
            TimeOfTransaction = DateTime.Now.ToString("yyyy/MM/dd-HH:mm") + ".txt";
            textParser.TransactionNoter(amount, withdrawAccount, depositAccount);
        }
        public Transaction(decimal amount, string type, Account withdrawAccount, Account depositAccount, bool isDepositAccount)
        {
            Amount = amount;
            TransactionType = type;
            WithdrawAccount = withdrawAccount;
            DepositAccount = depositAccount;
            TimeOfTransaction = DateTime.Now.ToString("yyyy/MM/dd-HH:mm") + ".txt";
            //textParser.TransactionNoter(amount, withdrawAccount, depositAccount);
        }
    }
}
