using KO.Application.Items.Extensions;
using KO.Core.Extensions;
using KO.Domain.Characters;
using KO.Domain.Items;
using System;
using System.Linq;

namespace KO.Application.Supplies.Extensions
{
    public static class SupplyExtensions
    {
        public static bool SupplyIsStart(this Character character)
        {
            character.UpdateItems(character.GetItems());

            if (Client.Supply.IsActiveWhenItemConsume)
            {
                if (Client.Supply.BuyHealthPotion)
                {
                    var healthPotion = character.InventoryItems.FirstOrDefault(x => x.ItemId == Convert.ToInt32(Client.Supply.HealthPotionType.Get().Group));
                    if (healthPotion == null || healthPotion.Count <= 2) return true;
                }

                if (Client.Supply.BuyManaPotion)
                {
                    var manaPotion = character.InventoryItems.FirstOrDefault(x => x.ItemId == Convert.ToInt32(Client.Supply.ManaPotionType.Get().Group));
                    if (manaPotion == null || manaPotion.Count <= 2) return true;
                }

                if (Client.Supply.BuyWolf && character.ClassType == CharacterClassType.Rogue && character.IsMain)
                {
                    var wolf = character.InventoryItems.FirstOrDefault(x => x.ItemId == 370004000);
                    if (wolf == null || wolf.Count <= 2) return true;
                }

                if (Client.Supply.BuyArrow && character.ClassType == CharacterClassType.Rogue)
                {
                    var arrow = character.InventoryItems.FirstOrDefault(x => x.ItemId == 391010000);
                    if (arrow == null || arrow.Count <= 50) return true;
                }

                if (Client.Supply.BuyTransformationGem)
                {
                    var gem = character.InventoryItems.FirstOrDefault(x => x.ItemId == 379091000);
                    if (gem == null || gem.Count <= 2) return true;
                }

                if (Client.Supply.BuyPrayerOfGodsPower && character.ClassType == CharacterClassType.Priest)
                {
                    var prayer = character.InventoryItems.FirstOrDefault(x => x.ItemId == 389026000);
                    if (prayer == null || prayer?.Count <= 2) return true;
                }
            }

            if (Client.Supply.IsActiveWhenInventoryFull && character.InventoryItems.Count(x => x.InventoryStatus == ItemInventoryStatusType.Row && x.ItemId == 0) <= 1) return true;

            var leftWeapon = character.InventoryItems.FirstOrDefault(x => x.InventoryStatus == ItemInventoryStatusType.LeftWeapon);
            if (leftWeapon != null && leftWeapon.ItemId > 0 && leftWeapon.Durability <= 250) return true;

            var rightWeapon = character.InventoryItems.FirstOrDefault(x => x.InventoryStatus == ItemInventoryStatusType.RightWeapon);
            if (rightWeapon != null && rightWeapon.ItemId > 0 && rightWeapon.Durability <= 250) return true;

            return false;
        }
    }
}
