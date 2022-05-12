using KO.Application.Items.Repositories;
using KO.Core.Extensions;
using KO.Domain.Boxes;
using KO.Domain.Characters;
using KO.Domain.Items;
using System.Linq;
using System.Threading.Tasks;

namespace KO.Application.Boxes.Extensions
{
    public static class BoxExtensions
    {
        public static async Task<bool> IsCollectable(this Character character, int itemId)
        {
            if (!Client.BoxCollect.IsActive) return true;

            if (Client.BoxCollect.MoneyIsActive && itemId == 900000000) return true;

            if (!Client.BoxCollect.IsListInclude && Client.BoxCollect.ItemIdList.Contains(itemId))
                return false;

            if (Client.BoxCollect.IsListInclude && Client.BoxCollect.ItemIdList.Contains(itemId))
                return true;

            if (Client.BoxCollect.CollectTypes.Any() || Client.BoxCollect.DoNotCollectTypes.Any())
            {
                using (var itemRepository = new ItemRepository())
                {
                    var item = await itemRepository.GetByItemId(itemId);

                    if (Client.BoxCollect.DoNotCollectTypes.Any() && CheckCollectItem(Client.BoxCollect.DoNotCollectTypes, item))
                        return false;

                    if (Client.BoxCollect.CollectTypes.Any() && CheckCollectItem(Client.BoxCollect.CollectTypes, item))
                        return true;
                }
            }

            return false;
        }

        public static bool CheckCollectItem(BoxCollectType[] lootTypes, Item item)
        {
            if (lootTypes.Any(x => x == BoxCollectType.TypeUnique) && item.Extension != null && item.Extension.GetExtensionType() == ItemExtType.Unique) return true;
            if (lootTypes.Any(x => x == BoxCollectType.TypeStackable) && item.IsCountable > 0) return true;
            if (lootTypes.Any(x => x == BoxCollectType.TypeScroll) && item.GetItemKindType() == ItemKindType.Scroll) return true;
            if (lootTypes.Any(x => x == BoxCollectType.TypeRare) && item.Extension != null && item.Extension.GetExtensionType() == ItemExtType.Rare) return true;
            if (lootTypes.Any(x => x == BoxCollectType.TypeCraft) && item.Extension != null && item.Extension.GetExtensionType() == ItemExtType.Craft) return true;
            if (lootTypes.Any(x => x == BoxCollectType.TypeQuest) && item.GetItemKindType() == ItemKindType.LuneItem) return true;
            if (lootTypes.Any(x => x == BoxCollectType.TypeHpRecovery) && item.Extension != null && item.Extension.HpRecovery > 0) return true;
            if (lootTypes.Any(x => x == BoxCollectType.TypeMpRecovery) && item.Extension != null && item.Extension.MpRecovery > 0) return true;

            if (lootTypes.Any(x => x == BoxCollectType.ScrollLow) && item.ItemScrollGrade == 1) return true;
            if (lootTypes.Any(x => x == BoxCollectType.ScrollMiddle) && item.ItemScrollGrade == 2) return true;
            if (lootTypes.Any(x => x == BoxCollectType.ScrollHigh) && item.ItemScrollGrade > 2) return true;

            var itemGrade = item.Grade();
            if (lootTypes.Any(x => x == BoxCollectType.Grade1) && itemGrade == 1) return true;
            if (lootTypes.Any(x => x == BoxCollectType.Grade2) && itemGrade == 2) return true;
            if (lootTypes.Any(x => x == BoxCollectType.Grade3) && itemGrade == 3) return true;
            if (lootTypes.Any(x => x == BoxCollectType.Grade4) && itemGrade == 4) return true;
            if (lootTypes.Any(x => x == BoxCollectType.Grade5) && itemGrade == 5) return true;
            if (lootTypes.Any(x => x == BoxCollectType.Grade6) && itemGrade == 6) return true;
            if (lootTypes.Any(x => x == BoxCollectType.Grade7) && itemGrade == 7) return true;
            if (lootTypes.Any(x => x == BoxCollectType.Grade8) && itemGrade == 8) return true;

            var itemKindData = item.GetItemKindType().Get();
            if (lootTypes.Any(x => x == BoxCollectType.ClassRogue) && itemKindData.Group.Contains("Rogue")) return true;
            if (lootTypes.Any(x => x == BoxCollectType.ClassWarrior) && itemKindData.Group.Contains("Warrior")) return true;
            if (lootTypes.Any(x => x == BoxCollectType.ClassPriest) && itemKindData.Group.Contains("Priest")) return true;
            if (lootTypes.Any(x => x == BoxCollectType.ClassMagician) && itemKindData.Group.Contains("Magician")) return true;

            if (lootTypes.Any(x => x == BoxCollectType.WearableWeapon) && itemKindData.Group.Contains("Weapon")) return true;
            if (lootTypes.Any(x => x == BoxCollectType.WearableArmor) && itemKindData.Group.Contains("Armor")) return true;
            if (lootTypes.Any(x => x == BoxCollectType.WearableJewelry) && itemKindData.Group.Contains("Jewelry")) return true;

            return false;
        }
    }
}
