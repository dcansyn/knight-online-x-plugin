using KO.Application.Characters.Extensions;
using KO.Core.Models.Query;
using KO.Domain.Characters;
using KO.Domain.Quests;
using KO.Infrastructure.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace KO.Application.Quests.Repositories
{
    public class QuestRepository : BaseRepository<Quest>
    {
        private readonly QuestGuideRepository _questGuideRepository;
        private readonly QuestItemExchangeRepository _questItemExchangeRepository;
        private readonly QuestNonPlayerCharacterDescriptionRepository _questNonPlayerCharacterDescriptionRepository;

        public QuestRepository()
        {
            _questGuideRepository = new QuestGuideRepository();
            _questItemExchangeRepository = new QuestItemExchangeRepository();
            _questNonPlayerCharacterDescriptionRepository = new QuestNonPlayerCharacterDescriptionRepository();
        }

        public async Task<Quest> GetById(int id)
        {
            var result = await Get(new Parameter(nameof(Quest.BaseId), id));
            if (result == null) return null;

            var guide = await _questGuideRepository.GetById(result.GuideId);
            var itemExchanges = await _questItemExchangeRepository.GetAllById(result.ExchangeId);
            var description = await _questNonPlayerCharacterDescriptionRepository.GetById(result.NpcRowId);

            result.UpdateGuide(guide);
            result.UpdateItemExchanges(itemExchanges);
            result.UpdateNpcDescription(description);

            return result;
        }

        public async Task<Quest[]> GetAllByGuideId(int guideId)
        {
            return await All(new Parameter(nameof(Quest.GuideId), guideId));
        }

        public async Task<Quest[]> GetAll(QuestFilterType[] filter, string search = null)
        {
            var quests = await All();
            var guides = await _questGuideRepository.All();
            var descriptions = await _questNonPlayerCharacterDescriptionRepository.All();
            var exchanges = await _questItemExchangeRepository.All();

            foreach (var item in quests)
            {
                var guide = guides.SingleOrDefault(x => x.BaseId == item.GuideId);
                var description = descriptions.SingleOrDefault(x => x.BaseId == item.NpcRowId);
                var exchange = exchanges.Where(x => x.BaseId == item.ExchangeId).ToArray();

                item.UpdateGuide(guide);
                item.UpdateNpcDescription(description);
                item.UpdateItemExchanges(exchange);
            }

            foreach (var item in filter)
            {
                switch (item)
                {
                    case QuestFilterType.Level:
                        quests = quests
                            .Where(x => x.RequiredLevel <= Client.Main.GetCharacterLevel())
                            .ToArray();
                        break;

                    case QuestFilterType.Race:
                        quests = quests
                            .Where(x => x.RaceBaseId == (int)Client.Main.GetCharacterRaceType() || x.RaceBaseId == (int)CharacterRaceType.All)
                            .ToArray();
                        break;

                    case QuestFilterType.Class:
                        quests = quests
                            .Where(x => x.ClassBaseId == (int)Client.Main.GetCharacterClassType())
                            .ToArray();
                        break;

                    case QuestFilterType.Zone:
                        quests = quests
                            .Where(x => x.ZoneId == Client.Main.GetCharacterZoneId())
                            .ToArray();
                        break;

                    case QuestFilterType.Money:
                        quests = quests
                            .Where(x => x.ItemExchanges?.Any(i => i.HasMoney) == true)
                            .ToArray();
                        break;

                    case QuestFilterType.Experience:
                        quests = quests
                            .Where(x => x.ItemExchanges?.Any(i => i.HasExperience) == true)
                            .ToArray();
                        break;

                    case QuestFilterType.Hunt:
                        quests = quests
                            .Where(x => x.ItemExchanges?.Any(i => i.Requests?.Any(r => r.IsHunt) == true) == true)
                            .ToArray();
                        break;
                }
            }
            if (!string.IsNullOrEmpty(search))
                quests = quests
                    .Where(x => x.Guide?.Title?.Contains(search) == true ||
                    x.Guide?.Description?.Contains(search) == true ||
                    x.NpcDescription?.Title?.Contains(search) == true ||
                    x.NpcDescription?.TitleExplanation?.Contains(search) == true)
                    .ToArray();

            return quests.OrderBy(x => x.RequiredLevel).ToArray();
        }
    }
}
