using KO.Application.Characters.Extensions;
using KO.Application.Skills.Extensions;
using KO.Application.Skills.Handlers;
using KO.Core.Extensions;
using KO.Domain.Characters;
using KO.Domain.Parties;
using KO.Domain.Skills;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KO.Application.Parties.Handlers
{
    public static class PartyActionHandler
    {
        private static readonly int SkillTime = 1000;
        private static Task HealTask = Task.CompletedTask;
        private static Task GroupHealTask = Task.CompletedTask;
        private static Task BufferTask = Task.CompletedTask;
        private static Task DefenceTask = Task.CompletedTask;
        private static Task RestoreTask = Task.CompletedTask;
        private static Task ResistanceTask = Task.CompletedTask;
        private static Task CureTask = Task.CompletedTask;

        public static async Task PartyAction(this Character character)
        {
            if (character.ClassType != CharacterClassType.Priest) return;

            await character.CollectPartyInformation();

            if (HealTask.IsCompleted)
                HealTask = character.PartyHeal();

            if (GroupHealTask.IsCompleted)
                GroupHealTask = character.PartyGroupHeal();

            if (BufferTask.IsCompleted)
                BufferTask = character.PartyBuffer();

            if (DefenceTask.IsCompleted)
                DefenceTask = character.PartyDefence();

            if (RestoreTask.IsCompleted)
                RestoreTask = character.PartyRestore();

            if (ResistanceTask.IsCompleted)
                ResistanceTask = character.PartyResistance();

            if (CureTask.IsCompleted)
                CureTask = character.PartyCure();
        }

        private static async Task PartyHeal(this Character character)
        {
            if (Client.Party.AutoEternity)
            {
                if (Client.PartyCharacters.Any(x => x.GetHealthLimit(Client.Party.HealPercent) && x.Distance < 50))
                {
                    var clientEternity = Client.Characters.FirstOrDefault(x => x.EternityExpireTime < DateTime.Now);
                    if (clientEternity != null)
                    {
                        await clientEternity.UseSkill(new Skill(492060, 9999, 20));
                        clientEternity.UpdateEternityExpireTime(2100);
                        return;
                    }
                }
            }

            if (DateTime.Now < character.PartySkillHealExpireTime) return;

            var healSkillType = Client.Party.HealSkillType.Get();
            var skillCode = healSkillType.Group;
            if (skillCode == "closed") return;

            foreach (var item in Client.PartyCharacters)
            {
                if (!item.GetHealthLimit(Client.Party.HealPercent) || item.Distance > 50 || DateTime.Now < character.PartySkillHealExpireTime) continue;

                switch (skillCode)
                {
                    default:
                        var defaultSkill = Client.PartySkills
                            .Where(x => !character.UsedSkills.Any(a => a.BaseId == x.BaseId))
                            .Where(x => x.BaseId == character.GetSkillId(Convert.ToInt32(skillCode)))
                            .Where(x => x.Point <= character.GetCharacterSkillPointByRow(x.Mastery))
                            .Where(x => x.Mana <= character.Mana)
                            .OrderByDescending(x => x.Cooldown)
                            .FirstOrDefault();

                        if (defaultSkill == null) continue;

                        await character.UseSkill(defaultSkill, item);
                        character.UpdatePartySkillHealExpireTime(SkillTime);
                        continue;

                    case "auto":
                        var autoSkillTypes = Client.Party.HealSkillType.List()
                            .Where(x => x.Order > 0)
                            .OrderBy(x => x.Order)
                            .ToArray();

                        var totalHeal = item.LastHealth;
                        foreach (var skillType in autoSkillTypes)
                        {
                            var autoSkill = Client.PartySkills
                                .Where(x => !character.UsedSkills.Any(a => a.BaseId == x.BaseId))
                                .Where(x => x.BaseId == character.GetSkillId(Convert.ToInt32(skillType.Group)))
                                .Where(x => x.Point <= character.GetCharacterSkillPointByRow(x.Mastery))
                                .Where(x => x.Mana <= character.Mana)
                                .OrderByDescending(x => x.Cooldown)
                                .FirstOrDefault();

                            if (autoSkill != null && totalHeal > 0)
                            {
                                await character.UseSkill(autoSkill, item);
                                character.UpdatePartySkillHealExpireTime(SkillTime);

                                totalHeal -= autoSkill.Extension.PotionValue;
                            }
                        }
                        break;
                }
            }
        }

        private static async Task PartyGroupHeal(this Character character)
        {
            if (DateTime.Now < character.PartySkillGroupHealExpireTime || !Client.Party.GroupHeal) return;

            var groupCharaters = Client.PartyCharacters.Where(x => x.GetHealthLimit(Client.Party.GroupHealPercent)).ToList();
            if (groupCharaters.Count < Client.Party.GroupHealCount) return;

            var maxLastHealth = groupCharaters.Max(x => x.LastHealth);
            var autoSkillTypes = GroupHealSkillType.GroupMassiveHealing.List()
                .Where(x => x.Order > 0)
                .OrderByDescending(x => x.Order)
                .ToArray();

            foreach (var autoSkillType in autoSkillTypes)
            {
                if (maxLastHealth > autoSkillType.Order)
                {
                    var autoSkill = Client.PartySkills
                        .Where(x => !character.UsedSkills.Any(a => a.BaseId == x.BaseId))
                        .Where(x => x.BaseId == character.GetSkillId(Convert.ToInt32(autoSkillType.Group)))
                        .Where(x => x.Point <= character.GetCharacterSkillPointByRow(x.Mastery))
                        .Where(x => x.Mana <= character.Mana)
                        .OrderByDescending(x => x.Cooldown)
                        .FirstOrDefault();

                    if (autoSkill != null)
                    {
                        await character.UseSkill(autoSkill);
                        character.UpdatePartySkillGroupHealExpireTime(SkillTime);
                        return;
                    }
                }
            }
        }

        public static async Task PartyBuffer(this Character character)
        {
            if (DateTime.Now < character.PartySkillBufferExpireTime) return;

            var skillTypes = Client.Party.BufferSkillType.Get();
            var skillCode = skillTypes.Group;
            if (skillCode == "closed") return;

            var undySkill = Client.PartySkills
                .Where(x => !character.UsedSkills.Any(a => a.BaseId == x.BaseId))
                .Where(x => x.BaseId == character.GetSkillId(654))
                .Where(x => x.Point <= character.GetCharacterSkillPointByRow(x.Mastery))
                .Where(x => x.Mana <= character.Mana)
                .OrderByDescending(x => x.Cooldown)
                .FirstOrDefault();

            foreach (var item in Client.PartyCharacters)
            {
                if (DateTime.Now < item.PartyBufferExpireTime || item.Distance > 50 || DateTime.Now < character.PartySkillBufferExpireTime) continue;

                switch (skillCode)
                {
                    default:
                        var defaultSkill = Client.PartySkills
                            .Where(x => !character.UsedSkills.Any(a => a.BaseId == x.BaseId))
                            .Where(x => x.BaseId == character.GetSkillId(Convert.ToInt32(skillCode)))
                            .Where(x => x.Point <= character.GetCharacterSkillPointByRow(x.Mastery))
                            .Where(x => x.Mana <= character.Mana)
                            .OrderByDescending(x => x.Cooldown)
                            .FirstOrDefault();

                        if (defaultSkill == null) continue;

                        await character.UseSkill(defaultSkill, item);
                        item.UpdatePartyBufferExpireTime();

                        character.UpdatePartySkillBufferExpireTime(SkillTime);
                        continue;

                    case "auto":
                        var autoSkillTypes = Client.Party.BufferSkillType.List()
                            .Where(x => x.Order > 0)
                            .OrderByDescending(x => x.Order)
                            .ToArray();

                        foreach (var skillType in autoSkillTypes)
                        {
                            var autoSkill = Client.PartySkills
                                .Where(x => !character.UsedSkills.Any(a => a.BaseId == x.BaseId))
                                .Where(x => x.BaseId == character.GetSkillId(Convert.ToInt32(skillType.Group)))
                                .Where(x => x.Point <= character.GetCharacterSkillPointByRow(x.Mastery))
                                .Where(x => x.Mana <= character.Mana)
                                .OrderByDescending(x => x.Cooldown)
                                .FirstOrDefault();

                            if (autoSkill == null) continue;

                            if (item.GetUndyLimit(skillType.Order))
                                await character.UseSkill(undySkill, item);
                            else
                                await character.UseSkill(autoSkill, item);

                            item.UpdatePartyBufferExpireTime();
                            character.UpdatePartySkillBufferExpireTime(SkillTime);
                            break;
                        }
                        break;
                }
            }
        }

        public static async Task PartyDefence(this Character character)
        {
            if (DateTime.Now < character.PartySkillDefenceExpireTime) return;

            var skillTypes = Client.Party.DefenceSkillType.Get();
            var skillCode = skillTypes.Group;
            if (skillCode == "closed") return;

            foreach (var item in Client.PartyCharacters)
            {
                if (DateTime.Now < item.PartyDefenceExpireTime || item.Distance > 50 || DateTime.Now < character.PartySkillDefenceExpireTime) continue;

                switch (skillCode)
                {
                    default:
                        var defaultSkill = Client.PartySkills
                            .Where(x => !character.UsedSkills.Any(a => a.BaseId == x.BaseId))
                            .Where(x => x.BaseId == character.GetSkillId(Convert.ToInt32(skillCode)))
                            .Where(x => x.Point <= character.GetCharacterSkillPointByRow(x.Mastery))
                            .Where(x => x.Mana <= character.Mana)
                            .OrderByDescending(x => x.Cooldown)
                            .FirstOrDefault();

                        if (defaultSkill == null) continue;

                        await character.UseSkill(defaultSkill, item);
                        item.UpdatePartyDefenceExpireTime();

                        character.UpdatePartySkillDefenceExpireTime(SkillTime);
                        continue;

                    case "auto":
                        var autoSkillTypes = Client.Party.DefenceSkillType.List()
                            .Where(x => x.Order > 0)
                            .OrderByDescending(x => x.Order)
                            .ToArray();

                        foreach (var skillType in autoSkillTypes)
                        {
                            var autoSkill = Client.PartySkills
                                .Where(x => !character.UsedSkills.Any(a => a.BaseId == x.BaseId))
                                .Where(x => x.BaseId == character.GetSkillId(Convert.ToInt32(skillType.Group)))
                                .Where(x => x.Point <= character.GetCharacterSkillPointByRow(x.Mastery))
                                .Where(x => x.Mana <= character.Mana)
                                .OrderByDescending(x => x.Cooldown)
                                .FirstOrDefault();

                            if (autoSkill == null) continue;

                            await character.UseSkill(autoSkill, item);
                            item.UpdatePartyDefenceExpireTime();
                            character.UpdatePartySkillDefenceExpireTime(SkillTime);
                            break;
                        }
                        break;
                }
            }
        }

        public static async Task PartyRestore(this Character character)
        {
            if (DateTime.Now < character.PartySkillRestoreExpireTime) return;

            var skillTypes = Client.Party.RestoreSkillType.Get();
            var skillCode = skillTypes.Group;
            if (skillCode == "closed") return;

            foreach (var item in Client.PartyCharacters)
            {
                if (DateTime.Now < item.PartyRestoreExpireTime || item.Distance > 50 || DateTime.Now < character.PartySkillRestoreExpireTime) continue;

                switch (skillCode)
                {
                    default:
                        var defaultSkill = Client.PartySkills
                            .Where(x => !character.UsedSkills.Any(a => a.BaseId == x.BaseId))
                            .Where(x => x.BaseId == character.GetSkillId(Convert.ToInt32(skillCode)))
                            .Where(x => x.Point <= character.GetCharacterSkillPointByRow(x.Mastery))
                            .Where(x => x.Mana <= character.Mana)
                            .OrderByDescending(x => x.Cooldown)
                            .FirstOrDefault();

                        if (defaultSkill == null) continue;

                        await character.UseSkill(defaultSkill, item);
                        item.UpdatePartyRestoreExpireTime();

                        character.UpdatePartySkillRestoreExpireTime(SkillTime);
                        continue;

                    case "auto":
                        var defenceSkillTypes = Client.Party.RestoreSkillType.List()
                            .Where(x => x.Order > 0)
                            .OrderByDescending(x => x.Order)
                            .ToArray();

                        foreach (var skillType in defenceSkillTypes)
                        {
                            var autoSkill = Client.PartySkills
                                .Where(x => !character.UsedSkills.Any(a => a.BaseId == x.BaseId))
                                .Where(x => x.BaseId == character.GetSkillId(Convert.ToInt32(skillType.Group)))
                                .Where(x => x.Point <= character.GetCharacterSkillPointByRow(x.Mastery))
                                .Where(x => x.Mana <= character.Mana)
                                .OrderByDescending(x => x.Cooldown)
                                .FirstOrDefault();

                            if (autoSkill == null) continue;

                            await character.UseSkill(autoSkill, item);
                            item.UpdatePartyRestoreExpireTime();
                            character.UpdatePartySkillRestoreExpireTime(SkillTime);
                            break;
                        }
                        break;
                }
            }
        }

        public static async Task PartyResistance(this Character character)
        {
            if (DateTime.Now < character.PartySkillResistanceExpireTime) return;

            var skillTypes = Client.Party.ResistanceSkillType.Get();
            var skillCode = skillTypes.Group;
            if (skillCode == "closed") return;

            foreach (var item in Client.PartyCharacters)
            {
                if (DateTime.Now < item.PartyResistanceExpireTime || item.Distance > 50 || DateTime.Now < character.PartySkillResistanceExpireTime) continue;

                switch (skillCode)
                {
                    default:
                        var defaultSkill = Client.PartySkills
                            .Where(x => !character.UsedSkills.Any(a => a.BaseId == x.BaseId))
                            .Where(x => x.BaseId == character.GetSkillId(Convert.ToInt32(skillCode)))
                            .Where(x => x.Point <= character.GetCharacterSkillPointByRow(x.Mastery))
                            .Where(x => x.Mana <= character.Mana)
                            .OrderByDescending(x => x.Cooldown)
                            .FirstOrDefault();

                        if (defaultSkill == null) continue;

                        await character.UseSkill(defaultSkill, item);
                        item.UpdatePartyResistanceExpireTime();

                        character.UpdatePartySkillResistanceExpireTime(SkillTime);
                        continue;

                    case "auto":
                        var defenceSkillTypes = Client.Party.ResistanceSkillType.List()
                            .Where(x => x.Order > 0)
                            .OrderByDescending(x => x.Order)
                            .ToArray();

                        foreach (var skillType in defenceSkillTypes)
                        {
                            var autoSkill = Client.PartySkills
                                .Where(x => !character.UsedSkills.Any(a => a.BaseId == x.BaseId))
                                .Where(x => x.BaseId == character.GetSkillId(Convert.ToInt32(skillType.Group)))
                                .Where(x => x.Point <= character.GetCharacterSkillPointByRow(x.Mastery))
                                .Where(x => x.Mana <= character.Mana)
                                .OrderByDescending(x => x.Cooldown)
                                .FirstOrDefault();

                            if (autoSkill == null) continue;

                            await character.UseSkill(autoSkill, item);
                            item.UpdatePartyResistanceExpireTime();
                            character.UpdatePartySkillResistanceExpireTime(SkillTime);
                            break;
                        }
                        break;
                }
            }
        }

        public static async Task PartyCure(this Character character)
        {
            if (DateTime.Now < character.PartySkillCureExpireTime || (!Client.Party.AutoCurseCure && !Client.Party.AutoDiseaseCure)) return;

            foreach (var item in Client.PartyCharacters)
            {
                if ((!item.NeedCureCurse && !item.NeedCureDisease) || item.Distance > 50 || DateTime.Now < character.PartySkillCureExpireTime) continue;

                if (item.NeedCureCurse && Client.Party.AutoCurseCure)
                    await character.UseCureCurse(item);

                if (item.NeedCureDisease && Client.Party.AutoDiseaseCure)
                    await character.UseCureDisease(item);

                character.UpdatePartySkillCureExpireTime(1600);
                break;
            }
        }
    }
}
