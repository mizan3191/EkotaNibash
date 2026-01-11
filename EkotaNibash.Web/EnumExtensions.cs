using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EkotaNibash.Web
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this System.Enum enumValue)
        {
            try
            {
                if (enumValue != null)
                {
                    return enumValue.GetType()
                                    .GetMember(enumValue.ToString())
                                    .First()
                                    .GetCustomAttribute<DisplayAttribute>()
                                    .GetName();
                }

                return string.Empty;
            }
            catch (System.Exception)
            {
                return string.Empty;
            }
        }

        public static string GetEnumCategoryAtrribute<T>(this T enumValue) where T : struct
        {
            if (!typeof(T).IsEnum)
                return null;

            var description = enumValue.ToString();
            var fieldInfo = enumValue.GetType().GetField(description);

            if (fieldInfo != null)
            {
                if (Attribute.GetCustomAttribute(fieldInfo, typeof(CategoryAttribute)) is CategoryAttribute attribute)
                {
                    description = attribute.Category;
                }
            }

            return description;
        }
    }
}
