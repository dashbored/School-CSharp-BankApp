using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    public class ReadWrite
    {
        public List<Customer> Customers { get; private set; }
        public List<Account> Accounts { get; private set; }

        public ReadWrite()
        {
            Customers = new List<Customer>();
            Accounts = new List<Account>();
        }

        public void GetDataFromFile(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                int nrOfCustomers = int.Parse(sr.ReadLine());

                for (int i = 0; i < nrOfCustomers; i++)
                {
                    string line = sr.ReadLine();
                    string[] entries = line.Split(';');

                    Customer customer = new Customer
                    {
                        CustomerNumber = int.Parse(entries[0], CultureInfo.InvariantCulture),
                        OrgNumber = entries[1],
                        BusinessName = entries[2],
                        Address = entries[3],
                        City = entries[4],
                        Region = entries[5],
                        PostNumber = entries[6],
                        Country = entries[7],
                        TelephoneNumber = entries[8]
                    };

                    Customers.Add(customer);
                }

                int nrOfBankAccounts = int.Parse(sr.ReadLine());

                for (int i = 0; i < nrOfBankAccounts; i++)
                {
                    Account account = new Account();
                    string line = sr.ReadLine();
                    string[] entries = line.Split(';');

                    account.SetAccountNumber(int.Parse(entries[0]));
                    account.SetCustomerNumber(int.Parse(entries[1]));
                    account.AddStartBalance(decimal.Parse(entries[2], CultureInfo.InvariantCulture));

                    Accounts.Add(account);
                }
            }
            SetAccountsToCustomer();
        }

        private void SetAccountsToCustomer()
        {
            foreach (var customer in Customers)
            {
                var accountsOfCustomer = (from a in Accounts
                                        where a.CustomerNumber == customer.CustomerNumber
                                        select a).ToList();

                foreach (var account in accountsOfCustomer)
                {
                    customer.AddAccount(account);
                }
            }
        }

        public string SetDataInFile(Bank bank)
        {
            string filePath = DateTime.Now.ToString("yyyyMMdd-HHmm") + ".txt";
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                char delimiter = ';';
                
                sw.WriteLine(bank.Customers.Count);
                foreach (var person in bank.Customers)
                {
                    sw.Write(person.CustomerNumber.ToString(CultureInfo.InvariantCulture) + delimiter);
                    sw.Write(person.OrgNumber.ToString(CultureInfo.InvariantCulture) + delimiter);
                    sw.Write(person.BusinessName.ToString(CultureInfo.InvariantCulture) + delimiter);
                    sw.Write(person.Address.ToString(CultureInfo.InvariantCulture) + delimiter);
                    sw.Write(person.City.ToString(CultureInfo.InvariantCulture) + delimiter);
                    sw.Write(person.Region.ToString(CultureInfo.InvariantCulture) + delimiter);
                    sw.Write(person.PostNumber.ToString(CultureInfo.InvariantCulture) + delimiter);
                    sw.Write(person.Country.ToString(CultureInfo.InvariantCulture) + delimiter);
                    sw.Write(person.TelephoneNumber.ToString(CultureInfo.InvariantCulture));
                    sw.WriteLine();
                }

                var accounts = (from c in bank.Customers
                                from a in c.Accounts
                                select a).ToList();
                sw.WriteLine(accounts.Count);
                foreach (var account in accounts)
                {
                    sw.Write(account.AccountNumber.ToString(CultureInfo.InvariantCulture) + delimiter);
                    sw.Write(account.CustomerNumber.ToString(CultureInfo.InvariantCulture) + delimiter);
                    sw.Write(account.Balance.ToString(CultureInfo.InvariantCulture));
                    sw.WriteLine();
                }
            }
            return filePath;
        }

        public void TransactionNoter(decimal amount, string type, Account account)
        {
            string filePath = DateTime.Now.ToString("yyyyMMdd") + ".txt";
            string transaction;

            if (type == "deposit")
            {
                transaction = String.Format("{0}: {1:C} har satts in på konto {2}. Kvarvarande saldo: {3:C};", DateTime.Now.ToString("yyyy/MM/dd-HH:mm"), amount.ToString("C", new CultureInfo("en-SE")), account.AccountNumber, account.Balance.ToString("C", new CultureInfo("en-SE")));
            }
            else if (type == "withdraw")
            {
                transaction = String.Format("{0}: {1:C} har tagits ut från konto {2}. Kvarvarande saldo: {3:C};", DateTime.Now.ToString("yyyy/MM/dd-HH:mm"), amount.ToString("C", new CultureInfo("en-SE")), account.AccountNumber, account.Balance.ToString("C", new CultureInfo("en-SE")));
            }
            else if (type == "interest")
            {
                transaction = String.Format("{0}: {1:C} har {3} konto {2}. Kvarvarande saldo: {4:C};", DateTime.Now.ToString("yyyy/MM/dd-HH:mm"), Math.Abs(amount).ToString("C", new CultureInfo("en-SE")), account.AccountNumber, ((amount >= 0) ? "lagts till" : "dragits från"), account.Balance.ToString("C", new CultureInfo("en-SE")));
            }
            else
            {
                throw new Exception();
            }

            if (File.Exists(filePath))
            {
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine(transaction);
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine(transaction);
                }
            }
        }

        public void TransactionNoter(decimal amount, Account withdrawAccount, Account depositAccount)
        {
            string filePath = DateTime.Now.ToString("yyyyMMdd") + ".txt";
            string transaction;

            transaction = String.Format("{0}: {1:C} har överförts från konto {2} till konto {3}. Kvarvarande saldo på konto {2}: {4:C}. Kvarvarande saldo på konto {3}: {5:C};", DateTime.Now.ToString("yyyy/MM/dd-HH:mm"), amount.ToString("C", new CultureInfo("en-SE")), withdrawAccount.AccountNumber, depositAccount.AccountNumber, withdrawAccount.Balance.ToString("C", new CultureInfo("en-SE")), depositAccount.Balance.ToString("C", new CultureInfo("en-SE")));

            if (File.Exists(filePath))
            {
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine(transaction);
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine(transaction);
                }
            }
        }
    }
}
