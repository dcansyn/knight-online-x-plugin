using KO.Application.Addresses.Extensions;
using KO.Core.Constants;
using KO.Core.Extensions;
using KO.Core.Helpers.Zone;
using KO.Domain.Characters;
using KO.Domain.Skills;
using System.Linq;

namespace KO.Application.Characters.Extensions
{
    public static class CharacterExtensions
    {
        public static int GetCharacterId(this Character character)
        {
            return character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_ID);
        }

        public static string GetCharacterName(this Character character)
        {
            int CharNickLen = character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_NAME_LENGTH);
            int CharNick = character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_NAME;
            if (CharNickLen > 15)
                return character.ReadByteArray(character.ReadLong(CharNick), CharNickLen).ConvertByteArrayToString();

            return character.ReadByteArray(CharNick, CharNickLen).ConvertByteArrayToString();
        }

        public static int GetCharacterLevel(this Character character)
        {
            return character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_LEVEL);
        }

        public static int GetCharacterX(this Character character)
        {
            return (int)character.ReadFloat(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_X);
        }

        public static int GetCharacterY(this Character character)
        {
            return (int)character.ReadFloat(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_Y);
        }

        public static int GetCharacterZ(this Character character)
        {
            return (int)character.ReadFloat(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_Z);
        }

        public static int GetCharacterMouseX(this Character character)
        {
            return (int)character.ReadFloat(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_MOUSE_X);
        }

        public static int GetCharacterMouseY(this Character character)
        {
            return (int)character.ReadFloat(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_MOUSE_Y);
        }

        public static int GetCharacterMouseZ(this Character character)
        {
            return (int)character.ReadFloat(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_MOUSE_Z);
        }

        public static int GetCharacterHealth(this Character character)
        {
            return character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_HP);
        }

        public static int GetCharacterMaxHealth(this Character character)
        {
            return character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_MAX_HP);
        }

        public static int GetCharacterMana(this Character character)
        {
            return character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_MP);
        }

        public static int GetCharacterMaxMana(this Character character)
        {
            return character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_MAX_MP);
        }

        public static int GetCharacterClassId(this Character character)
        {
            return character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_CLASS);
        }

        public static int GetCharacterExp(this Character character)
        {
            return character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_EXP);
        }

        public static int GetCharacterMaxExp(this Character character)
        {
            return character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_MAX_EXP);
        }

        public static int GetCharacterGold(this Character character)
        {
            return character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_GOLD);
        }

        public static int GetCharacterAttack(this Character character)
        {
            return character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_ATTACK);
        }

        public static int GetCharacterDefence(this Character character)
        {
            return character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_DEFENCE);
        }

        public static int GetCharacterWeight(this Character character)
        {
            return character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_WEIGHT);
        }

        public static int GetCharacterMaxWeight(this Character character)
        {
            return character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_MAX_WEIGHT);
        }

        public static int GetCharacterZoneId(this Character character)
        {
            return character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_ZONE);
        }

        public static string GetCharacterZone(this Character character)
        {
            return ZoneHelper.GetNameById(character.GetCharacterZoneId());
        }

        public static CharacterRaceType GetCharacterRaceType(this Character character)
        {
            switch (character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_NT))
            {
                case 0:
                    return CharacterRaceType.Monster;
                case 1:
                    return CharacterRaceType.Karus;
                case 2:
                    return CharacterRaceType.Human;
                default:
                    return CharacterRaceType.All;
            }
        }

        public static CharacterRaceType GetCharacterEnemyRaceType(this Character character)
        {
            return character.GetCharacterRaceType() == CharacterRaceType.Karus ? CharacterRaceType.Human : CharacterRaceType.Karus;
        }

        public static CharacterClassNameType GetCharacterClassNameType(this Character character)
        {
            switch (character.GetCharacterClassId())
            {
                case 101:
                    return CharacterClassNameType.Warrior;
                case 105:
                case 205:
                    return CharacterClassNameType.KnightWarrior;
                case 106:
                case 206:
                    return CharacterClassNameType.MasterWarrior;
                case 102:
                    return CharacterClassNameType.Rogue;
                case 107:
                case 207:
                    return CharacterClassNameType.KnightRogue;
                case 108:
                case 208:
                    return CharacterClassNameType.MasterRogue;
                case 103:
                    return CharacterClassNameType.Magician;
                case 109:
                case 209:
                    return CharacterClassNameType.KnightMagician;
                case 110:
                case 210:
                    return CharacterClassNameType.MasterMagician;
                case 104:
                    return CharacterClassNameType.Priest;
                case 111:
                case 211:
                    return CharacterClassNameType.KnightPriest;
                case 112:
                case 212:
                    return CharacterClassNameType.MasterPriest;
                case 113:
                case 213:
                    return CharacterClassNameType.Kurian;
                case 114:
                case 214:
                    return CharacterClassNameType.KnightKurian;
                case 115:
                case 215:
                    return CharacterClassNameType.MasterKurian;
                default:
                    return CharacterClassNameType.None;
            }
        }

        public static CharacterClassType GetCharacterClassType(this Character character)
        {
            switch (character.GetCharacterClassId())
            {
                case 101:
                case 105:
                case 205:
                case 106:
                case 206:
                    return CharacterClassType.Warrior;
                case 102:
                case 107:
                case 207:
                case 108:
                case 208:
                    return CharacterClassType.Rogue;
                case 103:
                case 109:
                case 209:
                case 110:
                case 210:
                    return CharacterClassType.Magician;
                case 104:
                case 111:
                case 211:
                case 112:
                case 212:
                    return CharacterClassType.Priest;
                case 113:
                case 213:
                case 114:
                case 214:
                case 115:
                case 215:
                    return CharacterClassType.Kurian;
                case 255:
                    return CharacterClassType.GM;
                default:
                    return CharacterClassType.None;
            }
        }

        public static CharacterMoveType GetTargetMoveType(this Character character)
        {
            var val = character.ReadByte(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_TARGET_MOVE);
            switch (val)
            {
                case 0:
                    return CharacterMoveType.Normal;
                case 1:
                    return CharacterMoveType.Attack;
                case 4:
                    return CharacterMoveType.Dead;
                default:
                    return CharacterMoveType.Unknown;
            }
        }

        public static CharacterStatusType GetCharacterStatusType(this Character character)
        {
            var val = character.ReadByte(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_TARGET_STATU);
            switch (val)
            {
                case 0:
                    return CharacterStatusType.Normal;
                case 1:
                    return CharacterStatusType.Moving;
                case 2:
                    return CharacterStatusType.Running;
                case 4:
                    return CharacterStatusType.Attack;
                case 10:
                    return CharacterStatusType.Dead;
                default:
                    return CharacterStatusType.Unknown;
            }
        }

        public static int GetCharacterStatPoint(this Character character, CharacterStatType stat)
        {
            switch (stat)
            {
                case CharacterStatType.Point:
                    return character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_STAT_POINT);
                case CharacterStatType.Strength:
                    return character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_STAT_STR);
                case CharacterStatType.Health:
                    return character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_STAT_HP);
                case CharacterStatType.Dexterity:
                    return character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_STAT_DEX);
                case CharacterStatType.Intelligence:
                    return character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_STAT_INT);
                case CharacterStatType.MagicPower:
                    return character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_STAT_MP);
                default:
                    return -1;
            }
        }

        public static CharacterStatType GetCharacterMaxStat(this Character character)
        {
            var maxResult = 0;
            var maxStat = CharacterStatType.Point;
            var stats = CharacterStatType.Point.List().Where(x => x.Value != 0).ToList();
            foreach (var item in stats)
            {
                var result = character.GetCharacterStatPoint((CharacterStatType)item.Self);
                if (result > maxResult)
                {
                    maxResult = result;
                    maxStat = (CharacterStatType)item.Self;
                }
            }
            return maxStat;
        }

        public static int GetCharacterSkillPoint(this Character character, CharacterSkillType skill)
        {
            switch (skill)
            {
                case CharacterSkillType.Point:
                    return character.ReadLong(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_SKILL_BASE) + Settings.KO_OFF_SKILL_POINT);
                case CharacterSkillType.Tab1:
                    return character.ReadLong(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_SKILL_BASE) + Settings.KO_OFF_SKILL_TAB1);
                case CharacterSkillType.Tab2:
                    return character.ReadLong(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_SKILL_BASE) + Settings.KO_OFF_SKILL_TAB2);
                case CharacterSkillType.Tab3:
                    return character.ReadLong(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_SKILL_BASE) + Settings.KO_OFF_SKILL_TAB3);
                case CharacterSkillType.Master:
                    return character.ReadLong(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_SKILL_BASE) + Settings.KO_OFF_SKILL_TAB4);
                default:
                    return -1;
            }
        }

        public static int GetCharacterSkillPointByRow(this Character character, int row)
        {
            return character.ReadLong(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_SKILL_BASE) + Settings.KO_OFF_SKILL_TAB1 + (4 * row));
        }

        public static CharacterSkillType GetCharacterMaxSkill(this Character character)
        {
            var maxResult = 0;
            var maxSkill = CharacterSkillType.Point;
            var stats = CharacterSkillType.Point.List().Where(x => x.Value != 0).ToList();
            foreach (var item in stats)
            {
                var result = character.GetCharacterSkillPoint((CharacterSkillType)item.Self);
                if (result > maxResult)
                {
                    maxResult = result;
                    maxSkill = (CharacterSkillType)item.Self;
                }
            }
            return maxSkill;
        }
    }
}
