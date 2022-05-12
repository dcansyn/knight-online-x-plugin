using KO.Core.Models.Query;
using KO.Domain.Quests;
using KO.Infrastructure.Repositories;
using System.Threading.Tasks;

namespace KO.Application.Quests.Repositories
{
    public class QuestItemExchangeRepository : BaseRepository<QuestItemExchange>
    {
        public async Task<QuestItemExchange[]> GetAllById(int id)
        {
            return await All(new Parameter(nameof(QuestItemExchange.BaseId), id));
        }

        public async Task<QuestItemExchange> GetById(int id)
        {
            return await Get(new Parameter(nameof(QuestItemExchange.BaseId), id));
        }
    }
}
