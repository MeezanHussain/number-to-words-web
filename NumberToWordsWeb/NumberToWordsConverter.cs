using System;
using System.Text;

namespace NumberToWordsWeb
{
    public class NumberToWordsConverter
    {
        private static readonly string[] Ones =
        {
            string.Empty, "ONE", "TWO", "THREE", "FOUR", "FIVE",
            "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN",
            "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN",
            "SEVENTEEN", "EIGHTEEN", "NINETEEN"
        };

        private static readonly string[] Tens =
        {
            string.Empty, string.Empty, "TWENTY", "THIRTY", "FORTY",
            "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
        };

        private static readonly string[] Groups =
        {
            string.Empty, "THOUSAND", "MILLION", "BILLION", "TRILLION"
        };

        public string ConvertAmountToWords(decimal amount)
        {
            if (amount == 0m)
            {
                return "ZERO DOLLARS";
            }

            StringBuilder builder = new StringBuilder();

            // Handle negative amounts
            if (amount < 0)
            {
                builder.Append("MINUS ");
                amount = Math.Abs(amount);
            }

            // Separate dollars and cents
            long dollars = (long)Math.Truncate(amount);
            int cents = (int)Math.Round((amount - dollars) * 100m);

            // Convert dollars
            if (dollars > 0)
            {
                builder.Append(ConvertWholeNumberToWords(dollars));
                builder.Append(dollars == 1 ? " DOLLAR" : " DOLLARS");
            }
            else
            {
                // When there are only cents, we still want "ZERO DOLLARS"
                builder.Append("ZERO DOLLARS");
            }

            // Convert cents if present
            if (cents > 0)
            {
                builder.Append(" AND ");
                builder.Append(ConvertWholeNumberToWords(cents));
                builder.Append(cents == 1 ? " CENT" : " CENTS");
            }

            return builder.ToString().Trim();
        }
        private string ConvertWholeNumberToWords(long number)
        {
            if (number == 0)
            {
                return "ZERO";
            }

            StringBuilder result = new StringBuilder();
            int groupIndex = 0;

            while (number > 0)
            {
                int group = (int)(number % 1000);
                if (group != 0)
                {
                    string groupWords = ConvertThreeDigitGroupToWords(group);
                    if (result.Length > 0)
                    {
                        // Prepend a space before the next group
                        result.Insert(0, " ");
                    }
                    if (groupIndex < Groups.Length && !string.IsNullOrEmpty(Groups[groupIndex]))
                    {
                        if (groupWords.Length > 0)
                        {
                            groupWords = groupWords + " " + Groups[groupIndex];
                        }
                        else
                        {
                            groupWords = Groups[groupIndex];
                        }
                    }
                    result.Insert(0, groupWords);
                }
                number /= 1000;
                groupIndex++;
            }

            return result.ToString();
        }
        private string ConvertThreeDigitGroupToWords(int number)
        {
            StringBuilder words = new StringBuilder();

            int hundreds = number / 100;
            int remainder = number % 100;

            // Hundreds place
            if (hundreds > 0)
            {
                words.Append(Ones[hundreds]);
                words.Append(" HUNDRED");
                if (remainder > 0)
                {
                    words.Append(" AND ");
                }
            }

            // Tens and ones
            if (remainder > 0)
            {
                if (remainder < 20)
                {
                    words.Append(Ones[remainder]);
                }
                else
                {
                    int tensPlace = remainder / 10;
                    int onesPlace = remainder % 10;
                    words.Append(Tens[tensPlace]);
                    if (onesPlace > 0)
                    {
                        words.Append("-");
                        words.Append(Ones[onesPlace]);
                    }
                }
            }

            return words.ToString();
        }
    }
}