using System;
using System.Collections.Generic;
using System.Globalization;

public static class MoneyFormatConvert
{
    // Tỉ giá quy đổi (ví dụ minh họa, bạn có thể load từ API hoặc config)
    private static readonly Dictionary<string, decimal> exchangeRates = new Dictionary<string, decimal>()
    {
        { "USD", 1m },        // USD gốc
        { "VND", 24000m },    // 1 USD = 24,000 VND
        { "EUR", 0.9m },      // 1 USD = 0.9 EUR
        { "JPY", 150m }       // 1 USD = 150 JPY
    };

    // Map từ mã tiền tệ sang ký hiệu
    private static readonly Dictionary<string, string> currencySymbols = new Dictionary<string, string>()
    {
        { "USD", "$" },
        { "VND", "₫" },
        { "EUR", "€" },
        { "JPY", "¥" }
    };

    /// <summary>
    /// Chuyển đổi tiền tệ từ loại này sang loại khác.
    /// </summary>
    public static decimal Convert(decimal amount, string fromCurrency, string toCurrency)
    {
        if (!exchangeRates.ContainsKey(fromCurrency) || !exchangeRates.ContainsKey(toCurrency))
            throw new ArgumentException("Currency not supported");

        // Quy đổi về USD trước
        decimal inUSD = amount / exchangeRates[fromCurrency];
        // Quy đổi sang tiền đích
        return inUSD * exchangeRates[toCurrency];
    }

    /// <summary>
    /// Format số tiền có dấu phân cách hàng nghìn theo dạng "100.000"
    /// </summary>
    public static string FormatWithThousandSeparator(decimal amount)
    {
        NumberFormatInfo nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        nfi.NumberGroupSeparator = ".";
        nfi.NumberDecimalDigits = 0;
        return amount.ToString("N", nfi);
    }

    /// <summary>
    /// Format + kèm ký hiệu tiền tệ (VD: 100000 -> "100.000 ₫")
    /// </summary>
    public static string FormatCurrency(decimal amount, string currencyCode)
    {
        string formatted = FormatWithThousandSeparator(amount);

        if (currencySymbols.ContainsKey(currencyCode))
        {
            return $"{formatted} {currencySymbols[currencyCode]}";
        }
        else
        {
            // Nếu chưa định nghĩa ký hiệu thì in kèm mã code
            return $"{formatted} {currencyCode}";
        }
    }
}
