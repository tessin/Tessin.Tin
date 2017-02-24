using System;
using System.Diagnostics;
using Tessin.Tin.Sweden;

namespace Tessin.Tin
{
    public static class LuhnAlgorithm
    {

        /// <summary>
        /// Calculate the control digit based on a string of decimal digits.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>Control digit.</returns>
        public static string Calculate(string input)
        {
            // Check to see if the input string is numeric.
            if (string.IsNullOrEmpty(input) || !UtilSe.IsNumeric.IsMatch(input)) 
                throw new ArgumentException("The input string has to be be composed " +
                                            "of decimal digit characters ('0'-'9') " +
                                            "and be at least one character long.");
            var sum = 0;
            // For every character in the test string except the last.
            for (var n = 0; n < input.Length; n++)
            {
                // Get the n'th character.
                var digit = ToInt(input[n]);
                // if n is even
                if ((n % 2) == 0)
                {
                    // Multiply digit by two.
                    digit *= 2;
                    // If new digit value is more than one decimal digits.
                    if (digit > 9)
                    {
                        // Add the two decimal digits together.
                        digit = 1 + (digit - 10);
                    }
                }
                sum = sum + digit;
            }
            // Get the control digit from the checksum.
            var ctrl = 10 - sum % 10;
            ctrl = ctrl == 10 ? 0 : ctrl;
            return ctrl.ToString();
        }

        /// <summary>
        /// Tests whether the input string is 
        /// a valid Luhn-10 string.
        /// </summary>
        /// <param name="input">String to test.</param>
        /// <returns>True if valid. False if not.</returns>
        public static bool Test(string input)
        {
            // Check to see if the input string is numeric.
            if (string.IsNullOrEmpty(input) || !UtilSe.IsNumeric.IsMatch(input)) return false;
            return (Calculate(AllButLastChar(input))[0] == LastChar(input));
        }

        private static int ToInt(char c)
        {
            return (c - '0');
        }

        private static char LastChar(string input)
        {
            return input[input.Length - 1];
        }

        private static string AllButLastChar(string input)
        {
            Debug.Assert(input.Length > 1, "Input string cannot be less than 2 characters i.e. data digit plus control digit.");
            return input.Substring(0, input.Length - 1);
        }


    }
}
