using KO.Application.Characters.Extensions;
using KO.Application.Skills.Extensions;
using KO.Application.Skills.Handlers;
using KO.Application.Targets.Extensions;
using KO.Core.Constants;
using KO.Domain.Characters;
using KO.Domain.Items;
using KO.Domain.Skills;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KO.Application.Characters.Handlers
{
    public static class CharacterActionHandler
    {
        public static async Task FollowAction(this Character character)
        {
            if (!Client.CharacterFollow.IsFollow || (character.IsBoxCollector && Client.BoxLoots.Any()) || DateTime.Now < character.FollowExpireTime) return;

            switch (Client.CharacterFollow.FollowType)
            {
                case FollowType.MainCharacter:
                    if (character.IsMain) return;

                    var mainDistance = character.GetTargetDistance(Client.Main.GetCharacterX(), Client.Main.GetCharacterY());
                    if (mainDistance > 0)
                        await character.Walk(Client.Main.GetCharacterX(), Client.Main.GetCharacterY(), mainDistance > 3 ? CharacterWalkType.Teleport : CharacterWalkType.Jump);
                    break;

                case FollowType.MainTarget:
                    if (Client.Main.Target == null) return;

                    var mainTargetDistance = character.GetTargetDistance(Client.Main.Target.X, Client.Main.Target.Y);
                    if (mainTargetDistance > 0)
                        await character.Walk(Client.Main.Target.X, Client.Main.Target.Y, mainTargetDistance > 3 ? CharacterWalkType.Teleport : CharacterWalkType.Jump);
                    break;
            }

            character.UpdateFollowExpireTime();
        }

        public static async Task CoordinateWalkAction(this Character character)
        {
            if (!Client.CharacterWalk.IsWalk || !Client.CharacterWalk.CoordinateList.Any() || DateTime.Now < character.WalkExpireTime) return;
            if (Client.CharacterWalk.IsWalkCoordinateWhenTargetEmpty && (character.Target != null || DateTime.Now < Client.CharacterTarget.TargetExpireTime)) return;
            if (character.IsBoxCollector && Client.BoxLoots.Any()) return;

            var coordinate = Client.CharacterWalk.GetCurrentCoodinate().Split('-').Select(x => Convert.ToInt32(x)).ToArray();

            if (character.GetTargetDistance(coordinate[0], coordinate[1]) < 2)
            {
                Client.CharacterWalk.NextCoordinate();
                return;
            }

            character.UpdateFollowExpireTime(2000);

            await character.Walk(coordinate[0], coordinate[1], Client.CharacterWalk.CoordinateWalkType);
            character.UpdateWalkExpireTime(Client.CharacterWalk.ChangeCoordinateSeconds * 1000);
        }

        public static async Task ProtectionAction(this Character character)
        {
            var inventoryItemIds = character.InventoryItems.Select(x => x.ItemId);
            var usedSkillIdList = character.UsedSkills.Select(x => x.BaseId);

            #region [When You Die]
            if (Client.CharacterProtection.IsGetUpWhenYouDie && character.GetCharacterHealth() <= 0 && character.DeathTime == null && DateTime.Now > character.GlobalExpireTime)
            {
                if (Client.CharacterProtection.IsGetUpReturnSlot)
                    character.UpdateDeathTime(DateTime.Now);

                await character.UseSuicide();
                character.UpdateGlobalExpireTime();
            }

            if (Client.CharacterProtection.IsGetUpReturnSlot && DateTime.Now > character.AvailableExpireTime && character.DeathTime != null && DateTime.Now > character.GlobalExpireTime)
            {
                await character.Walk(Client.CharacterTarget.TargetRunCenterX, Client.CharacterTarget.TargetRunCenterY, CharacterWalkType.Teleport);
                character.UpdateDeathTime(null);
            }
            #endregion

            #region [Suicide]
            if (Client.CharacterProtection.IsSuicide && DateTime.Now > character.GlobalExpireTime && character.GetHealthLimit(Client.CharacterProtection.SuicidePercent))
            {
                await character.UseSuicide();
                character.UpdateGlobalExpireTime();
            }

            if (Client.CharacterProtection.IsSuicideF4 && DateTime.Now > character.GlobalExpireTime && WinApi.GetAsyncKeyState((int)Keys.F4) > 0)
            {
                await character.UseSuicide();
                character.UpdateGlobalExpireTime();
            }
            #endregion

            if (DateTime.Now < character.AvailableExpireTime) return;

            #region [Eternity]
            if (Client.CharacterProtection.IsEternity &&
                DateTime.Now > character.EternityExpireTime &&
                !usedSkillIdList.Contains(492060) &&
                character.GetHealthLimit(Client.CharacterProtection.EternityPercent))
            {
                await character.UseSkill(new Skill(492060, 9999, 20));
                character.UpdateEternityExpireTime(2100);
            }
            #endregion

            #region [Minor]
            if (Client.CharacterProtection.IsMinor && character.ClassType == CharacterClassType.Rogue)
            {
                if (Client.CharacterProtection.MinorPercent >= 99 && (character.LastHealth > 60 || character.GetHealthLimit(50)))
                    await character.UseSkill(new Skill(character.GetSkillId(705)));

                if (Client.CharacterProtection.MinorPercent < 99 && character.GetHealthLimit(Client.CharacterProtection.MinorPercent))
                    await character.UseSkill(new Skill(character.GetSkillId(705)));
            }
            #endregion

            #region [Potion]
            if (Client.CharacterProtection.IsHealthPotion || Client.CharacterProtection.IsManaPotion)
            {
                var potions = Client.PotionSkills.Where(x => inventoryItemIds.Contains(x.RequiredItem) && !usedSkillIdList.Contains(x.BaseId));

                // Health
                if (Client.CharacterProtection.IsHealthPotion && potions.Any())
                {
                    var healthPotions = potions
                    .Where(x => x.Extension.IsHealthPotion && !usedSkillIdList.Contains(x.BaseId))
                    .OrderBy(x => x.Extension.PotionValue);

                    var totalHealthPotion = character.LastHealth;
                    foreach (var hp in healthPotions)
                    {
                        if (character.UsedSkills.Any(x => x.BaseId == hp.BaseId)) break;

                        if ((Client.CharacterProtection.IsAutomaticPotion && character.LastHealth >= hp.Extension.PotionValue) ||
                            !Client.CharacterProtection.IsAutomaticPotion && character.GetHealthLimit(Client.CharacterProtection.HealthPotionPercent))
                        {
                            if (Client.CharacterProtection.IsMultipleHealthPotion && totalHealthPotion <= 0) break;

                            await character.UseSkill(hp);
                            totalHealthPotion -= hp.Extension.PotionValue;

                            if (!Client.CharacterProtection.IsMultipleHealthPotion)
                                break;
                        }
                    }
                }

                // Mana
                if (Client.CharacterProtection.IsManaPotion && potions.Any())
                {
                    var manaPotions = potions
                    .Where(x => x.Extension.IsManaPotion && !usedSkillIdList.Contains(x.BaseId))
                    .OrderBy(x => x.Extension.PotionValue);

                    var totalManaPotion = character.LastMana;
                    foreach (var mana in manaPotions)
                    {
                        if (character.UsedSkills.Any(x => x.BaseId == mana.BaseId)) break;

                        if ((Client.CharacterProtection.IsAutomaticPotion && character.LastMana >= mana.Extension.PotionValue) ||
                            !Client.CharacterProtection.IsAutomaticPotion && character.GetManaLimit(Client.CharacterProtection.ManaPotionPercent))
                        {
                            if (Client.CharacterProtection.IsMultipleManaPotion && totalManaPotion <= 0) break;

                            await character.UseSkill(mana);
                            totalManaPotion -= mana.Extension.PotionValue;

                            if (!Client.CharacterProtection.IsMultipleManaPotion)
                                break;
                        }
                    }
                }
            }
            #endregion

            #region [Magic Hammer]
            if (Client.CharacterProtection.IsMagicHammer && DateTime.Now > character.MagicHammerExpireTime && character.InventoryItems.Any(x => x.InventoryStatus != ItemInventoryStatusType.Row && x.Durability <= 30) && DateTime.Now > character.GlobalExpireTime)
            {
                var magicHammer = Client.MagicHammerSkills.FirstOrDefault();
                await character.UseSkill(magicHammer);
                character.UpdateMagicHammerExpireTime();
            }
            #endregion

            await Task.CompletedTask;
        }
    }
}
