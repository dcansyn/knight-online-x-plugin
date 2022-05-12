using KO.Domain.Items;
using KO.Infrastructure.Data;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KO.Infrastructure.Services.Tables
{
    public class ItemTableService
    {
        public string GamePath { get; set; }
        private readonly TableService _tableService;

        public ItemTableService() { }

        public ItemTableService(string gamePath)
        {
            GamePath = gamePath;
            _tableService = new TableService();
        }

        public async Task SaveItems()
        {
            using (var db = new BaseDbContext())
            {
                var data = _tableService.GetList(_tableService.GetTable(Path.Combine(GamePath, "item_org_jp.tbl")));

                var model = data.Select(value => new Item(
                    Convert.ToInt32(value[0]),      // BaseId
                    Convert.ToInt32(value[1]),      // ExtensionId
                    value[2],                       // Name
                    Convert.ToInt32(value[4]),      // ExtBaseIdActive
                    Convert.ToInt32(value[6]),      // IconId
                    Convert.ToInt32(value[7]),      // IconDxtId
                    Convert.ToInt32(value[10]),     // KindId
                    Convert.ToInt32(value[13]),     // RaceId
                    Convert.ToInt32(value[14]),     // ClassId
                    Convert.ToInt32(value[15]),     // Damage
                    Convert.ToInt32(value[17]),     // Range
                    Convert.ToInt32(value[18]),     // Weight
                    Convert.ToInt32(value[19]),     // MaxDurability
                    Convert.ToInt32(value[20]),     // SellPrice
                    Convert.ToInt32(value[22]),     // Defense
                    Convert.ToInt32(value[23]),     // IsCountable
                    Convert.ToInt32(value[26]),     // ReqMinLevel
                    Convert.ToInt32(value[28]),     // IsPet
                    Convert.ToInt32(value[30]),     // ReqStatStrength
                    Convert.ToInt32(value[31]),     // ReqStatHealth
                    Convert.ToInt32(value[32]),     // ReqStatDexterity
                    Convert.ToInt32(value[33]),     // ReqStatIntellience
                    Convert.ToInt32(value[34]),     // ReqStatMagicPower
                    Convert.ToInt32(value[36])      // ItemScrollGrade
                    ))
                    .OrderBy(x => x.BaseId)
                    .ToArray();

                db.Items.AddRange(model);

                await db.SaveChangesAsync();
            }
        }

        public async Task SaveItemExtensions()
        {
            using (var db = new BaseDbContext())
            {
                var files = Directory.GetFiles(GamePath)
                          .Where(x => x.ToLower().Contains("item") && x.ToLower().Contains("_ext_"))
                          .OrderBy(x => x)
                          .ToArray();

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    var data = _tableService.GetList(_tableService.GetTable(fileInfo.FullName));
                    var number = Convert.ToInt32(fileInfo.Name.ToLower().Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries)[2]);

                    var model = data.Select(value => new ItemExtension(number,
                        Convert.ToInt32(value[0]),      // BaseId
                        value[1],                       // Name
                        Convert.ToInt32(value[2]),      // BaseId
                        value[3],                       // Description
                        Convert.ToInt32(value[5]),      // IconDxtId
                        Convert.ToInt32(value[6]),      // IconId
                        Convert.ToInt32(value[7]),      // TypeId
                        Convert.ToInt32(value[8]),      // Damage
                        Convert.ToInt32(value[9]),      // AttackIntervalPercentage
                        Convert.ToInt32(value[10]),     // AttackPowerRate
                        Convert.ToInt32(value[11]),     // DodgeRate
                        Convert.ToInt32(value[12]),     // MaxDurability
                        Convert.ToInt32(value[13]),     // PriceMultiply
                        Convert.ToInt32(value[14]),     // Defense
                        Convert.ToInt32(value[15]),     // DaggerDefense
                        Convert.ToInt32(value[16]),     // JamadarDefense
                        Convert.ToInt32(value[17]),     // SwordDefense
                        Convert.ToInt32(value[18]),     // ClubDefense
                        Convert.ToInt32(value[19]),     // AxeDefense
                        Convert.ToInt32(value[20]),     // SpearDefense
                        Convert.ToInt32(value[21]),     // ArrowDefense
                        Convert.ToInt32(value[22]),     // FireDamage
                        Convert.ToInt32(value[23]),     // GlacierDamage
                        Convert.ToInt32(value[24]),     // LightningDamage
                        Convert.ToInt32(value[25]),     // PosionDamage
                        Convert.ToInt32(value[26]),     // HpRecovery
                        Convert.ToInt32(value[27]),     // MpDamage
                        Convert.ToInt32(value[28]),     // MpRecovery
                        Convert.ToInt32(value[29]),     // ReturnPhysicalDamage
                        Convert.ToInt32(value[31]),     // StrengthBonus
                        Convert.ToInt32(value[32]),     // HealthBonus
                        Convert.ToInt32(value[33]),     // DexterityBonus
                        Convert.ToInt32(value[34]),     // IntellienceBonus
                        Convert.ToInt32(value[35]),     // MagicPowerBonus
                        Convert.ToInt32(value[36]),     // HpBonus
                        Convert.ToInt32(value[37]),     // MpBonus
                        Convert.ToInt32(value[38]),     // ResistanceToFlame
                        Convert.ToInt32(value[39]),     // ResistanceToGlacier
                        Convert.ToInt32(value[40]),     // ResistanceToLightning
                        Convert.ToInt32(value[41]),     // ResistanceToMagic
                        Convert.ToInt32(value[42]),     // ResistanceToPosion
                        Convert.ToInt32(value[43]),     // ResistanceToCurse
                        Convert.ToInt32(value[49]),     // ReqStatStrength
                        Convert.ToInt32(value[50]),     // ReqStatHealth
                        Convert.ToInt32(value[51]),     // ReqStatDexterity
                        Convert.ToInt32(value[52]),     // ReqStatIntellience
                        Convert.ToInt32(value[53])      // ReqStatMagicPower
                        ))
                        .OrderBy(x => x.BaseId)
                        .ToArray();

                    db.ItemExtensions.AddRange(model);
                }

                await db.SaveChangesAsync();
            }
        }
    }
}