using KO.Core.Models.Query;
using KO.Domain.Items;
using KO.Infrastructure.Repositories;
using System.Threading.Tasks;

namespace KO.Application.Items.Repositories
{
    public class ItemRepository : BaseRepository<Item>
    {
        private readonly ItemExtensionRepository _itemExtensionRepository;

        public ItemRepository()
        {
            _itemExtensionRepository = new ItemExtensionRepository();
        }

        public async Task<Item> GetByItemId(int itemId)
        {
            if (itemId < 0) return null;

            var baseId = itemId / 1000 * 1000;
            var result = await Get(new Parameter(nameof(Item.BaseId), baseId));
            if (result == null) return null;

            var itemExtension = await _itemExtensionRepository.GetByItemId(result.ExtensionNumber, itemId);
            result.UpdateExtension(itemExtension);
            result.UpdateItemId(itemId);

            return result;
        }
    }
}
