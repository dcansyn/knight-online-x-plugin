using KO.Core.Attributes;
using KO.Domain.Quests;
using Newtonsoft.Json;
using SQLite.CodeFirst;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KO.Domain.Characters
{
    [Table("NonPlayerCharacters")]
    public class NonPlayerCharacter
    {
        [Ignore]
        [Autoincrement, Key]
        public int Id { get; protected set; }

        [Ignore]
        public int BaseId { get; protected set; } // 0

        [JsonProperty]
        public string Name { get; protected set; } // 1

        [JsonProperty("Is Boss")]
        public bool IsBoss { get; protected set; } // 3

        [Ignore]
        [JsonProperty]
        public string NonPlayerCharacterDescriptionJson { get; protected set; }

        [NotMapped]
        [JsonProperty("Description")]
        public QuestNonPlayerCharacterDescription NonPlayerCharacterDescription => !string.IsNullOrEmpty(NonPlayerCharacterDescriptionJson) ? JsonConvert.DeserializeObject<QuestNonPlayerCharacterDescription>(NonPlayerCharacterDescriptionJson) : null;

        public NonPlayerCharacter() { }

        public NonPlayerCharacter(int baseId, string name, bool isBoss)
        {
            BaseId = baseId;
            Name = name;
            IsBoss = isBoss;
        }

        public void UpdateNonPlayerCharacterDescription(QuestNonPlayerCharacterDescription questNonPlayerCharacterDescription)
        {
            NonPlayerCharacterDescriptionJson = JsonConvert.SerializeObject(questNonPlayerCharacterDescription);
        }
    }
}
