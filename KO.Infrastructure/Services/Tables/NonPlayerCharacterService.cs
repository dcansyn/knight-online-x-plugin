using KO.Domain.Characters;
using KO.Infrastructure.Data;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KO.Infrastructure.Services.Tables
{
    public class NonPlayerCharacterService
    {
        public string GamePath { get; set; }
        private readonly TableService _tableService;
        private readonly QuestTableService _questTableService;

        public NonPlayerCharacterService(string gamePath)
        {
            GamePath = gamePath;
            _tableService = new TableService();
            _questTableService = new QuestTableService(GamePath);
        }

        public async Task SaveNonPlayerCharacters()
        {
            using (var db = new BaseDbContext())
            {
                var data = _tableService.GetList(_tableService.GetTable(Path.Combine(GamePath, "mob_jp.tbl")));
                var model = data.Select(x => new NonPlayerCharacter(
                    Convert.ToInt32(x[0]),
                    x[1],
                    x[3] != "0"
                    ))
                    .OrderBy(x => x.BaseId)
                    .ToArray();

                db.NonPlayerCharacters.AddRange(model);

                await db.SaveChangesAsync();
            }
        }

        public async Task SaveNonPlayerCharacterMaps()
        {
            using (var db = new BaseDbContext())
            {
                var data = _tableService.GetList(_tableService.GetTable(Path.Combine(GamePath, "NpcMopMap_info_jp.tbl")));
                var model = data.Select(x => new NonPlayerCharacterMap(
                        Convert.ToInt32(x[0]),          // Id
                        Convert.ToInt32(x[1]),          // BaseId
                        Convert.ToInt32(x[2]),          // ZoneId
                        Convert.ToInt32(x[3]),          // NpcType
                        Convert.ToInt32(x[4]),          // X
                        Convert.ToInt32(x[5]),          // Y
                        x[6],                           // Name
                        Convert.ToBoolean(x[7] != "0"), // IsMonster
                        Convert.ToInt32(x[8]),          // Race
                        x[9]                            // Description
                    ))
                    .OrderBy(x => x.BaseId)
                    .ToArray();

                db.NonPlayerCharacterMaps.AddRange(model);

                await db.SaveChangesAsync();
            }
        }
    }
}
