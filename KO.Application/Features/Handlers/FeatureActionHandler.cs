using KO.Application.Addresses.Handlers;
using KO.Application.Characters.Extensions;
using KO.Application.Characters.Handlers;
using KO.Application.Items.Extensions;
using KO.Application.Quests.Handlers;
using KO.Application.Quests.Repositories;
using KO.Application.Skills.Extensions;
using KO.Application.Skills.Handlers;
using KO.Application.Targets.Extensions;
using KO.Core.Constants;
using KO.Core.Extensions;
using KO.Domain.Characters;
using KO.Domain.Packets;
using KO.Domain.Quests;
using KO.Domain.Skills;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace KO.Application.Features.Handlers
{
    public static class FeatureActionHandler
    {
        public static async Task SpeedHackAction(this Character character)
        {
            if (Client.CharacterFeature.Speed && WinApi.GetAsyncKeyState((int)Keys.LControlKey) > 0 && DateTime.Now > Client.WalkTime)
            {
                await character.Walk(character.GetCharacterMouseX(), character.GetCharacterMouseY(), CharacterWalkType.Jump);
                Client.WalkTime = DateTime.Now.AddMilliseconds(100);
            }

            if (Client.CharacterFeature.Speed && WinApi.GetAsyncKeyState((int)Keys.LControlKey) > 0 && WinApi.GetAsyncKeyState((int)Keys.F) > 0)
                await character.Walk(character.GetCharacterMouseX(), character.GetCharacterMouseY(), CharacterWalkType.Teleport);
        }
    }
}
