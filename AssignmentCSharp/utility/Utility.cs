using System;
using System.Security.Cryptography;
using System.Text;

namespace DemoCSharp.utility
{
    public class Utility
    {
        private static MD5CryptoServiceProvider algorith = new MD5CryptoServiceProvider();
        

        public static int GetIntNumber()
        {
            var number = 0;
            while (true)
            {
                try
                {
                    number = Int32.Parse(Console.ReadLine());
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Vui lòng nhập một số nguyên.");
                }
            }

            return number;
        }

        public static decimal GetUnsignedDecimalNumber()
        {
            decimal number;
            while (true)
            {
                try
                {
                    number = Decimal.Parse(Console.ReadLine());
                    if (number < 0)
                    {
                        throw new Exception();
                    }
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Vui lòng nhập số lớn hơn không.");
                }
            }

            return number;
        } 
    }
}