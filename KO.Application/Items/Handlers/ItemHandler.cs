using KO.Application.Addresses.Extensions;
using KO.Application.Addresses.Handlers;
using KO.Application.Items.Extensions;
using KO.Core.Constants;
using KO.Core.Extensions;
using KO.Domain.Characters;
using System.Threading.Tasks;

namespace KO.Application.Items.Handlers
{
    public static class ItemHandler
    {
        public static async Task DestroyItem(this Character character, int row)
        {
            var itemBaseId = character.ReadLong(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_ITEM_BASE) + Settings.KO_OFF_ITEM_ROW_BASE + (4 * row));
            if (itemBaseId <= 0) return;

            var deleteRow = row - Settings.KO_OFF_INVENTORY_START_ROW;
            await character.WriteByte(Settings.KO_PTR_DESTROY_1, 1);
            await character.WriteLong(Settings.KO_PTR_DESTROY_1 + 0x4, deleteRow);
            await character.WriteLong(Settings.KO_PTR_DESTROY_1 + 0x8, itemBaseId);
            await character.WriteByte(Settings.KO_PTR_DESTROY_2, 1);
            await character.WriteLong(Settings.KO_PTR_DESTROY_2 + 0x8, itemBaseId);
            await character.WriteLong(Settings.KO_PTR_DESTROY_2 + 0xC, 1);
            await character.WriteLong(Settings.KO_PTR_DESTROY_2 + 0x10, deleteRow);
            await character.WriteLong(Settings.KO_PTR_DESTROY_2 + 0x14, 1);

            await character.ExecuteCode(character.RemoteAddress, "60B9", itemBaseId.ConvertToDword(), "BF", Settings.KO_PTR_DESTROY_FNC.ConvertToDword(), "FFD761C3");
            await character.Send("6A02");
        }

        public static async Task FakeOreads(this Character character, bool isActive)
        {
            if (isActive)
            {
                await character.FakeItemUse(700039000, 1);
                return;
            }

            await character.FakeItemDrop(700039000);
            await character.WriteLong(character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + 88) + 1476, 0);
        }

        public static async Task FakeItemUse(this Character character, int itemId, int row)
        {
            await character.ExecuteCode("608B0D", Settings.KO_PTR_CHR.ConvertToDword(), "6A", row.ConvertToDword(1), "68", itemId.ConvertToDword(), "B8", Settings.KO_PTR_FAKE_ITEM.ConvertToDword(), "FFD061C3");
        }

        public static async Task FakeItemDrop(this Character character, int itemId)
        {
            await character.ExecuteCode("608B0D", Settings.KO_PTR_CHR.ConvertToDword(), "6A", 0.ConvertToDword(1), "68", itemId.ConvertToDword(), "B8", Settings.KO_PTR_FAKE_ITEM.ConvertToDword(), "FFD061C3");
        }

        public static async Task SendBankItem(this Character character, string npcHexId, int inventoryRow, int bankPage, int bankRow)
        {
            var itemId = character.GetItemId(Settings.KO_OFF_INVENTORY_START_ROW + inventoryRow);
            var itemCount = character.GetItemCount(Settings.KO_OFF_INVENTORY_START_ROW + inventoryRow);

            await character.Send("4502",
                npcHexId,
                itemId.ConvertToDword(),
                bankPage.ConvertToDword(1),
                inventoryRow.ConvertToDword(1),
                bankRow.ConvertToDword(1),
                itemCount.ConvertToDword(2),
                "0000");

            await character.Send("6A02");
        }

        public static async Task ReceiveBankItem(this Character character, string npcHexId, int itemId, int bankPage, int bankRow, int inventoryRow, int itemCount)
        {
            await character.Send("4503",
                npcHexId,
                itemId.ConvertToDword(),
                bankPage.ConvertToDword(1),
                bankRow.ConvertToDword(1),
                inventoryRow.ConvertToDword(1),
                itemCount.ConvertToDword(2),
                "0000");

            await character.Send("6A02");
        }
    }
}
