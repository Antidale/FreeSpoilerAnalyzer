using System.ComponentModel;

namespace FreeSpoilerAnalyzer.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            if (field == null)
            {
                return enumValue.ToString();
            }

            var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes != null && attributes.Length > 0
                ? attributes[0].Description
                : enumValue.ToString();
        }

        /// <summary>
        /// <para>Returns all attributes of the given type decorating the enum value as a Span.</para>
        /// <para>if you're uncertain about whether or not the enum value has been decorated, use HasAttribute<T> before this</para>
        /// </summary>
        /// <typeparam name="T">The Attribute type you are getting</typeparam>
        /// <param name="enumValue">The enum value to get the attribute for</param>
        /// <returns>Returns the first of any attribute of the given type on the Enum Value</returns>
        public static T GetAttribute<T>(this Enum enumValue) where T : Attribute, new()
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            if (field == null)
                return new T();

            var attributes = (T[])field.GetCustomAttributes(typeof(T), false);

            return attributes != null && attributes.Length > 0
                ? attributes[0]
                : new T();
        }

        /// <summary>
        /// <para>Returns all attributes of the given type decorating the enum value as a Span.</para>
        /// <para>if you're uncertain about whether or not the enum value has been decorated, use HasAttribute<T> before this</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue"></param>
        /// <returns>Returns all attributes of the given type decorating the enum value as a Span</returns>
        public static Span<T> GetAttributes<T>(this Enum enumValue) where T : Attribute, new()
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            if (field == null)
                return [];

            var attributes = (T[])field.GetCustomAttributes(typeof(T), false);

            return attributes is null 
                ? [] 
                : attributes.AsSpan<T>();
        }

        /// <summary>
        /// Checks for the presense of the given attribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue"></param>
        /// <returns>a bool representing if there is at least one of the given attribute decorating the enum</returns>
        public static bool HasAttribute<T>(this Enum enumValue) where T : Attribute
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            if (field == null)
                return false;

            var attributes = (T[])field.GetCustomAttributes(typeof(T), false);

            return attributes != null && attributes.Length > 0;
        }
    }
}
