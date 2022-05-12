using KO.Core.Models.Query;
using KO.Domain.Items;
using KO.Infrastructure.Repositories;
using System.Threading.Tasks;

namespace KO.Application.Items.Repositories
{
    public class ItemExtensionRepository : BaseRepository<ItemExtension>
    {
        public async Task<ItemExtension> GetByItemId(int number, int itemId)
        {
            var baseId = itemId.ToString().Length > 9 ? ((itemId % 1000) + 1000) : itemId % 1000;

            return await Get(new Parameter(nameof(ItemExtension.Number), number), new Parameter(nameof(ItemExtension.BaseId), baseId));
        }
    }
}
