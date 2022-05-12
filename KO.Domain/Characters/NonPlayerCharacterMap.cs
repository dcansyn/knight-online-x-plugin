using KO.Core.Extensions;
using KO.Core.Helpers.Zone;
using Newtonsoft.Json;
using SQLite.CodeFirst;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KO.Domain.Characters
{
    [Table("NonPlayerCharacterMaps")]
    public class NonPlayerCharacterMap
    {
        [JsonIgnore]
        [Autoincrement, Key]
        public int Id { get; protected set; }

        [JsonIgnore]
        public int BaseId { get; protected set; } // 0

        [JsonIgnore]
        public int NpcId { get; protected set; } // 1

        [JsonIgnore]
        public int ZoneId { get; protected set; } // 2

        [JsonProperty("Zone")]
        public string Zone { get; protected set; } // 2

        [JsonIgnore]
        public int NpcTypeId { get; protected set; } // 3

        [JsonProperty("NPC Type")]
        public string NpcType { get; protected set; }

        [JsonProperty]
        public int X { get; protected set; } // 4

        [JsonProperty]
        public int Y { get; protected set; } // 5

        [JsonProperty]
        public string Name { get; protected set; } // 6

        [JsonProperty("Is Monster")]
        public bool IsMonster { get; protected set; } // 7

        [JsonIgnore]
        public int RaceId { get; protected set; } // 8

        [JsonProperty("Race")]
        public string Race { get; protected set; }

        [JsonProperty]
        public string Description { get; protected set; } // 9

        public NonPlayerCharacterMap() { }

        public NonPlayerCharacterMap(int baseId, int npcId, int zoneId, int npcType, int x, int y, string name, bool isMonster, int raceId, string description)
        {
            BaseId = baseId;
            NpcId = npcId;
            ZoneId = zoneId;
            NpcTypeId = npcType;
            X = x;
            Y = y;
            Name = name;
            IsMonster = isMonster;
            RaceId = raceId;
            Description = description;

            NpcType = NpcTypeId == 0 || NpcTypeId == 1 ? "Monster" : "Npc";
            Zone = ZoneHelper.GetNameById(ZoneId);
            Race = ((CharacterRaceType)RaceId).Get().DisplayName;
        }
    }
}
