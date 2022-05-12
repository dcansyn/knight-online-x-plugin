using KO.Core.Helpers.Storage;
using KO.Domain.Characters;
using KO.Domain.Items;
using KO.Domain.Quests;
using KO.Domain.Skills;
using SQLite.CodeFirst;
using System.Data.Entity;
using System.Data.SQLite;

namespace KO.Infrastructure.Data
{
    public class BaseDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemExtension> ItemExtensions { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<SkillExtension> SkillExtensions { get; set; }
        public DbSet<NonPlayerCharacter> NonPlayerCharacters { get; set; }
        public DbSet<NonPlayerCharacterMap> NonPlayerCharacterMaps { get; set; }
        public DbSet<Quest> Quests { get; set; }
        public DbSet<QuestGuide> QuestGuides { get; set; }
        public DbSet<QuestNonPlayerCharacterDescription> QuestNonPlayerCharacterDescriptions { get; set; }
        public DbSet<QuestNonPlayerCharacterExchange> QuestNonPlayerCharacterExchanges { get; set; }
        public DbSet<QuestItemExchange> QuestItemExchanges { get; set; }

        public BaseDbContext() : base(new SQLiteConnection(StorageHelper.GetDataConnectionString()), false) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new SqliteDropCreateDatabaseWhenModelChanges<BaseDbContext>(modelBuilder));
        }
    }
}
