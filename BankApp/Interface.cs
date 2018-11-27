using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    public class Interface
    {
        Bank bank;
        ReadWrite textParser;
        string filePath;

        public Interface(Bank bank, string filePath)
        {
            this.bank = bank;
            textParser = new ReadWrite();
            this.filePath = filePath;
            StartUpMessage();
            ShowMenu();

        }

        private void StartUpMessage()
        {
            Console.WriteLine("************************");
            Console.WriteLine("* BANK APPLICATION 1.0 *");
            Console.WriteLine("************************");

            //TODO: skriv in en textrad som skriver att den läser in namn.txt
            Console.WriteLine("Läser in {0}", filePath);
            Statistics();
        }

        private void Statistics()
        {
            Console.WriteLine("Antal kunder: {0}", bank.Customers.Count);
            Console.WriteLine("Antal konton: {0}", (from c in bank.Customers
                                                    from a in c.Accounts
                                                    select a).ToList().Count);
            Console.WriteLine("Totalt saldo: {0}", bank.TotalBalanceOfAllAccounts().ToString("C", new CultureInfo("en-SE")));
        }

        private void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("HUVUDMENY");
            Console.WriteLine("0) Avsluta och spara");
            Console.WriteLine("1) Sök kund");
            Console.WriteLine("2) Visa kundbild");
            Console.WriteLine("3) Skapa kund");
            Console.WriteLine("4) Ta bort kund");
            Console.WriteLine("5) Skapa konto");
            Console.WriteLine("6) Ta bort konto");
            Console.WriteLine("7) Insättning");
            Console.WriteLine("8) Uttag");
            Console.WriteLine("9) Överföring");
            Console.WriteLine("10) Tillsätt räntor");
            Console.WriteLine("11) Sätt sparränta");
            Console.WriteLine("12) Sätt skuldränta");
            Console.WriteLine("13) Sätt kreditgräns");
            Console.WriteLine("14) Omvandla till kreditkonto");

            Console.WriteLine();
            Console.Write("> ");
            Parse(Console.ReadLine());
        }

        private void Parse(string inputString)
        {

            if (int.TryParse(inputString, out int input))
            {
                switch (input)
                {
                    case 0:
                        Exit();
                        break;
                    case 1:
                        SearchCustomer();
                        break;
                    case 2:
                        ShowCustomer();
                        break;
                    case 3:
                        CreateCustomer();
                        break;
                    case 4:
                        RemoveCustomer();
                        break;
                    case 5:
                        CreateAccount();
                        break;
                    case 6:
                        RemoveAccount();
                        break;
                    case 7:
                        Deposit();
                        break;
                    case 8:
                        Withdraw();
                        break;
                    case 9:
                        Transfer();
                        break;
                    case 10:
                        AddInterest();
                        break;
                    case 11:
                        SetSavingsRate();
                        break;
                    case 12:
                        SetDebtRate();
                        break;
                    case 13:
                        SetCreditLimit();
                        break;
                    case 14:
                        MakeCreditAccount();
                        break;
                    default:
                        break;
                }
            }
            ShowMenu();
        }

        private bool VerifyAccount(string input, out Account account)
        {
            if (int.TryParse(input, out var accountNumber))
            {
                account = (from c in bank.Customers
                           from a in c.Accounts
                           where a.AccountNumber == accountNumber
                           select a).SingleOrDefault();

                if (account == null)
                {
                    Console.WriteLine("Hittade inget konto med det nummret.");
                    return false;
                }
                return true;
            }
            Console.WriteLine("Du måste ange ett nummer.");
            account = null;
            return false;
        }

        private bool VerifyCustomer(string input, out Customer customer)
        {
            if (int.TryParse(input, out var customerNumber))
            {
                customer = (from c in bank.Customers
                            where c.CustomerNumber == customerNumber
                            select c).SingleOrDefault();

                if (customer == null)
                {
                    Console.WriteLine("Hittade inget konto med det nummret.");
                    return false;
                }
                return true;
            }
            Console.WriteLine("Du måste ange ett nummer.");
            customer = null;
            return false;
        }

        private void SetCreditLimit()
        {
            Console.WriteLine("* Sätt kreditgräns *");
            Console.Write("Kontonummer? ");
            CreditLimit(Console.ReadLine());
        }

        private void CreditLimit(string input)
        {
            if (VerifyAccount(input, out var account))
            {
                Console.Write("Kreditgräns? ");
                if (decimal.TryParse(Console.ReadLine(), out var creditLimit))
                {
                    account.SetCreditLimit(Math.Abs(creditLimit));
                    Console.WriteLine("Skuldränta satt till {0:C}", Math.Abs(creditLimit).ToString("C", new CultureInfo("en-SE")));
                    return;
                }
                Console.WriteLine("Du måste ange ett nummer.");
            }
        }

        private void SetDebtRate()
        {
            Console.WriteLine("* Sätt skuldränta *");
            Console.Write("Kontonummer? ");
            DebtRate(Console.ReadLine());
        }

        private void DebtRate(string input)
        {
            if (VerifyAccount(input, out var account))
            {
                if (!account.CreditAccount)
                {
                    Console.WriteLine("Kontot är inte ett kreditkonto.");
                    return;
                }

                Console.Write("Räntesats? (Ange i procent): ");
                if (decimal.TryParse(Console.ReadLine(), out var creditRate))
                {
                    if (creditRate > 0 && creditRate < 100)
                    {
                        account.SetCreditRate(creditRate);
                        Console.WriteLine("Räntesatsen satt till {0} %.", creditRate);
                        return;
                    }
                    Console.WriteLine("Räntan måste vara mellan 0 och 100.");
                    return;
                }
                Console.WriteLine("Du måste ange ett nummer.");
            }
        }

        private void SetSavingsRate()
        {
            Console.WriteLine("* Sätt Sparränta *");
            Console.Write("Kontonummer? ");
            if (VerifyAccount(Console.ReadLine(), out var account))
            {
                Console.WriteLine("Nuvarande räntesats: {0:P0}", account.SavingsRate);
                Console.Write("Ny räntesats? (Ange i procent): ");
                if (decimal.TryParse(Console.ReadLine(), out var interestRate))
                {
                    if (interestRate > 0 && interestRate < 100)
                    {
                        account.SetSavingsRate(interestRate);
                        return;
                    }
                    Console.WriteLine("Räntan måste vara mellan 0 och 100.");
                    return;
                }
                Console.WriteLine("Du måste ange ett nummer.");
            }
        }

        private void MakeCreditAccount()
        {
            Console.WriteLine("* Omvandla till kreditkonto *");
            Console.Write("Kontonummer? ");
            if (VerifyAccount(Console.ReadLine(), out var account))
            {
                if (account.CreditAccount)
                {
                    Console.WriteLine("Kontot är redan ett kreditkonto.");
                    return;
                }
                account.MakeCreditAccount();
                Console.WriteLine("Kontonummer {0} är nu ett kreditkonto.", account.AccountNumber);
                CreditLimit(account.AccountNumber.ToString());
                DebtRate(account.AccountNumber.ToString());
            }
        }

        private void AddInterest()
        {
            Console.WriteLine("* Tillsätt räntor *");
            Console.WriteLine("Vänligen vänta...");
            var accounts = (from c in bank.Customers
                            from a in c.Accounts
                            select a).ToList();
            foreach (var account in accounts)
            {
                var amount = account.AddInterest();
                account.AddTransaction(new Transaction(amount, "interest", account));
            }
            Console.WriteLine("Räntor tillagda på konton.");
        }

        private void Exit()
        {
            string filePath = textParser.SetDataInFile(bank);
            Console.WriteLine("* Avsluta och spara *");
            Console.WriteLine("Sparar till {0}", filePath);
            Statistics();
            Console.ReadLine();
            Environment.Exit(0);
        }

        private void Transfer()
        {
            Console.WriteLine("* Överföring *");
            Console.Write("Från kontonummer? ");
            if (VerifyAccount(Console.ReadLine(), out var withdrawAccount))
            {
                Console.Write("Till kontonummer? ");
                if (VerifyAccount(Console.ReadLine(), out var depositAccount))
                {
                    Console.Write("Belopp? ");

                    if (decimal.TryParse(Console.ReadLine(), out var amount))
                    {
                        if (bank.Transfer(withdrawAccount, depositAccount, amount))
                        {
                            Console.WriteLine("{0:C} har överförts från konto {1} till {2}.", amount.ToString("C", new CultureInfo("en-SE")), withdrawAccount.AccountNumber, depositAccount.AccountNumber);

                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Du måste skriva in en siffra.");
                    }
                }
            }
        }

        private void Withdraw()
        {
            Console.WriteLine("* Uttag *");
            Console.Write("Kontonummer? ");
            if (VerifyAccount(Console.ReadLine(), out var account))
            {
                Console.Write("Summa? ");
                if (decimal.TryParse(Console.ReadLine(), out var amount))
                {
                    if (account.Withdraw(amount))
                    {
                        Console.WriteLine("{0:C} uttaget på konto {1}", amount.ToString("C", new CultureInfo("en-SE")), account.AccountNumber);
                    }
                }
                else
                {
                    Console.WriteLine("Du måste skriva in en siffra.");

                }

                Console.WriteLine("Kvarvarande saldo: {0:C}", account.Balance.ToString("C", new CultureInfo("en-SE")));
            }
        }

        private void Deposit()
        {
            Console.WriteLine("* Insättning *");
            Console.Write("Konto? ");
            if (VerifyAccount(Console.ReadLine(), out var account))
            {
                Console.Write("Summa? ");
                if (decimal.TryParse(Console.ReadLine(), out var amount))
                {
                    if (account.Deposit(amount))
                    {
                        Console.WriteLine("{0:C} insatt på konto {1}", amount.ToString("C", new CultureInfo("en-SE")), account.AccountNumber);
                    }
                    else
                    {
                        Console.WriteLine("Du måste ange ett värde större än 0.");

                    }
                }
                else
                {
                    Console.WriteLine("Du måste skriva in en siffra.");
                }
            }
        }

        private void RemoveAccount()
        {
            Console.WriteLine("* Ta bort konto *");
            Console.Write("Kontonummer? ");
            if (VerifyAccount(Console.ReadLine(), out var account))
            {
                var customer = (from c in bank.Customers
                                from a in c.Accounts
                                where a.AccountNumber == account.AccountNumber
                                select c).SingleOrDefault();

                if (customer.Accounts.Count <= 1)
                {
                    Console.WriteLine("Denna kund har bara ett konto. En kund måste alltid ha minst ett konto.");
                }
                else if (account.Balance < 0)
                {
                    Console.WriteLine("Det finns en skuld på detta konto. Betala av skulden ({0:C}) först för att kunna ta bort kontot.", account.Balance.ToString("C", new CultureInfo("en-SE")));
                }
                else if (account.Balance > 0)
                {
                    Console.WriteLine("Det finns pengar kvar på det här kontot. Överför eller plocka ut pengarna först.");
                }
                else
                {
                    customer.RemoveAccount(account);
                    Console.WriteLine("Kontonummer {0} borttaget från kund {1}", account.AccountNumber, customer.CustomerNumber);
                }
            }
        }

        private void CreateAccount()
        {
            Console.WriteLine("* Skapa konto *");

            Console.Write("Vilken kund tillhör kontot? Ange kundID: ");
            if (VerifyCustomer(Console.ReadLine(), out var customer))
            {
                var accountNumber = ((from c in bank.Customers
                                      from a in c.Accounts
                                      select a.AccountNumber).ToList().Max() + 1);
                customer.AddAccount(new Account(accountNumber, customer.CustomerNumber));
                Console.WriteLine("Ett nytt konto har skapats för kund {0}. Det nya kontots nummer är {1}.", customer.CustomerNumber, accountNumber);
            }
        }

        private void RemoveCustomer()
        {
            Console.WriteLine("* Ta bort kund *");
            Console.Write("Kund-Id? ");
            if (int.TryParse(Console.ReadLine(), out var customerIDToRemove))
            {
                var customer = (from c in bank.Customers
                                where c.CustomerNumber == customerIDToRemove
                                select c).SingleOrDefault();

                if (customer == null)
                {
                    Console.WriteLine("Det finns ingen kund med det kundnumret.");
                }
                else
                {
                    if (customer.TotalBalance() > 0)
                    {
                        Console.WriteLine("Det finns fortfarande pengar kvar hos denna kund. Överför pengarna först för att kunna ta bort kunden.");
                        return;
                    }
                    customer.ClearAccounts();
                    bank.RemoveCustomer(customer);
                    Console.WriteLine("Kund borttagen.");
                }
            }
            else
            {
                Console.WriteLine("Du måste skriva in en siffra.");
            }
        }

        private void CreateCustomer()
        {
            Console.WriteLine("* Skapa kund *");
            Customer customer = new Customer();

            customer.CustomerNumber = ((from c in bank.Customers
                                        select c.CustomerNumber).ToList().Max() + 1);

            Console.Write("Organisationsnummer? ");
            var input = Console.ReadLine();
            while (CheckIfValid(input, "organisationsnummer"))
            {
                input = Console.ReadLine();
            }
            customer.OrgNumber = input;

            Console.Write("Företagsnamn? ");
            input = Console.ReadLine();
            while (CheckIfValid(input, "företagsnamn"))
            {
                input = Console.ReadLine();
            }
            customer.BusinessName = input;

            Console.Write("Adress? ");
            input = Console.ReadLine();
            while (CheckIfValid(input, "adress"))
            {
                input = Console.ReadLine();
            }
            customer.Address = input;

            Console.Write("Stad? ");
            input = Console.ReadLine();
            while (CheckIfValid(input, "stad"))
            {
                input = Console.ReadLine();
            }
            customer.City = input;

            Console.Write("Region? ");
            input = Console.ReadLine();
            while (CheckIfValid(input, "region"))
            {
                input = Console.ReadLine();
            }
            customer.Region = input;

            Console.Write("Postnummer? ");
            input = Console.ReadLine();
            while (CheckIfValid(input, "postnummer"))
            {
                input = Console.ReadLine();
            }
            customer.PostNumber = input;

            Console.Write("Land? ");
            input = Console.ReadLine();
            while (CheckIfValid(input, "land"))
            {
                input = Console.ReadLine();
            }
            customer.Country = input;

            Console.Write("Telefonnummer? ");
            input = Console.ReadLine();
            while (CheckIfValid(input, "telefonnummer"))
            {
                input = Console.ReadLine();
            }
            customer.TelephoneNumber = input;

            bank.AddCustomer(customer);
            var accountNumber = ((from c in bank.Customers
                                  from a in c.Accounts
                                  select a.AccountNumber).ToList().Max() + 1);
            customer.AddAccount(new Account(accountNumber, customer.CustomerNumber));
            Console.WriteLine("Kund skapad! Nya kundens ID är {0}", customer.CustomerNumber);
        }

        private bool CheckIfValid(string input, string infoField)
        {
            if (input.Length < 1)
            {
                Console.Write("Vänligen skriv in ett/en {0}. ", infoField);
            }
            return input.Length < 1;
        }

        private void ShowCustomer()
        {
            Console.WriteLine("* Visa kundbild *");
            Console.Write("Kundnummer/Kontonummer? ");
            if (int.TryParse(Console.ReadLine(), out var input))
            {
                var customer = (from c in bank.Customers
                                where c.CustomerNumber == input
                                select c).SingleOrDefault();

                if (customer == null)
                {
                    customer = (from c in bank.Customers
                                from a in c.Accounts
                                where a.AccountNumber == input
                                select c).SingleOrDefault();
                }



                if (customer == null)
                {
                    Console.WriteLine("Det finns ingen kund med det kundnummret.");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Kundnummer: {0}", customer.CustomerNumber);
                    Console.WriteLine("Organisationsnummer: {0}", customer.OrgNumber);
                    Console.WriteLine("Namn: {0}", customer.BusinessName);
                    Console.WriteLine("Adress: {0}", customer.Address);
                    Console.WriteLine("Stad: {0}", customer.City);
                    Console.WriteLine("Region: {0}", customer.Region);
                    Console.WriteLine("Postnummer: {0}", customer.PostNumber);
                    Console.WriteLine("Land: {0}", customer.Country);
                    Console.WriteLine("Telefonnummer: {0}", customer.TelephoneNumber);
                    Console.WriteLine();

                    Console.WriteLine("Konton");
                    if (customer.Accounts.Count < 1)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Det finns inga konton hos den här kunden.");
                        return;
                    }
                    foreach (var account in customer.Accounts)
                    {
                        Console.WriteLine("{0} {1}: {2:C}", (account.CreditAccount ? "Kreditkonto" : "Debitkonto"), account.AccountNumber, account.Balance.ToString("C", new CultureInfo("en-SE")));
                        if (account.Transactions.Count > 0)
                        {
                            Console.WriteLine("Dagens historik för konto {0}:", account.AccountNumber);
                            foreach (var transaction in account.Transactions)
                            {
                                if (transaction.TransactionType == "interest" && Math.Abs(transaction.Amount) > 0)
                                {
                                    Console.WriteLine("{0}: {1:C} har {2} kontot i form av ränta.",
                                        transaction.TimeOfTransaction, transaction.Amount.ToString("C", new CultureInfo("en-SE")),
                                        ((transaction.Amount > 0) ? "lagts till" : "dragits från"));
                                }
                                else if (transaction.TransactionType == "withdraw")
                                {
                                    Console.WriteLine("{0}: {1:C} har tagits ut från kontot.", transaction.TimeOfTransaction, transaction.Amount.ToString("C", new CultureInfo("en-SE")));
                                }
                                else if (transaction.TransactionType == "deposit")
                                {
                                    Console.WriteLine("{0}: {1:C} har satts in på kontot.", transaction.TimeOfTransaction, transaction.Amount.ToString("C", new CultureInfo("en-SE")));
                                }
                                else if (transaction.TransactionType == "transfer")
                                {
                                    Console.WriteLine("{0}: {1:C} har överförts från konto {2} till konto {3}.",
                                        DateTime.Now.ToString("yyyy/MM/dd-HH:mm"),
                                        transaction.Amount.ToString("C", new CultureInfo("en-SE")),
                                        transaction.WithdrawAccount.AccountNumber,
                                        transaction.DepositAccount.AccountNumber);
                                }
                            }
                            Console.WriteLine();
                        }
                    }
                    Console.WriteLine("Totalt saldo: {0:C}", customer.TotalBalance().ToString("C", new CultureInfo("en-SE")));
                }
            }
            else
            {
                Console.WriteLine("Du måste skriva in ett nummer.");
            }
        }

        private void SearchCustomer()
        {
            Console.WriteLine("* Sök kund *");
            Console.Write("Namn eller region? ");
            var input = Console.ReadLine();

            var customerList = (from c in bank.Customers
                                where (c.BusinessName.IndexOf(input, StringComparison.InvariantCultureIgnoreCase) != -1) || (c.Region.IndexOf(input, StringComparison.InvariantCultureIgnoreCase) != -1)
                                select c).ToList();

            if (customerList.Count < 1)
            {
                Console.WriteLine("Hittade inga kunder med sökparametern \"{0}\".", input);
            }
            else
            {
                foreach (var customer in customerList)
                {
                    Console.WriteLine("{0}: {1}", customer.CustomerNumber, customer.BusinessName);
                }
            }

            Console.ReadLine();
        }
    }
}
