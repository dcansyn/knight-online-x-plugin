using KO.Application.Addresses.Extensions;
using KO.Application.Characters.Extensions;
using KO.Core.Constants;
using KO.Core.Extensions;
using KO.Domain.Characters;
using KO.Domain.Parties;
using KO.Domain.Skills;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KO.Application.Skills.Extensions
{
    public static class SkillExtensions
    {
        public static int GetSkillId(this Character character, object skillId)
        {
            return Convert.ToInt32($"{character.GetCharacterClassId()}{skillId}");
        }

        public static int GetUsedSkillCount(this Character character)
        {
            return character.ReadLong(character.ReadLong(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_USE_SKILL_BASE) + 0x4) + Settings.KO_OFF_USE_SKILL_ID + 0x4);
        }

        public static int GetUsedSkillByIndex(this Character character, int index)
        {
            var result = character.ReadLong(character.ReadLong(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_USE_SKILL_BASE) + 0x4) + Settings.KO_OFF_USE_SKILL_ID);

            for (int i = 1; i <= index; i++)
                result = character.ReadLong(result);

            return character.ReadLong(result + 0x8) > 0 ? character.ReadLong(character.ReadLong(result + 0x8)) : 0;
        }

        public static List<int> GetAllUsedSkill(this Character character)
        {
            var result = new List<int>();

            for (int i = 1; i <= character.GetUsedSkillCount(); i++)
                result.Add(character.GetUsedSkillByIndex(i));

            return result;
        }

        public static bool GetSkillExists(this Character character, int id)
        {
            return character.GetAllUsedSkill().Contains(id);
        }

        public static bool GetTransformationScrollExists(this Character character, int skillId = 472)
        {
            for (int i = 1; i <= character.GetUsedSkillCount(); i++)
                if (skillId == Convert.ToInt32(character.GetUsedSkillByIndex(i) / 1000))
                    return true;

            return false;
        }

        public static bool GetSkillExists(this Character character, string id)
        {
            for (int i = 1; i <= character.GetUsedSkillCount(); i++)
            {
                string strSkill = character.GetUsedSkillByIndex(i).ToString();
                if (strSkill.Substring(strSkill.Length - 3) == id)
                    return true;
            }
            return false;
        }

        public static bool GetBufferExists(this Character character, BuffSkillType buffType)
        {
            return character.GetSkillExists((int)buffType);
        }

        public static bool GetPartyBufferExists(this Character character)
        {
            return BufferSkillType.Closed.List().Any(x => character.GetSkillExists(x.Group));
        }

        public static bool GetPartyDefenceExists(this Character character)
        {
            return DefenceSkillType.Closed.List().Any(x => character.GetSkillExists(x.Group));
        }

        public static bool GetPartyRestoreExists(this Character character)
        {
            return RestoreSkillType.Closed.List().Any(x => character.GetSkillExists(x.Group));
        }

        public static bool GetPartyResistanceExists(this Character character)
        {
            return ResistanceSkillType.Closed.List().Any(x => character.GetSkillExists(x.Group));
        }

        public static bool GetPartyStrengthExists(this Character character)
        {
            return character.GetSkillExists("004");
        }

        public static bool GetPartyCureStatus(this Character character)
        {
            return new[]
            {
                111703, // Malice
                111715, // Confusion
                111724, // Slow
                111745, // Parasite
                111757, // Torment
                111760, // Massive

                211703, // Malice
                211715, // Confusion
                211724, // Slow
                211745, // Parasite
                211757, // Torment
                211760, // Massive

                112703, // Malice
                112715, // Confusion
                112724, // Slow
                112745, // Parasite
                112757, // Torment
                112760, // Massive

                212703, // Malice
                212715, // Confusion
                212724, // Slow
                212745, // Parasite
                212757, // Torment
                212760, // Massive
            }.Any(x => character.GetSkillExists(x));
        }

        public static bool GetPartyReverseLifeStatus(this Character character)
        {
            return character.GetSkillExists("727");
        }

        public static bool GetPartyDiseaseStatus(this Character character)
        {
            return new[]
            {
                109509, // Blaze
                109518, // Ignition
                109539, // Hell Fire
                109557, // Fire Impact
                109560, // Super Nova
                109571, // Meteor Fall
                109574, // Vampiric Fire
                109609, // Chill
                109618, // Solid
                109639, // Frostbite
                109657, // Frost Impact
                109660, // Frost Nova
                109670, // Prismatic
                109671, // Ice Storm
                109709, // Counter Spell
                109718, // Static Hemisphe
                109739, // Discharge
                109757, // Thunder Impact Impact
                109760, // Static Nova
                109771, // Chain Lightning

                209509, // Blaze
                209518, // Ignition
                209539, // Hell Fire
                209557, // Fire Impact
                209560, // Super Nova
                209571, // Meteor Fall
                209574, // Vampiric Fire
                209609, // Chill
                209618, // Solid
                209639, // Frostbite
                209657, // Frost Impact
                209660, // Frost Nova
                209670, // Prismatic
                209671, // Ice Storm
                209709, // Counter Spell
                209718, // Static Hemisphe
                209739, // Discharge
                209757, // Thunder Impact Impact
                209760, // Static Nova
                209771, // Chain Lightning

                110509, // Blaze
                110518, // Ignition
                110539, // Hell Fire
                110557, // Fire Impact
                110560, // Super Nova
                110571, // Meteor Fall
                110574, // Vampiric Fire
                110609, // Chill
                110618, // Solid
                110639, // Frostbite
                110657, // Frost Impact
                110660, // Frost Nova
                110670, // Prismatic
                110671, // Ice Storm
                110709, // Counter Spell
                110718, // Static Hemisphe
                110739, // Discharge
                110757, // Thunder Impact Impact
                110760, // Static Nova
                110771, // Chain Lightning

                210509, // Blaze
                210518, // Ignition
                210539, // Hell Fire
                210557, // Fire Impact
                210560, // Super Nova
                210571, // Meteor Fall
                210574, // Vampiric Fire
                210609, // Chill
                210618, // Solid
                210639, // Frostbite
                210657, // Frost Impact
                210660, // Frost Nova
                210670, // Prismatic
                210671, // Ice Storm
                210709, // Counter Spell
                210718, // Static Hemisphe
                210739, // Discharge
                210757, // Thunder Impact Impact
                210760, // Static Nova
                210771, // Chain Lightning
            }.Any(x => character.GetSkillExists(x));
        }
    }
}
