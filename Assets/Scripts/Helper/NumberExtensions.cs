using System;
using System.Globalization;

public static class NumberExtensions
{
    private static string FormatShortNumber(double number, int decimalPlaces)
    {
        string[] suffixes = { "", "K", "M", "B", "T", "Q" };
        int suffixIndex = 0;
        double absNumber = Math.Abs(number);

        while (absNumber >= 1000 && suffixIndex < suffixes.Length - 1)
        {
            absNumber /= 1000;
            suffixIndex++;
        }

        string format = "0." + new string('#', decimalPlaces);
        string shortNumber = absNumber.ToString(format, CultureInfo.InvariantCulture);
        return (number < 0 ? "-" : "") + shortNumber + suffixes[suffixIndex];
    }

    public static string ToShortNumberString(this double number, int decimalPlaces = 2)
        => FormatShortNumber(number, decimalPlaces);

    public static string ToShortNumberString(this float number, int decimalPlaces = 2)
        => FormatShortNumber(number, decimalPlaces);
    public static string ToShortNumberString(this int number, int decimalPlaces = 2)
        => FormatShortNumber(number, decimalPlaces);
    public static string ToShortNumberString(this long number, int decimalPlaces = 2)
        => FormatShortNumber(number, decimalPlaces);
    public static string ToShortNumberString(this decimal number, int decimalPlaces = 2)
        => FormatShortNumber((double)number, decimalPlaces);
    public static string ToShortNumberString(this string input, int decimalPlaces = 2)
    {
        if (double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
        {
            return FormatShortNumber(number, decimalPlaces);
        }

        return input;
    }
}