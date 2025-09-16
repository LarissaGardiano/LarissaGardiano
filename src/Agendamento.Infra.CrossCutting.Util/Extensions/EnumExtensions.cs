using System.ComponentModel;
using System.Reflection;

namespace Agendamento.Infra.CrossCutting.Util.Extensions
{
    public static class EnumExtensions
    {
        public static string PegarDescricao(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();

            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static T PegarEnumPorDescricao<T>(this string value, Enum defaultEnum) where T : Enum
        {
            Type type = typeof(T);

            foreach (T itemEnum in type.GetEnumValues())
            {
                string temtEnum = itemEnum.PegarDescricao();

                if (value == temtEnum)
                    return itemEnum;
            }

            return (T)defaultEnum;
        }
    }
}