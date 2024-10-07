using System;
using System.Linq;
using UnityEngine;

internal static class StringExtensions
{
    public static byte[] StringToByteArray(this string hex) {
        return Enumerable.Range(0, hex.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
            .ToArray();
    }

    public static Color HexToColor(this string hex)
    {
        var rgb = hex.StringToByteArray();
        return new Color(rgb[0]/255f, rgb[1]/255f, rgb[2]/255f);
    }
}