using KO.Application.Addresses.Handlers;
using KO.Application.Quests.Extensions;
using KO.Application.Quests.Repositories;
using KO.Application.Targets.Extensions;
using KO.Core.Extensions;
using KO.Domain.Characters;
using System.Linq;
using System.Threading.Tasks;

namespace KO.Application.Quests.Handlers
{
    public static class QuestHandler
    {
        public static async Task TakeQuest(this Character character, int questId)
        {
            await character.Send("6406", questId.ConvertToDword());
        }

        public static async Task RemoveQuest(this Character character, int questId)
        {
            await character.Send("6405", questId.ConvertToDword());
        }

        public static async Task RewardQuest(this Character character, int questId, string targetHexId = null, bool check = true)
        {
            using (var questRepository = new QuestRepository())
            {
                var quest = await questRepository.GetById(questId);
                if (quest == null || (check && (quest.GetRaceType() != character.RaceType || quest.GetClassType() != character.ClassType))) return;

                await character.Send("2001", targetHexId ?? Client.Main.GetTargetHexId(), "FFFFFFFF");
                await character.Send("6407", quest.BaseId.ConvertToDword());

                if (quest.ItemExchanges == null) return;
                foreach (var item in quest.ItemExchanges)
                {
                    if (item.ExchangeTypeId == 11 || item.ExchangeTypeId == 12)
                    {
                        var itemNumber = await character.GetQuestAvailableItem(item.Rewards);
                        if (itemNumber < 0) continue;

                        await character.Send("5500", quest.LuaName.Length.ConvertToDword(1), quest.LuaName.ConvertStringToHex(), itemNumber.ConvertToDword(1));
                        return;
                    }
                }

                if (!quest.ItemExchanges.Any(x => x.ExchangeTypeId == 11 || x.ExchangeTypeId == 12))
                    await character.Send("5500", quest.LuaName.Length.ConvertToDword(1), quest.LuaName.ConvertStringToHex(), "FF");
            }
        }
    }
}
