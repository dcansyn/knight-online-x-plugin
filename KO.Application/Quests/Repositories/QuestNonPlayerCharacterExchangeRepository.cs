using KO.Core.Models.Query;
using KO.Domain.Quests;
using KO.Infrastructure.Repositories;
using System.Threading.Tasks;

namespace KO.Application.Quests.Repositories
{
    public class QuestNonPlayerCharacterExchangeRepository : BaseRepository<QuestNonPlayerCharacterExchange>
    {
        public async Task<QuestNonPlayerCharacterExchange> GetById(int id)
        {
            return await Get(new Parameter(nameof(QuestNonPlayerCharacterExchange.BaseId), id));
        }
    }
}
