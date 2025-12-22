using System.Globalization;
using UnityEngine;

public static class NumberFormatter
{
    private static readonly CultureInfo s_culture;
    private static readonly string[] s_suffixes = { "", "K", "M", "B", "T" };

    public static string FormatNumber(float value)
    {
        if (value == 0)
            return "0";

        float absoluteValue = Mathf.Abs(value);
        int suffixIndex = 0;

        while (absoluteValue >= 1000 && suffixIndex < s_suffixes.Length - 1)
        {
            absoluteValue /= 1000;
            suffixIndex++;
        }

        string sign = value < 0 ? "-" : "";
        string numberString = FormatWithAppropriateDecimals(absoluteValue);

        return $"{sign}{numberString}{s_suffixes[suffixIndex]}";
    }

    private static string FormatWithAppropriateDecimals(float value)
    {
        if (Mathf.Approximately(value, Mathf.Round(value)))
            return Mathf.Round(value).ToString(s_culture);

        if (value < 10)
            return value.ToString("F2", s_culture).TrimEnd('0').TrimEnd('.');

        if (value < 100)
            return value.ToString("F1", s_culture).TrimEnd('0').TrimEnd('.');

        return Mathf.Round(value).ToString(s_culture);
    }
}