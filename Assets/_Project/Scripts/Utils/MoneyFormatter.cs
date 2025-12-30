using System.Globalization;
using UnityEngine;

public static class MoneyFormatter
{
    private static CultureInfo s_culture = CultureInfo.InvariantCulture;
    private static readonly string[] s_suffixes = { "", "K", "M", "B", "T" };

    public static bool NeedUpdateText(out string formattedText, float lastValue, float currentValue)
    {
        if (Mathf.Approximately(lastValue, currentValue))
        {
            formattedText = string.Empty;
            return false;
        }

        int lastSuffixIndex = GetSuffixIndex(Mathf.Abs(lastValue));
        int currentSuffixIndex = GetSuffixIndex(Mathf.Abs(currentValue));

        if (lastSuffixIndex != currentSuffixIndex)
        {
            formattedText = FormatNumber(currentValue);
            return true;
        }

        float lastRounded = GetRoundedValue(lastValue, lastSuffixIndex);
        float currentRounded = GetRoundedValue(currentValue, currentSuffixIndex);

        if (Mathf.Approximately(lastRounded, currentRounded) == false)
        {
            formattedText = FormatNumber(currentValue);
            return true;
        }

        formattedText = string.Empty;

        return false;
    }

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

    private static int GetSuffixIndex(float absoluteValue)
    {
        int suffixIndex = 0;

        while (absoluteValue >= 1000f && suffixIndex < s_suffixes.Length - 1)
        {
            absoluteValue /= 1000f;
            suffixIndex++;
        }

        return suffixIndex;
    }

    private static float GetRoundedValue(float value, int suffixIndex)
    {
        float absoluteValue = Mathf.Abs(value);

        for (int i = 0; i < suffixIndex; i++)
            absoluteValue /= 1000f;

        if (absoluteValue < 10f)
            return (float)System.Math.Round(absoluteValue, 2);

        if (absoluteValue < 100f)
            return (float)System.Math.Round(absoluteValue, 1);

        return Mathf.Round(absoluteValue);
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