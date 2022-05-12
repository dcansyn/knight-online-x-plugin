using KO.Domain.Skills;
using KO.Infrastructure.Data;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KO.Infrastructure.Services.Tables
{
    public class SkillTableService
    {
        public string GamePath { get; set; }
        private readonly TableService _tableService;

        public SkillTableService(string gamePath)
        {
            GamePath = gamePath;
            _tableService = new TableService();
        }

        public async Task SaveSkills()
        {
            using (var db = new BaseDbContext())
            {
                var data = _tableService.GetList(_tableService.GetTable(Path.Combine(GamePath, "Skill_Magic_Main_jp.tbl")));
                var model = data.Select(x => new Skill(
                    Convert.ToInt32(x[0]),          // BaseId 
                    x[2],                           // Name
                    x[3],                           // Description
                    Convert.ToInt32(x[7]),          // SelfEffect 
                    Convert.ToInt32(x[11]),         // RequiredFlyEffect 
                    Convert.ToInt32(x[14]),         // MoralTypeBase 
                    Convert.ToInt32(x[15]),         // Point 
                    Convert.ToInt32(x[16]),         // ClassBaseId 
                    Convert.ToInt32(x[17]),         // Mana 
                    Convert.ToInt32(x[21]),         // RequiredItem 
                    Convert.ToInt32(x[22]),         // CastEffect 
                    Convert.ToInt32(x[23]),         // CooldownBase 
                    Convert.ToInt32(x[27]),         // MaxRange 
                    Convert.ToInt32(x[28])          // ExtensionNumber
                    ))
                    .OrderBy(x => x.BaseId)
                    .ToArray();

                db.Skills.AddRange(model);

                await db.SaveChangesAsync();
            }
        }

        public async Task SaveSkillExtensions()
        {
            using (var db = new BaseDbContext())
            {
                var files = Directory.GetFiles(GamePath)
                    .Where(x =>
                    {
                        var fileInfo = new FileInfo(x);
                        return fileInfo.Name.ToLower().Contains("skill_magic_") && fileInfo.Name.Length <= "skill_magic_99.tbl".Length;
                    })
                    .OrderBy(x => x)
                    .ToArray();

                foreach (var item in files)
                {
                    var fileInfo = new FileInfo(item);
                    var data = _tableService.GetList(_tableService.GetTable(fileInfo.FullName));
                    var number = Convert.ToInt32(fileInfo.Name.ToLower().Split(new string[] { "_", ".tbl" }, StringSplitOptions.RemoveEmptyEntries)[2]);
                    switch (number)
                    {
                        case 2:
                            db.SkillExtensions.AddRange(data.Select(x => new SkillExtension(number, Convert.ToInt32(x[0]), Convert.ToInt32(x[5]))).ToArray());
                            break;
                        case 3:
                            db.SkillExtensions.AddRange(data.Select(x => new SkillExtension(number, Convert.ToInt32(x[0]), Convert.ToInt32(x[2]), Convert.ToInt32(x[3]))).ToArray());
                            break;
                        case 4:
                            db.SkillExtensions.AddRange(data.Select(x => new SkillExtension(number, Convert.ToInt32(x[0]), Convert.ToInt32(x[1]), Convert.ToInt32(x[2]), Convert.ToInt32(x[3]))).ToArray());
                            break;
                        default:
                            db.SkillExtensions.AddRange(data.Select(x => new SkillExtension(number, Convert.ToInt32(x[0]))).ToArray());
                            break;
                    }
                }
                await db.SaveChangesAsync();
            }
        }
    }
}