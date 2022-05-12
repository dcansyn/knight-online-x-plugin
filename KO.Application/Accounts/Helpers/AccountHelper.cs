using KO.Application.Characters.Extensions;
using KO.Core.Constants;
using KO.Core.Helpers.Memory;
using KO.Domain.Characters;
using System.Linq;
using System.Threading.Tasks;

namespace KO.Application.Accounts.Helpers
{
    public static class AccountHelper
    {
        public static async Task<bool> Open(string path, string name, string arguments)
        {
            return await MemoryHelper.OpenParentProcess(path, name, arguments);
        }

        public static async Task<Character[]> GetAll()
        {
            var result = MemoryHelper.GetProcesses(Settings.GameDefaultName)
                .Select(x =>
                {
                    var character = new Character(x.MainWindowTitle, false);

                    character.UpdateInformation(character.GetCharacterName(), character.GetCharacterLevel(), character.GetCharacterClassNameType());

                    return character;
                })
                .OrderBy(x => x.GameName)
                .ToArray();

            return await Task.FromResult(result);
        }
    }
}
