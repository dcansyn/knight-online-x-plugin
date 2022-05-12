using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace KO.Core.Extensions
{
    public class EnumModel
    {
        public object Self { get; set; }
        public int Value => Convert.ToInt32(Self);
        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public string Group { get; set; }
        public string ShortName { get; set; }
        public string Prompt { get; set; }
        public int Order { get; set; }
    }

    public class EnumAttributeModel<T> : EnumModel where T : Attribute
    {
        public T Attribute { get; set; }
    }

    public static class EnumExtensions
    {
        public static EnumModel Get(this Enum value)
        {
            var data = value.GetType().GetField(value.ToString());
            var display = data.GetCustomAttribute<DisplayAttribute>();

            return new EnumModel()
            {
                Self = value,
                Name = data.Name,
                DisplayName = display?.Name,
                Description = display?.Description,
                Group = display?.GroupName,
                ShortName = display?.ShortName,
                Prompt = display?.Prompt,
                Order = data.CustomAttributes.Any(c => c.NamedArguments.Where(n => n.MemberName == "Order").Any()) ? display.Order : 0
            };
        }

        public static EnumAttributeModel<T> Get<T>(this Enum value) where T : Attribute
        {
            var data = value.GetType().GetField(value.ToString());
            var display = data.GetCustomAttribute<DisplayAttribute>();
            var attribute = data.GetCustomAttribute<T>();

            return new EnumAttributeModel<T>()
            {
                Attribute = attribute,
                Self = value,
                Name = data.Name,
                DisplayName = display?.Name,
                Description = display?.Description,
                Group = display?.GroupName,
                ShortName = display?.ShortName,
                Prompt = display?.Prompt,
                Order = data.CustomAttributes.Any(c => c.NamedArguments.Where(n => n.MemberName == "Order").Any()) ? display.Order : 0
            };
        }

        public static EnumModel[] List(this Enum value)
        {
            return value.GetType().GetFields()
                .Where(x => x.IsStatic)
                .Select(x => new
                {
                    Data = x,
                    Display = x.GetCustomAttribute<DisplayAttribute>()
                })
                .Select(x => new EnumModel()
                {
                    Self = Enum.Parse(x.Data.DeclaringType, x.Data.Name, false),
                    Name = x.Data.Name,
                    DisplayName = x.Display?.Name,
                    Description = x.Display?.Description,
                    Group = x.Display?.GroupName,
                    ShortName = x.Display?.ShortName,
                    Prompt = x.Display?.Prompt,
                    Order = x.Data.CustomAttributes.Any(c => c.NamedArguments.Where(n => n.MemberName == "Order").Any()) ? x.Display.Order : 0
                })
                .OrderBy(x => x.Order)
                .ToArray();
        }

        public static EnumAttributeModel<T>[] List<T>(this Enum value) where T : Attribute
        {
            return value.GetType().GetFields()
                .Where(x => x.IsStatic)
                .Select(x => new
                {
                    Data = x,
                    Display = x.GetCustomAttribute<DisplayAttribute>()
                })
                .Select(x => new EnumAttributeModel<T>()
                {
                    Attribute = (T)x.Data.GetCustomAttribute(typeof(T)),
                    Self = Enum.Parse(x.Data.DeclaringType, x.Data.Name, false),
                    Name = x.Data.Name,
                    DisplayName = x.Display?.Name,
                    Description = x.Display?.Description,
                    Group = x.Display?.GroupName,
                    ShortName = x.Display?.ShortName,
                    Prompt = x.Display?.Prompt,
                    Order = x.Data.CustomAttributes.Any(c => c.NamedArguments.Where(n => n.MemberName == "Order").Any()) ? x.Display.Order : 0
                })
                .OrderBy(x => x.Order)
                .ToArray();
        }

        public static string[] StringList(this Enum value)
        {
            return Enum.GetValues(value.GetType()).Cast<Enum>().Select(x => x.ToString()).ToArray();
        }

        public static Enum[] All(this Enum value)
        {
            return Enum.GetValues(value.GetType()).Cast<Enum>().ToArray();
        }

        public static int Count(this Enum value)
        {
            return Enum.GetValues(value.GetType()).Cast<Enum>().Count();
        }

        public static bool Exists<T>(byte value)
        {
            return Enum.IsDefined(typeof(T), value);
        }
    }
}
