using System;
using NumberToWordsWeb;

class Program
{
    static void Main()
    {
        Console.WriteLine("Running NumberToWordsWeb automated tests...\n");

        var tests = new (string Input, string Expected, bool ExpectError)[]
        {
            // Functional tests (1.1 Basic conversions)
            ("0", "ZERO DOLLARS", false),
            ("1", "ONE DOLLAR", false),
            ("12", "TWELVE DOLLARS", false),
            ("21", "TWENTY-ONE DOLLARS", false),
            ("105", "ONE HUNDRED AND FIVE DOLLARS", false),
            ("569457", "FIVE HUNDRED AND SIXTY-NINE THOUSAND FOUR HUNDRED AND FIFTY-SEVEN DOLLARS", false),
            ("1000000", "ONE MILLION DOLLARS", false),
            ("123.45", "ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS", false),
            ("0.99", "ZERO DOLLARS AND NINETY-NINE CENTS", false),
            ("1.01", "ONE DOLLAR AND ONE CENT", false),

            // Large numbers (1.2)
            ("999999999999.99", "NINE HUNDRED AND NINETY-NINE BILLION NINE HUNDRED AND NINETY-NINE MILLION NINE HUNDRED AND NINETY-NINE THOUSAND NINE HUNDRED AND NINETY-NINE DOLLARS AND NINETY-NINE CENTS", false),
            ("1001001001.01", "ONE BILLION ONE MILLION ONE THOUSAND ONE DOLLARS AND ONE CENT", false),

            // Negative values (1.3)
            ("-123.45", "MINUS ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS", false),

            // Invalid input (1.4)
            ("", "ERROR: Please enter an amount.", true),
            ("abc", "ERROR: Invalid numeric amount.", true)
        };

        int passed = 0, failed = 0;
        var converter = new NumberToWordsConverter();

        foreach (var (input, expected, expectError) in tests)
        {
            string actual;
            bool errorOccurred = false;

            try
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    throw new FormatException("Please enter an amount.");
                }

                decimal value = decimal.Parse(input);
                actual = converter.ConvertAmountToWords(value);
            }
            catch (Exception ex)
            {
                errorOccurred = true;
                actual = ex.Message.Contains("Please enter an amount.")
                    ? "ERROR: Please enter an amount."
                    : "ERROR: Invalid numeric amount.";
            }

            // Compare
            if (expectError == errorOccurred && actual == expected)
            {
                Console.WriteLine($"PASS: {input} -> {actual}");
                passed++;
            }
            else
            {
                Console.WriteLine($"FAIL: {input} -> {actual}");
                Console.WriteLine($"      Expected: {expected}");
                failed++;
            }
        }

        Console.WriteLine($"\nSummary: {passed} passed, {failed} failed");
        Console.WriteLine("Tests complete.");
    }
}
