using KO.Application.Addresses.Handlers;
using KO.Application.Characters.Handlers;
using KO.Application.Targets.Extensions;
using KO.Core.Extensions;
using KO.Domain.Characters;
using System.Linq;
using System.Threading.Tasks;

namespace KO.Application.Boxes.Handlers
{
    public static class BoxActionHandler
    {
        public static async Task BoxCollectAction(this Character character)
        {
            if (!Client.BoxCollect.IsCollect || !character.IsBoxCollector || !Client.BoxLoots.Any()) return;

            var box = Client.BoxLoots.OrderBy(x => character.GetTargetDistance(x.X, x.Y)).FirstOrDefault();
            if (box == null) return;

            if (character.GetTargetDistance(box.X, box.Y) > 2)
                await character.Walk(box.X, box.Y, CharacterWalkType.Teleport);
            else
                await character.Send("24", box.Id.ConvertToDword());

            character.UpdateWalkExpireTime();
        }
    }
}
