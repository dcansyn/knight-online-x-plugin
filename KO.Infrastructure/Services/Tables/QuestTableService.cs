using KO.Domain.Characters;
using KO.Domain.Quests;
using KO.Domain.Quests.Data;
using KO.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KO.Infrastructure.Services.Tables
{
    public class QuestTableService
    {
        public string GamePath { get; set; }
        private readonly TableService _tableService;

        public QuestTableService(string gamePath)
        {
            GamePath = gamePath;
            _tableService = new TableService();
        }

        public async Task<Quest[]> GetQuests()
        {
            using (var db = new BaseDbContext())
            {
                var result = db.Quests.ToArray();

                return await Task.FromResult(result);
            }
        }

        public async Task SaveQuests()
        {
            using (var db = new BaseDbContext())
            {
                var data = _tableService.GetList(_tableService.GetTable(Path.Combine(GamePath, "Quest_Helper.tbl")));
                var model = data.Select(x => new Quest(
                    Convert.ToInt32(x[0]),       // BaseId
                    Convert.ToInt32(x[1]),       // UnknownNpcTypeId
                    Convert.ToInt32(x[2]),       // RequiredLevel
                    Convert.ToInt32(x[3]),       // RequiredExperience
                    Convert.ToInt32(x[6]),       // RaceBaseId
                    Convert.ToInt32(x[7]),       // QuestTypeId
                    Convert.ToInt32(x[8]),       // ZoneId
                    Convert.ToInt32(x[9]),       // NpcId
                    Convert.ToInt32(x[10]),      // MonsterExchangeId
                    Convert.ToInt32(x[11]),      // ClassBaseId
                    Convert.ToInt32(x[12]),      // QuestTalk1Id
                    Convert.ToInt32(x[14]),      // ExchangeId
                    Convert.ToInt32(x[15]),      // QuestTalk2Id
                    x[16],                       // LuaName
                    Convert.ToInt32(x[17]),      // GuideId
                    Convert.ToInt32(x[18]),      // NpcRowId
                    Convert.ToInt32(x[19])       // IsQuestBaseId
                    ))
                    .OrderBy(x => x.BaseId)
                    .ToArray();

                db.Quests.AddRange(model);

                await db.SaveChangesAsync();
            }
        }

        public async Task SaveQuestGuides()
        {
            using (var db = new BaseDbContext())
            {
                var data = _tableService.GetList(_tableService.GetTable(Path.Combine(GamePath, "Quest_Guide_jp.tbl")));
                var model = data.Select(x => new QuestGuide(
                    Convert.ToInt32(x[0]),          // BaseId
                    Convert.ToInt32(x[1]),          // MinLevel
                    Convert.ToInt32(x[2]),          // MaxLevel
                    x[3],                           // Title
                    x[4],                           // SubTitle
                    x[5]                            // Description
                    ))
                    .OrderBy(x => x.BaseId)
                    .ToArray();

                db.QuestGuides.AddRange(model);

                await db.SaveChangesAsync();
            }
        }

        public async Task SaveQuestNonPlayerCharacterDescriptions()
        {
            using (var db = new BaseDbContext())
            {
                var data = _tableService.GetList(_tableService.GetTable(Path.Combine(GamePath, "Quest_Npc_Desc_jp.tbl")));
                var model = data.Select(x => new QuestNonPlayerCharacterDescription(
                        Convert.ToInt32(x[0]),       // Id
                        Convert.ToInt32(x[1]),       // NpcId
                        Convert.ToInt32(x[3]),       // RaceId
                        Convert.ToInt32(x[4]),       // ZoneId
                        Convert.ToInt32(x[5]),       // NpcTypeId
                        x[6],                        // Title
                        x[7],                        // TitleExplanation
                        x[9],                        // LocationDescription
                        Convert.ToInt32(x[10]),      // X1
                        Convert.ToInt32(x[11]),      // Y1
                        Convert.ToInt32(x[12]),      // X2
                        Convert.ToInt32(x[13])       // Y2
                    ))
                    .OrderBy(x => x.BaseId)
                    .ToArray();

                db.QuestNonPlayerCharacterDescriptions.AddRange(model);

                await db.SaveChangesAsync();
            }
        }

        public async Task SaveQuestNonPlayerCharacterExchanges()
        {
            using (var db = new BaseDbContext())
            {
                var nonPlayerCharacters = db.NonPlayerCharacters.AsNoTracking().ToArray();
                var questNpcDescriptions = db.QuestNonPlayerCharacterDescriptions.AsNoTracking().ToArray();

                var data = _tableService.GetList(_tableService.GetTable(Path.Combine(GamePath, "Quest_Monster_Exchange.tbl")));

                foreach (var item in data)
                {
                    var nonPlayerCharacterExchange = new QuestNonPlayerCharacterExchange(Convert.ToInt32(item[0]));
                    var nonPlayerCharacterList = new List<NonPlayerCharacter>();
                    for (int i = 1; i < 5; i++)
                    {
                        var count = Convert.ToInt32(item[i * 5]);
                        for (int j = 1; j < 5 && count > 0; j++)
                        {
                            var nonPlayerCharaterId = Convert.ToInt32(item[(i - 1) * 5 + j]);
                            if (nonPlayerCharaterId > 0)
                            {
                                var huntNonPlayerCharater = nonPlayerCharacters.SingleOrDefault(x => x.BaseId == nonPlayerCharaterId);

                                var questNpcDescription = questNpcDescriptions.FirstOrDefault(x => x.NpcId == nonPlayerCharaterId);
                                if (questNpcDescription != null)
                                {
                                    if (huntNonPlayerCharater == null)
                                        huntNonPlayerCharater = new NonPlayerCharacter(nonPlayerCharaterId, questNpcDescription.Title, false);
                                }

                                if (huntNonPlayerCharater != null)
                                {
                                    huntNonPlayerCharater.UpdateNonPlayerCharacterDescription(questNpcDescription);
                                    nonPlayerCharacterList.Add(huntNonPlayerCharater);
                                }
                            }
                        }
                    }
                    nonPlayerCharacterExchange.UpdateNonPlayerCharacter(nonPlayerCharacterList.Distinct().ToArray());

                    db.QuestNonPlayerCharacterExchanges.Add(nonPlayerCharacterExchange);
                }

                await db.SaveChangesAsync();
            }
        }

        public async Task SaveQuestItemExchanges()
        {
            using (var db = new BaseDbContext())
            {
                var nonPlayerCharacterExchanges = db.QuestNonPlayerCharacterExchanges.AsNoTracking().ToArray();
                var items = db.Items.AsNoTracking().ToArray();
                var itemExtensions = db.ItemExtensions.AsNoTracking().ToArray();

                var dataExchanges = _tableService.GetList(_tableService.GetTable(Path.Combine(GamePath, "Item_Exchange.tbl")));
                var dataExchangeExps = _tableService.GetList(_tableService.GetTable(Path.Combine(GamePath, "Item_Exchange_exp.tbl")));
                var result = new List<QuestItemExchange>();

                foreach (var item in dataExchanges)
                {
                    var itemExchange = new QuestItemExchange(Convert.ToInt32(item[0]), Convert.ToInt32(item[1]));
                    var requests = new List<QuestRequestData>();
                    var rewards = new List<QuestRewardData>();

                    for (int i = 0; i <= 4 && Convert.ToInt32(item[i + 3]) > 100000000; i += 2)
                    {
                        var request = new QuestRequestData(Convert.ToInt32(item[i + 3]), Convert.ToInt32(item[i + 4]));

                        if (request.ItemId == 900005000)
                        {
                            var nonPlayerCharacterExchange = nonPlayerCharacterExchanges.SingleOrDefault(x => x.BaseId == request.CountOrNpcExchangeId);
                            if (nonPlayerCharacterExchange != null)
                                request.UpdateNonPlayerExchange(nonPlayerCharacterExchange);
                        }
                        else
                        {
                            var requestItem = items.SingleOrDefault(x => x.BaseId == request.ItemBaseId);
                            if (requestItem != null)
                            {
                                var requestItemExtension = itemExtensions.SingleOrDefault(x => x.Number == requestItem.ExtensionNumber && x.BaseId == request.ItemExtensionId);
                                if (requestItemExtension != null)
                                    requestItem.UpdateExtension(requestItemExtension);

                                request.UpdateItemName(requestItem.GetTitleWithGrade());
                            }
                        }

                        requests.Add(request);
                    }

                    for (int i = 0; i <= 12 && Convert.ToInt32(item[i + 13]) > 100000000; i += 2)
                    {
                        var reward = new QuestRewardData(Convert.ToInt32(item[i + 13]), Convert.ToInt32(item[i + 14]));

                        var rewardItem = items.SingleOrDefault(x => x.BaseId == reward.ItemBaseId);
                        if (rewardItem != null)
                        {
                            var requestItemExtension = itemExtensions.SingleOrDefault(x => x.Number == rewardItem.ExtensionNumber && x.BaseId == reward.ItemExtensionId);
                            if (requestItemExtension != null)
                                rewardItem.UpdateExtension(requestItemExtension);

                            reward.UpdateItemName(rewardItem.GetTitleWithGrade());
                        }

                        rewards.Add(reward);
                    }

                    itemExchange.UpdateHasMoney(rewards.Any(x => x.ItemId == 900000000));
                    itemExchange.UpdateHasExperience(rewards.Any(x => x.ItemId == 900001000));

                    itemExchange.UpdateRequestJson(requests.ToArray());
                    itemExchange.UpdateRewardJson(rewards.ToArray());

                    result.Add(itemExchange);
                }

                foreach (var item in dataExchangeExps)
                {
                    var itemExchangeExp = new QuestItemExchange(Convert.ToInt32(item[0]), Convert.ToInt32(item[1]));
                    var rewards = new List<QuestRewardData>();

                    for (int i = 0; i < 10 && Convert.ToInt32(item[i + 2]) > 100000000; i += 2)
                    {
                        var reward = new QuestRewardData(Convert.ToInt32(item[i + 2]), Convert.ToInt32(item[i + 3]));

                        var requestItem = items.SingleOrDefault(x => x.BaseId == reward.ItemBaseId);
                        if (requestItem != null)
                        {
                            var requestItemExtension = itemExtensions.SingleOrDefault(x => x.Number == requestItem.ExtensionNumber && x.BaseId == reward.ItemExtensionId);
                            if (requestItemExtension != null)
                                requestItem.UpdateExtension(requestItemExtension);

                            reward.UpdateItemName(requestItem.GetTitleWithGrade());
                        }

                        rewards.Add(reward);
                    }

                    if (rewards.Count > 0)
                        itemExchangeExp.UpdateRewardJson(rewards.Distinct().ToArray());

                    itemExchangeExp.UpdateHasMoney(rewards.Any(x => x.ItemId == 900000000));
                    itemExchangeExp.UpdateHasExperience(rewards.Any(x => x.ItemId == 900001000));

                    result.Add(itemExchangeExp);
                }

                db.QuestItemExchanges.AddRange(result);

                await db.SaveChangesAsync();
            }
        }
    }
}
