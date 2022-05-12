using KO.Core.Attributes;
using KO.Domain.Characters;
using Newtonsoft.Json;
using SQLite.CodeFirst;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KO.Domain.Quests
{
    [Table("QuestNonPlayerCharacterExchanges")]
    public class QuestNonPlayerCharacterExchange
    {
        [Ignore]
        [Autoincrement, Key]
        public int Id { get; protected set; }

        [Ignore]
        public int BaseId { get; protected set; }

        [Ignore]
        [JsonProperty]
        public string NonPlayerCharacterJson { get; protected set; }

        [NotMapped]
        [JsonProperty("NPC List")]
        public NonPlayerCharacter[] NonPlayerCharacters => !string.IsNullOrEmpty(NonPlayerCharacterJson) ? JsonConvert.DeserializeObject<NonPlayerCharacter[]>(NonPlayerCharacterJson) : null;

        public QuestNonPlayerCharacterExchange() { }

        public QuestNonPlayerCharacterExchange(int baseId)
        {
            BaseId = baseId;
        }

        public void UpdateNonPlayerCharacter(NonPlayerCharacter[] nonPlayerCharacters)
        {
            NonPlayerCharacterJson = JsonConvert.SerializeObject(nonPlayerCharacters);
        }
    }
}
