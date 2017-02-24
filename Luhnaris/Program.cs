using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Luhnaris.Framework;

namespace Luhnaris
{
    class Program
    {
        static void Main(string[] args)
        {
            TextReader trfn = new StreamReader("fornamn.txt");
            TextReader tren = new StreamReader("efternamn.txt");

            List<string> numbers = LuhnGenerate.GeneratePnr(int.Parse(args[0]));
            foreach (var number in numbers)
            {
                string pnr = args[1] + number;
                string fn = trfn.ReadLine().Replace("'","''");
                string en = tren.ReadLine().Replace("'", "''");
                Console.OutputEncoding = new UTF8Encoding();
                Console.WriteLine(string.Format("UPDATE [TSL_TEST].[dbo].[Person] SET [Personnummer] = '{0}' WHERE [Fornamn] = '{1}' AND [Efternamn] = '{2}'", pnr, fn, en));
            }

            trfn.Close();
            tren.Close();
        }
    }
}
