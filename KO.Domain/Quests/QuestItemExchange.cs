using KO.Core.Attributes;
using KO.Domain.Quests.Data;
using Newtonsoft.Json;
using SQLite.CodeFirst;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KO.Domain.Quests
{
    [Table("QuestItemExchanges")]
    public class QuestItemExchange
    {
        [Ignore]
        [Autoincrement, Key]
        public int Id { get; protected set; }

        [Index]
        [Ignore]
        public int BaseId { get; protected set; } // 0
        [Ignore]
        public int ExchangeTypeId { get; protected set; } // 1
        [Ignore]
        public string RequestJson { get; protected set; } // 3, 5, 7, 9, 11
        [Ignore]
        public string RewardJson { get; protected set; } // 13, 15, 17, 19, 21, 23, 25

        [JsonProperty("Is Money")]
        public bool HasMoney { get; protected set; }

        [JsonProperty("Is Experience")]
        public bool HasExperience { get; protected set; }

        [NotMapped]
        [JsonProperty]
        public QuestRequestData[] Requests => !string.IsNullOrEmpty(RequestJson) ? JsonConvert.DeserializeObject<QuestRequestData[]>(RequestJson) : null;

        [NotMapped]
        [JsonProperty]
        public QuestRewardData[] Rewards => !string.IsNullOrEmpty(RewardJson) ? JsonConvert.DeserializeObject<QuestRewardData[]>(RewardJson) : null;

        public QuestItemExchange() { }

        public QuestItemExchange(int baseId, int exchangeTypeId)
        {
            BaseId = baseId;
            ExchangeTypeId = exchangeTypeId;
        }

        public void UpdateRequestJson(QuestRequestData[] requests)
        {
            RequestJson = JsonConvert.SerializeObject(requests);
        }

        public void UpdateRewardJson(QuestRewardData[] rewards)
        {
            RewardJson = JsonConvert.SerializeObject(rewards);
        }

        public void UpdateHasMoney(bool hasMoney)
        {
            HasMoney = hasMoney;
        }

        public void UpdateHasExperience(bool hasExperience)
        {
            HasExperience = hasExperience;
        }
    }
}
