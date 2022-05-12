using System;
using System.Globalization;
using System.Linq;

namespace KO.Core.Extensions
{
    public static class ConvertExtensions
    {
        public static string ConvertToDword(this byte value, int length = 4)
        {
            return ConvertToDword((long)value, length);
        }

        public static string ConvertToDword(this int value, int length = 4)
        {
            return ConvertToDword((long)value, length);
        }

        public static string ConvertToDword(this long value, int length = 4)
        {
            var hex = string.Format("{0:x" + (2 * length).ToString() + "}", value);
            var result = "";
            for (int i = length - 1; i >= 0; i--)
                result += hex.Substring(i * 2, 2);

            return result.ToUpper();
        }

        public static int ConvertDwordToInt(this string value)
        {
            var result = "";
            for (int i = (value.Length / 2) - 1; i >= 0; i--)
                result += value.Substring(i * 2, 2);

            return Convert.ToInt32(result, 16);
        }

        public static string ConvertByteArrayToHex(this byte[] value)
        {
            return string.Join("", value.Select(x => x.ToString("x2"))).ToUpper();
        }

        public static string ConvertByteArrayToString(this byte[] value)
        {
            try
            {
                return string.Join("", value.Select(x => Convert.ToChar(x)));
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static byte[] ConvertStringToByteArray(this string value)
        {
            var tmpbyte = new byte[value.Length / 2];
            var sayac = 0;
            for (int i = 0; i < value.Length; i += 2)
            {
                tmpbyte[sayac] = byte.MinValue;
                if (byte.TryParse(value.Substring(i, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte val))
                    tmpbyte[sayac] = val;
                sayac++;
            }
            return tmpbyte;
        }

        public static int ConvertHexToInt(this string value)
        {
            return Convert.ToInt32(value, 16);
        }

        public static string ConvertIntToHex(this int value)
        {
            return value.ToString("x2").ToUpper();
        }

        public static string ConvertHexToDword(this string value)
        {
            return string.Join("", BitConverter.GetBytes(value.ConvertHexToInt()).Select(x => x.ToString("x2"))).ToUpper();
        }

        public static long ConvertFloatToLong(this float value)
        {
            return Convert.ToInt64(string.Join("", BitConverter.GetBytes(value).Reverse().Select(x => x.ToString("x2"))), 16);
        }

        public static string ConvertStringToHex(this string value)
        {
            return string.Join("", value.Select(x => string.Format("{0:x2}", Convert.ToUInt32(x)))).ToUpper();
        }

        public static string ConvertHexToString(this string value)
        {
            var result = "";
            for (int i = 0; i < value.Length; i += 2)
                result += Convert.ToChar(Convert.ToUInt32(value.Substring(i, 2), 16)).ToString();

            return result;
        }

        public static long GetAddressDestination(this long value, long destination)
        {
            return value - destination > 0 ? 0xFFFFFFFB - (value - destination) : destination - value - 5;
        }
    }
}
