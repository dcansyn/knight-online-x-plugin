using KO.Core.Attributes;
using Newtonsoft.Json;

namespace KO.Domain.Quests.Data
{
    public class QuestRewardData
    {
        [Ignore]
        public int ItemId { get; protected set; }

        [Ignore]
        public int ItemBaseId { get; protected set; }

        [Ignore]
        public int ItemExtensionId { get; protected set; }

        [JsonProperty]
        public int Count { get; protected set; }

        [JsonProperty("Item Name")]
        public string ItemName { get; protected set; }

        public QuestRewardData(int itemId, int count)
        {
            ItemId = itemId;
            Count = count;

            ItemBaseId = ItemId / 1000 * 1000;
            ItemExtensionId = ItemId.ToString().Length > 9 ? ((ItemId % 1000) + 1000) : ItemId % 1000;
        }

        public void UpdateItemName(string itemName)
        {
            ItemName = itemName;
        }
    }
}
