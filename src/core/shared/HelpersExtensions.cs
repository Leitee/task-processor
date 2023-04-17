using System.Text;

namespace TaskProcessor.Core.Shared
{
    public static class Helper
    {
        /// <summary>
        /// Extension method to return an enum value of type TEnum for the given string.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TEnum ConvertToEnum<TEnum>(string value) where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, true);
        }

        /// <summary>
        /// Extension method to return an enum value of type TEnum for the given int.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TEnum ConvertToEnum<TEnum>(int value) where TEnum : struct
        {
            var name = Enum.GetName(typeof(TEnum), value);
            return name is null ? default : ConvertToEnum<TEnum>(name);
        }
    }

    public static class HelpersExtensions
    {
        public static TEnum ToEnum<TEnum>(this string value)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, true);
        }

        public static TEnum? ToEnum<TEnum>(this int value)
        {
            var name = Enum.GetName(typeof(TEnum), value);
            return name is null ? default : name.ToEnum<TEnum>();
        }

        public static string EncodeBase64(this string text, Encoding encoding)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            byte[] textAsBytes = encoding.GetBytes(text);
            return Convert.ToBase64String(textAsBytes);
        }

        public static string DecodeBase64(this string text, Encoding encoding)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            byte[] textAsBytes = Convert.FromBase64String(text);
            return encoding.GetString(textAsBytes);
        }
    }
}
