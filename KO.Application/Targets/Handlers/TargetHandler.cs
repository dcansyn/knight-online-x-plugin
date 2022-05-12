using KO.Application.Addresses.Extensions;
using KO.Application.Addresses.Handlers;
using KO.Core.Constants;
using KO.Core.Extensions;
using KO.Domain.Characters;
using System.Threading.Tasks;

namespace KO.Application.Targets.Handlers
{
    public static class TargetHandler
    {
        public static async Task SelectTarget(this Character character, int id)
        {
            await character.WriteLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_MOB, id);
        }

        public static async Task PM(this Character character, string nick, string message)
        {
            await character.Send("3501", nick.Length.ConvertToDword(1), "00", nick.ConvertStringToHex());
            await character.Send("1002", message.Length.ConvertToDword(1), "00", message.ConvertStringToHex());
        }
    }
}
