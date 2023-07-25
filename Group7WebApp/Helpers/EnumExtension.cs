using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Group7WebApp.Helpers
{
    public static class EnumExtension
    {
       

        public static string GetDescription(this Enum value)
        {
            var enumMember = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            var descriptionAttribute =
                enumMember == null
                    ? default(DisplayAttribute)
                    : enumMember.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
            return
                descriptionAttribute == null
                    ? value.ToString()
                    : descriptionAttribute.Name;
        }


    }
}
