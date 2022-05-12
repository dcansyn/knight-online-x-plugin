using KO.Core.Helpers.Zone;
using KO.Domain.Characters;
using SQLite.CodeFirst;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KO.Domain.Quests
{
    [Table("Quests")]
    public class Quest
    {
        [Autoincrement, Key]
        public int Id { get; protected set; }

        [Index]
        public int BaseId { get; protected set; } // 0
        public int UnknownNpcTypeId { get; protected set; } // 1
        public int RequiredLevel { get; protected set; } // 2
        public int RequiredExperience { get; protected set; } // 3
        public int RaceBaseId { get; protected set; } // 6
        public int QuestTypeId { get; protected set; } // 7
        public int ZoneId { get; protected set; } // 8
        public string Zone { get; protected set; }
        public int NpcId { get; protected set; } // 9
        public int MonsterExchangeId { get; protected set; } // 10
        public int ClassBaseId { get; protected set; } // 11
        public int QuestTalk1Id { get; protected set; } // 12
        public int ExchangeId { get; protected set; } // 14
        public int QuestTalk2Id { get; protected set; } // 15
        public string LuaName { get; protected set; } // 16
        public int GuideId { get; protected set; } // 17
        public int NpcRowId { get; protected set; } // 18
        public int IsQuestBaseId { get; protected set; } // 19

        [NotMapped]
        public QuestGuide Guide { get; protected set; }

        [NotMapped]
        public QuestNonPlayerCharacterDescription NpcDescription { get; protected set; }

        [NotMapped]
        public QuestItemExchange[] ItemExchanges { get; protected set; }

        public Quest() { }

        public Quest(int baseId,
            int unknownNpcTypeId,
            int requiredLevel,
            int requiredExperience,
            int raceBaseId,
            int questTypeId,
            int zoneId,
            int npcId,
            int monsterExchangeId,
            int classBaseId,
            int questTalk1Id,
            int exchangeId,
            int questTalk2Id,
            string luaName,
            int guideId,
            int npcRowId,
            int isQuestBaseId)
        {
            BaseId = baseId;
            UnknownNpcTypeId = unknownNpcTypeId;
            RequiredLevel = requiredLevel;
            RequiredExperience = requiredExperience;
            RaceBaseId = raceBaseId;
            QuestTypeId = questTypeId;
            ZoneId = zoneId;
            NpcId = npcId;
            MonsterExchangeId = monsterExchangeId;
            ClassBaseId = classBaseId;
            QuestTalk1Id = questTalk1Id;
            ExchangeId = exchangeId;
            QuestTalk2Id = questTalk2Id;
            LuaName = luaName;
            GuideId = guideId;
            NpcRowId = npcRowId;
            IsQuestBaseId = isQuestBaseId;

            Zone = ZoneHelper.GetNameById(ZoneId);
        }

        public CharacterClassType GetClassType()
        {
            return (CharacterClassType)ClassBaseId;
        }

        public CharacterRaceType GetRaceType()
        {
            return (CharacterRaceType)RaceBaseId;
        }

        public void UpdateGuide(QuestGuide guide)
        {
            Guide = guide;
        }

        public void UpdateNpcDescription(QuestNonPlayerCharacterDescription description)
        {
            NpcDescription = description;
        }

        public void UpdateItemExchanges(QuestItemExchange[] itemExchanges)
        {
            ItemExchanges = itemExchanges;
        }
    }

    public enum QuestFilterType
    {
        Level,
        Zone,
        Race,
        Class,
        Money,
        Experience,
        Hunt
    }

    public enum SealedSoulType
    {
        [Display(Name = "Go NPC")]
        GoNpc,
        [Display(Name = "Take Quest")]
        TakeQuest,
        [Display(Name = "Go Bowl")]
        GoBowl,
        [Display(Name = "Submit Bowl Quest")]
        SubmitMonsterQuest,
        [Display(Name = "Turn Back NPC")]
        TurnBack,
        [Display(Name = "Submit Quest")]
        SubmitQuest
    }

    public enum BlueChestActionType
    {
        [Display(Name = "Go NPC")]
        GoNpc,
        [Display(Name = "Take Quest")]
        TakeQuest,
        [Display(Name = "Go Enemy Zone")]
        GoEnemyZone,
        [Display(Name = "Go Enemy Quest NPC")]
        GoEnemyQuestNpc,
        [Display(Name = "Submit Enemy Quest")]
        SubmitEnemyQuest,
        [Display(Name = "Go Enemy Gate")]
        GoEnemyGate,
        [Display(Name = "Turn Back")]
        TurnBack,
        [Display(Name = "Town")]
        Town,
        [Display(Name = "Go NPC for Second Quest")]
        GoNpcForSecondQuest,
        [Display(Name = "Submit and Take Second Quest")]
        SubmitAndTakeSecondQuest,
        [Display(Name = "Go Gate")]
        GoGate,
        [Display(Name = "Go Ronark Land")]
        GoRonarkLand,
        [Display(Name = "Go NPC")]
        GoRonarkNpc,
        [Display(Name = "Take Quest")]
        TakeRonarkQuest,
        [Display(Name = "Use Hiding Skill")]
        UseHidingSkill,
        [Display(Name = "Go Enemy NPC")]
        GoEnemyRonarkLandNpc,
        [Display(Name = "Submit Enemy Quest")]
        SubmitEnemyRonarkLandQuest,
        [Display(Name = "Turn Back Town")]
        TurnBackRonarkLandTown,
        [Display(Name = "Submit Quest")]
        SubmitRonarkLandQuest,
        [Display(Name = "Go Gate")]
        GoRonarkLandGate,
        [Display(Name = "Turn Back Castle")]
        TurnBackCastle,
        [Display(Name = "Go Again")]
        GoAgain
    }
}
