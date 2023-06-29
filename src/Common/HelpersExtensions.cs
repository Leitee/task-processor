using System;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace TaskProcessor.Common
{
    public static partial class Helper
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
        private static Encoding DEFAULT_FORMAT = Encoding.UTF8;
        private static JsonSerializerOptions JSON_OPTIONS = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public static TEnum ToEnum<TEnum>(this string value)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, true);
        }

        public static TEnum ToEnum<TEnum>(this int value)
        {
            var name = Enum.GetName(typeof(TEnum), value);
            return name is null ? default : name.ToEnum<TEnum>();
        }

        public static byte[] EncodeToBase64(this string text) =>
            !string.IsNullOrEmpty(text) ?
            DEFAULT_FORMAT.GetBytes(text) :
            Array.Empty<byte>();

        public static string DecodeBase64(this byte[] payload) =>
            payload?.Any() is true ?
            DEFAULT_FORMAT.GetString(payload) :
            string.Empty;

        public static string EncodeToString(this string text) =>
            Convert.ToBase64String(text.EncodeToBase64());

        public static string DecodeToString(this string text) =>
            Convert.FromBase64String(text).DecodeBase64();

        public static TPayload DecodeToPayload<TPayload>(this string text) =>
            JsonSerializer.Deserialize<TPayload>(text, JSON_OPTIONS);

        public static string EncodePayload<TPayload>(this TPayload payload) =>
            JsonSerializer.Serialize(payload, JSON_OPTIONS);
    }
}
