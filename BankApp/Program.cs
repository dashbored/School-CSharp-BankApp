using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    public class Program
    {
        static void Main(string[] args)
        {

                if (args.Length < 1)
                {
                    Console.WriteLine("Du måste skicka in en textfil som ska läsas in.");
                    return;
                }

                Console.WriteLine("Kort (0) eller lång (1) textfil?");

            int value = int.Parse(Console.ReadLine());
            string filePath = args[value];

            ReadWrite textParser = new ReadWrite();
            textParser.GetDataFromFile(filePath);

            Bank bank = new Bank(textParser.Customers);
            Interface BankApp = new Interface(bank, filePath);

            Console.ReadLine();
        }
    }
}
