using KO.Application.Addresses.Extensions;
using KO.Application.Addresses.Handlers;
using KO.Application.Items.Repositories;
using KO.Core.Constants;
using KO.Core.Extensions;
using KO.Domain.Characters;
using KO.Domain.Items;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KO.Application.Items.Extensions
{
    public static class ItemExtensions
    {
        public static async Task<Item[]> GetDataItems(this Character character)
        {
            using (var itemRepository = new ItemRepository())
            {
                var lst = new List<Item>();

                for (int i = 0; i < Settings.KO_OFF_INVENTORY_COUNT; i++)
                {
                    var itemId = character.GetItemId(i);
                    if (itemId == 0) continue;

                    var inventoryStatus = i >= Settings.KO_OFF_INVENTORY_START_ROW ? ItemInventoryStatusType.Row : (ItemInventoryStatusType)i;
                    var item = await itemRepository.GetByItemId(itemId);
                    if (item == null) continue;

                    var count = character.GetItemCount(i);
                    var durability = character.GetItemDurability(i);

                    item.UpdateInventoryItem(itemId, count, durability, i, inventoryStatus);
                    lst.Add(item);
                }

                return lst.ToArray();
            }
        }

        public static async Task<Item> GetDataItemByType(this Character character, ItemInventoryType inventoryType)
        {
            using (var itemRepository = new ItemRepository())
                return await itemRepository.GetByItemId(character.GetItemId((int)inventoryType));
        }

        public static async Task<Item> GetDataItemById(this Character character, int itemId)
        {
            using (var itemRepository = new ItemRepository())
                return await itemRepository.GetByItemId(itemId);
        }

        public static Item[] GetItems(this Character character)
        {
            var lst = new List<Item>();

            for (int i = 0; i < Settings.KO_OFF_INVENTORY_COUNT; i++)
            {
                var inventoryStatus = i >= Settings.KO_OFF_INVENTORY_START_ROW ? ItemInventoryStatusType.Row : (ItemInventoryStatusType)i;
                var inventoryType = (ItemInventoryType)i;
                lst.Add(new Item(character.GetItemId(i), i, character.GetItemCount(i), character.GetItemDurability(i), inventoryType, inventoryStatus));
            }

            return lst.ToArray();
        }

        public static int GetItemBaseId(this Character character, int row)
        {
            return character.ReadLong(character.ReadLong(character.ReadLong(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_ITEM_BASE) + (Settings.KO_OFF_ITEM_ROW_BASE + (4 * row))) + Settings.KO_OFF_ITEM_ID));
        }

        public static int GetItemId(this Character character, int row)
        {
            return character.ReadLong(character.ReadLong(character.ReadLong(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_ITEM_BASE) + (Settings.KO_OFF_ITEM_ROW_BASE + (4 * row))) + Settings.KO_OFF_ITEM_ID)) + character.ReadLong(character.ReadLong(character.ReadLong(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_ITEM_BASE) + (Settings.KO_OFF_ITEM_ROW_BASE + (4 * row))) + Settings.KO_OFF_ITEM_EXTENSION));
        }

        public static int GetItemExTableId(this Character character, int row)
        {
            return character.ReadLong(character.ReadLong(character.ReadLong(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_ITEM_BASE) + (Settings.KO_OFF_ITEM_ROW_BASE + (4 * row))) + Settings.KO_OFF_ITEM_EXTENSION));
        }

        public static int GetItemCount(this Character character, int row)
        {
            return character.ReadLong(character.ReadLong(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_ITEM_BASE) + (Settings.KO_OFF_ITEM_ROW_BASE + (4 * row))) + Settings.KO_OFF_ITEM_COUNT);
        }

        public static int GetItemDurability(this Character character, int row)
        {
            return character.ReadLong(character.ReadLong(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_ITEM_BASE) + (Settings.KO_OFF_ITEM_ROW_BASE + (4 * row))) + Settings.KO_OFF_ITEM_DURABILITY);
        }

        public static int GetFirstEmptyRow(this Character character)
        {
            return character.InventoryItems.FirstOrDefault(x => x.ItemId == 0)?.Row ?? -1;
        }

        public static int GetEmptyRowCount(this Character character)
        {
            return character.InventoryItems.Count(x => x.ItemId == 0);
        }

        public static bool GetBankIsActive(this Character character)
        {
            return character.ReadByte(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_BANK_BASE) + Settings.KO_OFF_BANK_OPEN) == 1;
        }

        public static async Task CollectBankItems(this Character character, string bankHexId)
        {
            await character.Send("2001", bankHexId, "FFFFFFFF");
            await character.Send("4501", bankHexId);
            await character.Send("2001", bankHexId, "FFFFFFFF");

            using (var itemRepository = new ItemRepository())
            {
                var lst = new List<Item>();
                var page = 0;
                for (int i = 0; i < Settings.KO_OFF_BANK_ROW_COUNT; i++)
                {
                    if (i % Settings.KO_OFF_BANK_PAGE_COUNT == 0 && i != 0)
                        page++;

                    var itemId = character.GetBankItemId(i);
                    var item = await itemRepository.GetByItemId(itemId);
                    if (itemId == 0)
                    {
                        lst.Add(new Item(page, i));
                        continue;
                    }

                    var count = character.GetBankItemCount(i);
                    var durability = character.GetBankItemDurability(i);

                    item.UpdateBankItem(itemId, count, durability, page, i);
                    lst.Add(item);
                }

                character.UpdateBankItems(lst.ToArray());
            }
        }

        public static int GetBankItemBaseId(this Character character, int row)
        {
            return character.ReadLong(character.ReadLong(character.ReadLong(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_BANK_BASE) + Settings.KO_OFF_BANK_ROW_BASE + (4 * row)) + Settings.KO_OFF_ITEM_ID));
        }

        public static int GetBankItemId(this Character character, int row)
        {
            return character.ReadLong(character.ReadLong(character.ReadLong(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_BANK_BASE) + (Settings.KO_OFF_BANK_ROW_BASE + (4 * row))) + Settings.KO_OFF_ITEM_ID)) + character.ReadLong(character.ReadLong(character.ReadLong(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_BANK_BASE) + (Settings.KO_OFF_BANK_ROW_BASE + (4 * row))) + Settings.KO_OFF_ITEM_EXTENSION));
        }

        public static int GetBankItemCount(this Character character, int row)
        {
            return character.ReadLong(character.ReadLong(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_BANK_BASE) + Settings.KO_OFF_BANK_ROW_BASE + (4 * row)) + Settings.KO_OFF_ITEM_COUNT);
        }

        public static int GetBankItemDurability(this Character character, int row)
        {
            return character.ReadLong(character.ReadLong(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_BANK_BASE) + (Settings.KO_OFF_BANK_ROW_BASE + (4 * row))) + Settings.KO_OFF_ITEM_DURABILITY);
        }

        public static int GetBankFirstEmptyRow(this Character character)
        {
            return character.BankItems.FirstOrDefault(x => x.ItemId == 0)?.Row ?? -1;
        }

        public static int GetBankFirstEmptyPage(this Character character)
        {
            return character.BankItems.FirstOrDefault(x => x.ItemId == 0)?.Page ?? -1;
        }

        public static int GetBankEmptyRowCount(this Character character)
        {
            return character.BankItems.Count(x => x.ItemId == 0);
        }
    }
}
