using KO.Core.Constants.Query;
using KO.Core.Extensions;
using KO.Core.Models.Query;
using KO.Domain.Parties;
using KO.Domain.Skills;
using KO.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KO.Application.Skills.Repository
{
    public class SkillRepository : BaseRepository<Skill>
    {
        private readonly SkillExtensionRepository _skillExtensionRepository;

        public SkillRepository()
        {
            _skillExtensionRepository = new SkillExtensionRepository();
        }

        public async Task<Skill[]> GetTarotCardSkills()
        {
            return await All(new Parameter(nameof(Skill.Name), "Taro", OperatorType.Like));
        }

        public async Task<Skill> GetById(int id)
        {
            return await Get(new Parameter(nameof(Skill.BaseId), id));
        }

        public async Task<Skill[]> GetPotionSkills()
        {
            var skills = await All(new Parameter(nameof(Skill.ExtensionNumber), 3), new Parameter(nameof(Skill.ClassId), 0));
            var extensions = await _skillExtensionRepository.GetPotionSkillExtensions();

            return skills.Join(extensions, p => p.BaseId, e => e.BaseSkillId, (skill, extension) =>
            {
                skill.UpdateSkillExtension(extension);
                return skill;
            }).ToArray();
        }

        public async Task<Skill[]> GetPartySkills()
        {
            var codes = GetPartySkillCodes();
            var skills = await All(new Parameter(nameof(Skill.BaseId), codes, OperatorType.In));
            var extensions = await _skillExtensionRepository.GetAllById(3);

            foreach (var skill in skills)
            {
                var extension = extensions.FirstOrDefault(x => x.BaseSkillId == skill.BaseId);

                if (skill.ExtensionNumber == 3 && extension != null)
                    skill.UpdateSkillExtension(extension);
            }

            return skills.ToArray();
        }

        public async Task<Skill[]> GetMagicHammerSkills()
        {
            return await All(new Parameter(nameof(Skill.Name), "Hammer", OperatorType.Like));
        }

        private static int[] GetPartySkillCodes()
        {
            var classTypeCodes = new[] { 104, 111, 211, 112, 212 };
            var skillTypes = new Enum[] { HealSkillType.Closed, BufferSkillType.Closed, DefenceSkillType.Closed, RestoreSkillType.Closed, ResistanceSkillType.Closed, GroupHealSkillType.GroupMassiveHealing };
            var codes = new List<int>();

            foreach (var classType in classTypeCodes)
                foreach (var skillType in skillTypes)
                    foreach (var data in skillType.List())
                    {
                        if (int.TryParse(data.Group, out int _))
                            codes.Add(Convert.ToInt32($"{classType}{data.Group}"));
                    }

            return codes.ToArray();
        }
    }
}