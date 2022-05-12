using KO.Core.Attributes;
using KO.Core.Extensions;
using KO.Core.Helpers.Zone;
using KO.Domain.Characters;
using Newtonsoft.Json;
using SQLite.CodeFirst;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KO.Domain.Quests
{
    [Table("QuestNonPlayerCharacterDescriptions")]
    public class QuestNonPlayerCharacterDescription
    {
        [Ignore]
        [Autoincrement, Key]
        public int Id { get; protected set; }

        [Index]
        [Ignore]
        public int BaseId { get; protected set; } // 0

        [Ignore]
        public int NpcId { get; protected set; } // 1

        [Ignore]
        public int RaceId { get; protected set; } // 3

        [JsonProperty("Race")]
        public string Race { get; protected set; }

        [Ignore]
        public int ZoneId { get; protected set; } // 4

        [JsonProperty("Zone")]
        public string Zone { get; protected set; } // 4

        [Ignore]
        public int NpcTypeId { get; protected set; } // 5 => 0,1 Monster, 2 NPC

        [JsonProperty("NPC Type")]
        public string NpcType { get; protected set; }

        [JsonProperty]
        public string Title { get; protected set; } // 6

        [JsonProperty("Title Explanation")]
        public string TitleExplanation { get; protected set; } // 7

        [JsonProperty("Location Description")]
        public string LocationDescription { get; protected set; } // 9

        [JsonProperty("X")]
        public int X1 { get; protected set; } // 10

        [JsonProperty("Y")]
        public int Y1 { get; protected set; } // 11

        [JsonProperty("Alternative X")]
        public int X2 { get; protected set; } // 12

        [JsonProperty("Alternative Y")]
        public int Y2 { get; protected set; } // 13

        public QuestNonPlayerCharacterDescription() { }

        public QuestNonPlayerCharacterDescription(int baseId,
            int npcId,
            int raceId,
            int zoneId,
            int npcTypeId,
            string title,
            string titleExplanation,
            string locationDescription,
            int x1,
            int y1,
            int x2,
            int y2)
        {
            BaseId = baseId;
            NpcId = npcId;
            RaceId = raceId;
            ZoneId = zoneId;
            NpcTypeId = npcTypeId;
            Title = title;
            TitleExplanation = titleExplanation;
            LocationDescription = locationDescription;
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;

            NpcType = NpcTypeId == 0 || NpcTypeId == 1 ? "Monster" : "Npc";
            Zone = ZoneHelper.GetNameById(ZoneId);
            Race = ((CharacterRaceType)RaceId).Get().DisplayName;
        }
    }
}
