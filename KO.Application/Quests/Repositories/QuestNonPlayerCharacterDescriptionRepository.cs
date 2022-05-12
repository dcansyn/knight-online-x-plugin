using KO.Core.Models.Query;
using KO.Domain.Quests;
using KO.Infrastructure.Repositories;
using System.Threading.Tasks;

namespace KO.Application.Quests.Repositories
{
    public class QuestNonPlayerCharacterDescriptionRepository : BaseRepository<QuestNonPlayerCharacterDescription>
    {
        public async Task<QuestNonPlayerCharacterDescription> GetById(int id)
        {
            return await Get(new Parameter(nameof(QuestNonPlayerCharacterDescription.BaseId), id));
        }
    }
}
