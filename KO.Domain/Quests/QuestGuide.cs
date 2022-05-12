using KO.Core.Attributes;
using Newtonsoft.Json;
using SQLite.CodeFirst;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KO.Domain.Quests
{
    [Table("QuestGuides")]
    public class QuestGuide
    {
        [Ignore]
        [Autoincrement, Key]
        public int Id { get; protected set; }

        [Index]
        [Ignore]
        public int BaseId { get; set; } // 0

        [JsonProperty("Minimum Level")]
        public int MinLevel { get; set; } // 1

        [JsonProperty("Maximum Level")]
        public int MaxLevel { get; set; } // 2

        public string Title { get; set; } // 3

        [JsonProperty("Sub Title")]
        public string SubTitle { get; set; } // 4 

        public string Description { get; set; } // 5

        public QuestGuide() { }

        public QuestGuide(int baseId, int minLevel, int maxLevel, string title, string subTitle, string description)
        {
            BaseId = baseId;
            MinLevel = minLevel;
            MaxLevel = maxLevel;
            Title = title;
            SubTitle = subTitle;
            Description = description;
        }
    }
}
