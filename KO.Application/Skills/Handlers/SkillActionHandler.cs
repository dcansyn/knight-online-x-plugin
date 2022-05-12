using KO.Application.Characters.Extensions;
using KO.Application.Skills.Extensions;
using KO.Domain.Characters;
using KO.Domain.Skills;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KO.Application.Skills.Handlers
{
    public static class SkillActionHandler
    {
        public static async Task PersonalSkillAction(this Character character)
        {
            if (DateTime.Now < character.PersonalSkillExpireTime) return;

            var usedSkillIdList = character.UsedSkills.Select(x => x.BaseId).ToList();

            var skill = Client.Skills
                .Where(x => !usedSkillIdList.Contains(x.BaseId))
                .Where(x => x.Mastery == 0 || x.Point <= character.GetCharacterSkillPointByRow(x.Mastery))
                .Where(x => x.RequiredItem == 0 || character.InventoryItems.Any(i => i.ItemId == x.RequiredItem))
                .Where(x => x.ClassId == character.ClassId)
                .Where(x => x.Mana <= character.Mana)
                .Where(x => x.Type == SkillType.Personal)
                .Where(x => !character.GetSkillExists(x.BaseId))
                .OrderByDescending(x => x.Cooldown)
                .FirstOrDefault();

            if (skill == null) return;

            if (skill.Name.Contains("Strength of wolf"))
                skill.UpdateCooldown(3000);

            await character.UseSkill(skill, character.Target);
            character.UpdatePersonalSkillExpireTime();
        }

        public static async Task ScrollSkillAction(this Character character)
        {
            var inventoryItemIds = character.InventoryItems.Select(x => x.ItemId).ToArray();

            #region [Scrolls]
            if (Client.CharacterScroll.IsAkaraBlessing && !character.GetBufferExists(BuffSkillType.AkaraBlessing) && DateTime.Now > character.BufferScollExpireTime)
            {
                await character.UseBuff(BuffSkillType.AkaraBlessing, inventoryItemIds);
                character.UpdateBufferScrollExpireTime();
            }

            if (Client.CharacterScroll.IsSeaCucumberPow && !character.GetBufferExists(BuffSkillType.SeaCucumperPow) && DateTime.Now > character.BufferScollExpireTime)
            {
                await character.UseBuff(BuffSkillType.SeaCucumperPow, inventoryItemIds);
                character.UpdateBufferScrollExpireTime();
            }

            if (Client.CharacterScroll.IsSpiritOfKaufmann && !character.GetBufferExists(BuffSkillType.SpiritOfKaufman) && DateTime.Now > character.BufferScollExpireTime)
            {
                await character.UseBuff(BuffSkillType.SpiritOfKaufman, inventoryItemIds);
                character.UpdateBufferScrollExpireTime();
            }

            if (Client.CharacterScroll.IsStatIncrease && !character.GetBufferExists(BuffSkillType.BlessOfTemplateStat) && DateTime.Now > character.BufferScollExpireTime)
            {
                await character.UseBuff(BuffSkillType.BlessOfTemplateStat, inventoryItemIds);
                character.UpdateBufferScrollExpireTime();
            }

            if (Client.CharacterScroll.IsDefenseIncrease && !character.GetBufferExists(BuffSkillType.BlessOfTemplateDefence) && DateTime.Now > character.BufferScollExpireTime)
            {
                await character.UseBuff(BuffSkillType.BlessOfTemplateDefence, inventoryItemIds);
                character.UpdateBufferScrollExpireTime();
            }

            if (Client.CharacterScroll.IsWeaponEnchant && !character.GetBufferExists(BuffSkillType.WeaponEnchant) && DateTime.Now > character.BufferScollExpireTime)
            {
                await character.UseBuff(BuffSkillType.WeaponEnchant, inventoryItemIds);
                character.UpdateBufferScrollExpireTime();
            }

            if (Client.CharacterScroll.IsArmorEnchant && !character.GetBufferExists(BuffSkillType.ArmorEnchant) && DateTime.Now > character.BufferScollExpireTime)
            {
                await character.UseBuff(BuffSkillType.ArmorEnchant, inventoryItemIds);
                character.UpdateBufferScrollExpireTime();
            }
            #endregion

            #region [Transformation Scroll]
            if (Client.CharacterScroll.IsTransformationScroll &&
                DateTime.Now > character.TransformationScrollExpireTime &&
                !character.GetTransformationScrollExists() &&
                !character.GetBufferExists(BuffSkillType.SpiritOfKaufman) &&
                inventoryItemIds.Contains(379091000))
            {
                await character.UseTransformationScroll(Client.CharacterScroll.TransformationType);
                character.UpdateTransformationScrollExpireTime();
            }
            #endregion
        }
    }
}
