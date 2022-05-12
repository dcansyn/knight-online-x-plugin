using KO.Application.Addresses.Handlers;
using KO.Application.Characters.Handlers;
using KO.Application.Items.Extensions;
using KO.Application.Items.Handlers;
using KO.Application.Quests.Handlers;
using KO.Application.Quests.Repositories;
using KO.Application.Targets.Extensions;
using KO.Core.Constants;
using KO.Core.Extensions;
using KO.Domain.Characters;
using KO.Domain.Items;
using System.Linq;
using System.Threading.Tasks;

namespace KO.Application.Features.Handlers
{
    public static class FeatureHandler
    {
        public static async Task Town()
        {
            Parallel.ForEach(Client.Characters, async (character) =>
            {
                character.UpdateFollowExpireTime(2000);

                await character.Send("4800");
            });

            await Task.CompletedTask;
        }

        public static async Task Teleport(int x, int y)
        {
            Parallel.ForEach(Client.Characters, async (character) =>
            {
                character.UpdateFollowExpireTime(2000);

                await character.Walk(x, y, CharacterWalkType.Teleport);
            });

            await Task.CompletedTask;
        }

        public static async Task DisableWall(bool status)
        {
            Parallel.ForEach(Client.Characters, async (character) =>
            {
                await character.DisableWall(status);
            });

            await Task.CompletedTask;
        }

        public static async Task Oreads(bool status)
        {
            Parallel.ForEach(Client.Characters, async (character) =>
            {
                await character.FakeOreads(status);
            });

            await Task.CompletedTask;
        }

        public static async Task MoradonBuff()
        {
            Parallel.ForEach(Client.Characters, async (character) =>
            {
                var npc = character.GetTargetByName("Enchanter");
                if (npc == null || npc.Distance > 3) return;

                // Attack
                await character.Send("2001", npc.HexId);
                await character.Send("55001233313530385F4E456E6368616E742E6C7561");
                await character.Send("55001233313530385F4E456E6368616E742E6C7561");
                await character.Send("55001233313530385F4E456E6368616E742E6C7561");
                await character.Send("55001233313530385F4E456E6368616E742E6C7561");

                // Defence
                await character.Send("2001", npc.HexId);
                await character.Send("55001233313530385F4E456E6368616E742E6C7561");
                await character.Send("55001233313530385F4E456E6368616E742E6C7561");
                await character.Send("55001233313530385F4E456E6368616E742E6C7561");
                await character.Send("55011233313530385F4E456E6368616E742E6C7561");

                // Physical Strength
                await character.Send("2001", npc.HexId);
                await character.Send("55001233313530385F4E456E6368616E742E6C7561");
                await character.Send("55001233313530385F4E456E6368616E742E6C7561");
                await character.Send("55001233313530385F4E456E6368616E742E6C7561");
                await character.Send("55021233313530385F4E456E6368616E742E6C7561");
                await character.Send("55001233313530385F4E456E6368616E742E6C7561");

                // Strong Legs
                await character.Send("2001", npc.HexId);
                await character.Send("55001233313530385F4E456E6368616E742E6C7561");
                await character.Send("55001233313530385F4E456E6368616E742E6C7561");
                await character.Send("55001233313530385F4E456E6368616E742E6C7561");
                await character.Send("55031233313530385F4E456E6368616E742E6C7561");
            });

            await Task.CompletedTask;
        }

        public static async Task GoMonsterStoneArea()
        {
            Parallel.ForEach(Client.Characters, async (character) =>
            {
                await character.Send("5F06971BA735");
            });

            await Task.CompletedTask;
        }

        public static async Task GetMonsterStones()
        {
            Parallel.ForEach(Client.Characters, async (character) =>
            {
                await character.Send("99028201"); //Reach Level 10
                await character.Send("99028301"); //Reach Level 20
                await character.Send("99028401"); //Reach Level 30
                await character.Send("99028501"); //Reach Level 40
                await character.Send("99028601"); //Reach Level 50
                await character.Send("99028701"); //Reach Level 60
                await character.Send("99028801"); //Reach Level 70
                await character.Send("99028901"); //Reach Level 80
                await character.Send("99025600"); //Merchant's Daughter
                await character.Send("99025800"); //Chief Guard Patrick
                await character.Send("99026500"); //Chief Hunting I
                await character.Send("99027400"); //Chief Hunting II
                await character.Send("99020200"); //10 Kill Görevi 1
                await character.Send("99021000"); //10 Kill Görevi 2
                await character.Send("99025F00"); //1.000 Mob kesme
                await character.Send("99026200"); //30.000 Mob kesme
                await character.Send("99026A00"); //Battle With Fog
            });

            await Task.CompletedTask;
        }

        public static async Task UpgradeAll()
        {
            Parallel.ForEach(Client.Characters, async (character) =>
            {
                var items = (await character.GetDataItems())
                .Where(x => x.InventoryStatus == ItemInventoryStatusType.Row && x.ItemId > 0)
                .ToList();

                var scroll = items.LastOrDefault();
                if (scroll == null)
                    return;

                for (int i = 0; i < items.Count - 1; i++)
                {
                    var item = items[i];
                    if (item.ItemId > 0)
                        await character.Send("5B02011427",
                            item.ItemId.ConvertToDword(),
                            (item.Row - Settings.KO_OFF_INVENTORY_START_ROW).ConvertToDword(1),
                            scroll.ItemId.ConvertToDword(),
                            "1B00000000FF00000000FF00000000FF00000000FF00000000FF00000000FF00000000FF00000000FF");
                }
            });

            await Task.CompletedTask;
        }
    }
}
