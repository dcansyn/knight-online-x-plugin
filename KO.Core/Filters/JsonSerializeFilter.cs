using KO.Core.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Reflection;

namespace KO.Core.Filters
{
    // ♥
    public class JsonSerializeFilter : JsonSerializerSettings
    {
        public JsonSerializeFilter(params string[] members)
        {
            ContractResolver = new CustomContractResolver(members);
        }

        public class CustomContractResolver : DefaultContractResolver
        {
            public string[] Members { get; set; }

            public CustomContractResolver(params string[] members)
            {
                Members = members;
            }

            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                JsonProperty property = base.CreateProperty(member, memberSerialization);

                if (Members?.Length > 0)
                    property.ShouldSerialize = instance => { return Members.Contains(member.Name) || Members.Contains($"{property.DeclaringType.Name}.{member.Name}"); };
                else
                    property.ShouldSerialize = instance => { return member.GetCustomAttribute<IgnoreAttribute>() == null; };

                return property;
            }
        }
    }
}
