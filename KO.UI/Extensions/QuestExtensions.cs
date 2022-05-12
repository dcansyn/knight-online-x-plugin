using KO.Application;
using KO.Application.Features.Handlers;
using KO.Application.Quests.Handlers;
using KO.Application.Quests.Repositories;
using KO.Core.Extensions;
using KO.Core.Filters;
using KO.Domain.Quests;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KO.UI.Extensions
{
    public static class QuestExtensions
    {
        public static async Task GetAllQuest(this Main form, string search = null)
        {
            using (var questRepository = new QuestRepository())
            {
                form.ChkLstQuestFilter.Enabled = false;
                form.LvQuests.Items.Clear();

                var checkedListFilters = form.ChkLstQuestFilter.Items.Cast<object>()
                    .Where((x, i) => form.ChkLstQuestFilter.GetItemChecked(i))
                    .Select(x => x.ToString())
                    .ToList();

                var questFilter = QuestFilterType.Level.List()
                    .Where(x => checkedListFilters.Contains(x.Name))
                    .Select(x => (QuestFilterType)x.Self)
                    .ToArray();

                var result = await questRepository.GetAll(questFilter, search);

                foreach (var quest in result)
                    form.LvQuests.Items.Add(new ListViewItem(new[]
                        {
                        quest.BaseId.ToString(),
                        quest.RequiredLevel.ToString(),
                        quest.Zone,
                        quest.Guide?.Title ?? "Unknown",
                        quest.NpcDescription?.Title ?? "Unknown"
                    }));

                form.ChkLstQuestFilter.Enabled = true;
                await Task.CompletedTask;
            }
        }

        public static async Task GetQuestDetail(this Main form)
        {
            using (var questRepository = new QuestRepository())
            {
                if (form.LvQuests.FocusedItem == null)
                    return;

                if (!int.TryParse(form.LvQuests.Items[form.LvQuests.FocusedItem.Index].SubItems[0].Text, out int questId))
                    return;

                var quest = await questRepository.GetById(questId);
                if (quest == null) return;

                form.TxtQuestDetailGuideJson.Text = "Data not found.";
                if (quest.Guide != null)
                    form.TxtQuestDetailGuideJson.Text = JsonConvert.SerializeObject(quest.Guide, Formatting.Indented, new JsonSerializeFilter());

                form.TxtQuestDetailNpcJson.Text = "Data not found.";
                if (quest.NpcDescription != null)
                    form.TxtQuestDetailNpcJson.Text = JsonConvert.SerializeObject(quest.NpcDescription, Formatting.Indented, new JsonSerializeFilter());

                form.TxtQuestDetailItemExchangeJson.Text = "Data not found.";
                if (quest.ItemExchanges != null)
                    form.TxtQuestDetailItemExchangeJson.Text = JsonConvert.SerializeObject(quest.ItemExchanges, Formatting.Indented, new JsonSerializeFilter());
            }
        }

        public static async Task TakeQuest(this Main form)
        {
            using (var questRepository = new QuestRepository())
            {
                if (form.LvQuests.FocusedItem == null)
                    return;

                if (!int.TryParse(form.LvQuests.Items[form.LvQuests.FocusedItem.Index].SubItems[0].Text, out int questId))
                    return;

                var quest = await questRepository.GetById(questId);
                if (quest == null) return;

                if (quest?.Guide != null)
                {
                    var allQuests = await questRepository.GetAllByGuideId(quest.GuideId);
                    foreach (var q in allQuests)
                    {
                        foreach (var item in Client.Characters)
                            await item.TakeQuest(q.BaseId);
                    }
                }
                else
                {
                    foreach (var item in Client.Characters)
                        await item.TakeQuest(questId);
                }
            }
        }

        public static async Task RemoveQuest(this Main form)
        {
            using (var questRepository = new QuestRepository())
            {
                if (form.LvQuests.FocusedItem == null)
                    return;

                if (!int.TryParse(form.LvQuests.Items[form.LvQuests.FocusedItem.Index].SubItems[0].Text, out int questId))
                    return;

                var quest = await questRepository.GetById(questId);
                if (quest == null) return;

                if (quest?.Guide != null)
                {
                    var allQuests = await questRepository.GetAllByGuideId(quest.GuideId);
                    foreach (var q in allQuests)
                    {
                        foreach (var item in Client.Characters)
                            await item.RemoveQuest(q.BaseId);
                    }
                }
                else
                {
                    foreach (var item in Client.Characters)
                        await item.RemoveQuest(questId);
                }
            }
        }

        public static async Task RewardQuest(this Main form)
        {
            using (var questRepository = new QuestRepository())
            {
                if (form.LvQuests.FocusedItem == null)
                    return;

                if (!int.TryParse(form.LvQuests.Items[form.LvQuests.FocusedItem.Index].SubItems[0].Text, out int questId))
                    return;

                var quest = await questRepository.GetById(questId);
                if (quest == null) return;

                if (quest?.Guide != null)
                {
                    var allQuests = await questRepository.GetAllByGuideId(quest.GuideId);
                    foreach (var q in allQuests)
                    {
                        foreach (var item in Client.Characters)
                            await item.RewardQuest(q.BaseId);
                    }
                }
                else
                {
                    foreach (var item in Client.Characters)
                        await item.RewardQuest(quest.BaseId);
                }
            }
        }
    }
}
