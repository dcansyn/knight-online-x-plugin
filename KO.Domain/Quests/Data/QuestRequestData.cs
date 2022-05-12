using KO.Core.Attributes;
using Newtonsoft.Json;

namespace KO.Domain.Quests.Data
{
    public class QuestRequestData
    {
        [Ignore]
        public int ItemId { get; protected set; }

        [Ignore]
        public int ItemBaseId { get; protected set; }

        [Ignore]
        public int ItemExtensionId { get; protected set; }

        [Ignore]
        public int CountOrNpcExchangeId { get; protected set; }

        [JsonProperty("Is Hunt")]
        public bool IsHunt { get; protected set; }

        [JsonProperty("Item Name")]
        public string ItemName { get; protected set; }

        [JsonProperty("NPC Details")]
        public QuestNonPlayerCharacterExchange NonPlayerCharacterExchange { get; protected set; }

        public QuestRequestData(int itemId, int countOrNpcExchangeId)
        {
            ItemId = itemId;
            CountOrNpcExchangeId = countOrNpcExchangeId;

            IsHunt = ItemId == 900005000;
            ItemBaseId = ItemId / 1000 * 1000;
            ItemExtensionId = ItemId.ToString().Length > 9 ? ((ItemId % 1000) + 1000) : ItemId % 1000;
        }

        public void UpdateNonPlayerExchange(QuestNonPlayerCharacterExchange questNonPlayerCharacterExchange)
        {
            NonPlayerCharacterExchange = questNonPlayerCharacterExchange;
        }

        public void UpdateItemName(string itemName)
        {
            ItemName = itemName;
        }
    }
}
