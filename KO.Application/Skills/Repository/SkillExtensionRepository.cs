using KO.Core.Constants.Query;
using KO.Core.Models.Query;
using KO.Domain.Skills;
using KO.Infrastructure.Repositories;
using System.Threading.Tasks;

namespace KO.Application.Skills.Repository
{
    public class SkillExtensionRepository : BaseRepository<SkillExtension>
    {
        public async Task<SkillExtension[]> GetAllById(int number)
        {
            return await All(new Parameter(nameof(SkillExtension.Number), number));
        }

        public async Task<SkillExtension[]> GetPotionSkillExtensions()
        {
            return await All(new Parameter(nameof(SkillExtension.Number), 3),
                new Parameter(nameof(SkillExtension.IsPotion), true),
                new Parameter(nameof(SkillExtension.PotionValue), 0, OperatorType.Greater));
        }
    }
}
