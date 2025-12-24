using System.Globalization;
using UnityEngine;

public static class NumberFormatter
{
    private static CultureInfo s_culture = CultureInfo.InvariantCulture;
    private static readonly string[] s_suffixes = { "", "K", "M", "B", "T" };

    public static string FormatNumber(float value)
    {
        if (Mathf.Approximately(value, 0f))
            return "0";

        float absoluteValue = Mathf.Abs(value);
        int suffixIndex = 0;

        while (absoluteValue >= 1000f && suffixIndex < s_suffixes.Length - 1)
        {
            absoluteValue /= 1000f;
            suffixIndex++;
        }

        string formattedValue = FormatWithAppropriateDecimals(absoluteValue);

        if (value < 0f)
            return $"-{formattedValue}{s_suffixes[suffixIndex]}";

        return $"{formattedValue}{s_suffixes[suffixIndex]}";
    }

    private static string FormatWithAppropriateDecimals(float value)
    {
        if (Mathf.Approximately(value, Mathf.Round(value)))
            return Mathf.Round(value).ToString("0", s_culture);

        if (value < 10f)
        {
            string formatted = value.ToString("F2", s_culture);
            return TrimTrailingZeros(formatted);
        }

        if (value < 100f)
        {
            string formatted = value.ToString("F1", s_culture);
            return TrimTrailingZeros(formatted);
        }

        return Mathf.Round(value).ToString("0", s_culture);
    }

    private static string TrimTrailingZeros(string value)
    {
        if (value.Contains('.') == false)
            return value;

        int trimLength = value.Length;

        while (trimLength > 0 && value[trimLength - 1] == '0')
            trimLength--;

        if (trimLength > 0 && value[trimLength - 1] == '.')
            trimLength--;

        return value[..trimLength];
    }
}