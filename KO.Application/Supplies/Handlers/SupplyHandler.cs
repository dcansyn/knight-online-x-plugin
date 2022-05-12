using KO.Application.Addresses.Handlers;
using KO.Application.Characters.Extensions;
using KO.Application.Items.Extensions;
using KO.Application.Items.Handlers;
using KO.Application.Targets.Extensions;
using KO.Core.Constants;
using KO.Core.Extensions;
using KO.Domain.Characters;
using KO.Domain.Items;
using KO.Domain.Supplies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KO.Application.Supplies.Handlers
{
    public static class SupplyHandler
    {
        public static async Task SupplyItem(this Character character, SupplyItem[] supplyItems, SupplyNpcType npcType = SupplyNpcType.SundriesBuy)
        {
            var npcTypeData = npcType.Get();

            var npc = character.GetTargetByName(npcTypeData.DisplayName);
            if (npc == null || npc.Distance > 3) return;

            character.UpdateItems(character.GetItems());

            var emptyRows = character.InventoryItems.Where(x => x.InventoryStatus == ItemInventoryStatusType.Row && x.ItemId == 0).ToList();
            if (emptyRows.Count() < supplyItems.Length) throw new Exception("Empty row not found.");

            var code = "";
            var count = 0;
            for (int i = 0; i < supplyItems.Length; i++)
            {
                var supplyItem = supplyItems[i];
                var item = character.InventoryItems.FirstOrDefault(x => x.ItemId == supplyItem.ItemId);

                if (item != null && item.Count >= supplyItem.Count && supplyItem.IsComplete) continue;

                var inventoryRow = (item != null && item.Count > 0 ? item.Row : emptyRows[count].Row) - Settings.KO_OFF_INVENTORY_START_ROW;
                var supplyCount = supplyItem.IsComplete && item != null && item.Count > 0 ? supplyItem.Count - item.Count : supplyItem.Count;
                if (supplyCount <= 0) continue;

                if (npcTypeData.Name.Contains("Buy"))
                    code += $"{supplyItem.ItemId.ConvertToDword()}{inventoryRow.ConvertToDword(1)}{supplyCount.ConvertToDword(2)}{supplyItem.Page.ConvertToDword(1)}{supplyItem.Row.ConvertToDword(1)}";
                else
                    code += $"{supplyItem.ItemId.ConvertToDword()}{inventoryRow.ConvertToDword(1)}{supplyCount.ConvertToDword(2)}";

                count++;
            }

            if (count <= 0) return;

            await character.Send("2001", npc.HexId, "FFFFFFFF");
            await character.Send(npcTypeData.Group, npcTypeData.ShortName, npc.HexId, count.ConvertToDword(1), code);
            await character.Send("6A02");
        }

        public static async Task RepairItem(this Character character, int row)
        {
            Thread.Sleep(1000);

            var itemId = character.GetItemId(row);
            if (itemId <= 0) return;

            var npc = character.GetTargetByName(SupplyNpcType.SundriesBuy.Get().DisplayName);
            if (npc == null || npc.Distance > 3) return;

            itemId = itemId.ToString().Length > 9 ? itemId - 1000 : itemId;

            await character.Send("3B01", row.ConvertToDword(1), npc.HexId, itemId.ConvertToDword());
            await character.Send("6A02");
        }

        public static async Task SupplySundires(this Character character)
        {
            if (Client.Supply.RepairWeapon)
                await character.RepairItem((int)ItemInventoryType.LeftWeapon);

            if (Client.Supply.RepairWeapon)
                await character.RepairItem((int)ItemInventoryType.RightWeapon);

            if (Client.Supply.RepairHelmet)
                await character.RepairItem((int)ItemInventoryType.Helmet);

            if (Client.Supply.RepairPauldron)
                await character.RepairItem((int)ItemInventoryType.Pauldron);

            if (Client.Supply.RepairPants)
                await character.RepairItem((int)ItemInventoryType.Pad);

            if (Client.Supply.RepairGauntlet)
                await character.RepairItem((int)ItemInventoryType.Gauntlet);

            if (Client.Supply.RepairBoots)
                await character.RepairItem((int)ItemInventoryType.Boots);

            var supplyItems = new List<SupplyItem>();

            if (Client.Supply.BuyArrow && character.ClassType == CharacterClassType.Rogue)
                supplyItems.Add(new SupplyItem(391010000, Client.Supply.ArrowCount, 0, 0));

            if (Client.Supply.BuyWolf && character.ClassType == CharacterClassType.Rogue && character.IsMain)
                supplyItems.Add(new SupplyItem(370004000, Client.Supply.WolfCount, 0, 7));

            if (Client.Supply.BuyPrayerOfGodsPower && character.ClassType == CharacterClassType.Priest)
                supplyItems.Add(new SupplyItem(389026000, Client.Supply.PrayerOfGodsPowerCount, 0, 11));

            if (Client.Supply.BuyTransformationGem)
                supplyItems.Add(new SupplyItem(379091000, Client.Supply.TransformationGemCount, 1, 3));

            if (supplyItems.Count > 0)
                await character.SupplyItem(supplyItems.ToArray());
        }

        public static async Task SupplyPotion(this Character character)
        {
            var supplyItems = new List<SupplyItem>();

            if (Client.Supply.BuyHealthPotion)
            {
                var healthPotionTypeData = Client.Supply.HealthPotionType.Get();
                supplyItems.Add(new SupplyItem(Convert.ToInt32(healthPotionTypeData.Group), Client.Supply.HealthPotionCount, 0, healthPotionTypeData.Value));
            }

            if (Client.Supply.BuyManaPotion)
            {
                var manaPotionTypeData = Client.Supply.ManaPotionType.Get();
                supplyItems.Add(new SupplyItem(Convert.ToInt32(manaPotionTypeData.Group), Client.Supply.ManaPotionCount, 0, manaPotionTypeData.Value));
            }

            if (supplyItems.Count > 0)
                await character.SupplyItem(supplyItems.ToArray(), character.GetCharacterZoneId() == 75 ? SupplyNpcType.PortionBuy : SupplyNpcType.PotionBuy);
        }

        public static async Task SupplyBank(this Character character)
        {
            var npcHexId = Client.Main.GetTargetHexId();
            if (npcHexId == "FFFF") return;

            character.UpdateItems(character.GetItems());

            await character.CollectBankItems(npcHexId);
            Thread.Sleep(500);
            await character.CollectBankItems(npcHexId);

            if (Client.Supply.BankHealthPotion)
            {
                var healthPotionTypeData = Client.Supply.BankHealthPotionType.Get();
                var healthPotionItemId = Convert.ToInt32(healthPotionTypeData.Group);

                var healthPotionItem = character.InventoryItems.FirstOrDefault(x => x.ItemId == healthPotionItemId);
                var bankHealthPotion = character.BankItems.FirstOrDefault(x => x.ItemId == healthPotionItemId);
                if (bankHealthPotion != null)
                {
                    var healthPotionCount = healthPotionItem != null ? Client.Supply.BankHealthPotionCount - healthPotionItem.Count : Client.Supply.BankHealthPotionCount;
                    var healthPotionRow = healthPotionItem != null ? healthPotionItem.InventoryRow : character.GetFirstEmptyRow();
                    if (healthPotionRow >= 0 && healthPotionCount > 0)
                    {
                        await character.ReceiveBankItem(npcHexId, healthPotionItemId, bankHealthPotion.Page, bankHealthPotion.Row, healthPotionRow, healthPotionCount);
                        Thread.Sleep(500);
                    }
                }
            }

            if (Client.Supply.BankManaPotion)
            {
                var manaPotionTypeData = Client.Supply.BankManaPotionType.Get();
                var manaPotionItemId = Convert.ToInt32(manaPotionTypeData.Group);

                var manaPotionItem = character.InventoryItems.FirstOrDefault(x => x.ItemId == manaPotionItemId);
                var bankManaPotion = character.BankItems.FirstOrDefault(x => x.ItemId == manaPotionItemId);
                if (bankManaPotion != null)
                {
                    var manaPotionCount = manaPotionItem != null ? Client.Supply.BankManaPotionCount - manaPotionItem.Count : Client.Supply.BankManaPotionCount;
                    var manaPotionRow = manaPotionItem != null ? manaPotionItem.InventoryRow : character.GetFirstEmptyRow();
                    if (manaPotionRow >= 0 && manaPotionCount > 0)
                    {
                        await character.ReceiveBankItem(npcHexId, manaPotionItemId, bankManaPotion.Page, bankManaPotion.Row, manaPotionRow, manaPotionCount);
                        Thread.Sleep(500);
                    }
                }
            }

            await Task.CompletedTask;
        }

        public static async Task SupplySendBankItems(this Character character)
        {
            var npc = character.GetTargetByName("Kronil");
            if (npc == null || npc.Distance > 3) return;

            await character.CollectBankItems(npc.HexId);
            Thread.Sleep(500);
            await character.CollectBankItems(npc.HexId);

            if (Client.Supply.BuyHealthPotion)
            {
                var healthPotionTypeData = Client.Supply.HealthPotionType.Get();
                var healthPotionItemId = Convert.ToInt32(healthPotionTypeData.Group);
                var healthPotionItem = character.InventoryItems.FirstOrDefault(x => x.ItemId == healthPotionItemId);
                var bankHealthPotion = character.BankItems.FirstOrDefault(x => x.ItemId == healthPotionItemId);
                var bankHealthPotionPage = bankHealthPotion?.Page ?? character.GetBankFirstEmptyPage();
                var bankHealthPotionRow = bankHealthPotion?.Row ?? character.GetBankFirstEmptyRow();

                if (healthPotionItem?.Count + bankHealthPotion?.Count >= 9999)
                {
                    Client.Supply.UpdateFillToBankWithPotion(false);
                    return;
                }

                if (healthPotionItem != null && bankHealthPotionPage >= 0 && bankHealthPotionRow >= 0)
                {
                    await character.SendBankItem(npc.HexId, healthPotionItem.InventoryRow, bankHealthPotionPage, bankHealthPotionRow);
                    Thread.Sleep(1000);
                }
            }

            if (Client.Supply.BuyManaPotion)
            {
                var manaPotionTypeData = Client.Supply.ManaPotionType.Get();
                var manaPotionItemId = Convert.ToInt32(manaPotionTypeData.Group);
                var manaPotionItem = character.InventoryItems.FirstOrDefault(x => x.ItemId == manaPotionItemId);
                var bankManaPotion = character.BankItems.FirstOrDefault(x => x.ItemId == manaPotionItemId);
                var bankManaPotionPage = bankManaPotion?.Page ?? character.GetBankFirstEmptyPage();
                var bankManaPotionRow = bankManaPotion?.Row ?? character.GetBankFirstEmptyRow();

                if (manaPotionItem?.Count + bankManaPotion?.Count >= 9999)
                {
                    Client.Supply.UpdateFillToBankWithPotion(false);
                    return;
                }

                if (manaPotionItem != null && bankManaPotionPage >= 0 && bankManaPotionRow >= 0)
                {
                    await character.SendBankItem(npc.HexId, manaPotionItem.InventoryRow, bankManaPotionPage, bankManaPotionRow);
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
