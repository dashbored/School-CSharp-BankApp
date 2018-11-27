using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    public class Customer
    {
        public int CustomerNumber { get; set; }
        public string OrgNumber { get; set; }
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostNumber { get; set; }
        public string Country { get; set; }
        public string TelephoneNumber { get; set; }

        public List<Account> Accounts { get; private set; }

        public Customer()
        {
            Accounts = new List<Account>();
        }

        public void AddAccount(Account account)
        {
            Accounts.Add(account);
        }

        public void RemoveAccount(Account account)
        {
            Accounts.Remove(account);
        }

        public decimal TotalBalance()
        {
            var result = 0.0m;

            foreach (var account in Accounts)
            {
                result += account.Balance;
            }

            return result;
        }

        public void ClearAccounts()
        {
            Accounts.Clear();
        }
    }
}
