using KO.Core.Models.Query;
using KO.Domain.Quests;
using KO.Infrastructure.Repositories;
using System.Threading.Tasks;

namespace KO.Application.Quests.Repositories
{
    public class QuestGuideRepository : BaseRepository<QuestGuide>
    {
        public async Task<QuestGuide> GetById(int id)
        {
            return await Get(new Parameter(nameof(QuestGuide.BaseId), id));
        }
    }
}
