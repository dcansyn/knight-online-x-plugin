using KO.Application.Addresses.Handlers;
using KO.Application.Characters.Extensions;
using KO.Application.Skills.Extensions;
using KO.Core.Extensions;
using KO.Domain.Characters;
using KO.Domain.Skills;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KO.Application.Skills.Handlers
{
    public static class SkillHandler
    {
        public static async Task UseCureCurse(this Character character, Character target = null)
        {
            await character.UseSkill(new Skill(character.GetSkillId(525), 2, 15, 15), target);
        }

        public static async Task UseCureDisease(this Character character, Character target = null)
        {
            await character.UseSkill(new Skill(character.GetSkillId(535), 2, 15, 15), target);
        }

        public static async Task UseRegular(this Character character, Character target = null)
        {
            await character.Send("080101", target.HexId, "D100F9FF00");
        }

        public static async Task UseTransformationScroll(this Character character, TransformationType transformationType)
        {
            var transformationTypeData = transformationType.Get();
            if (transformationTypeData.Order > character.GetCharacterLevel()) return;

            await character.UseSkill(new Skill(Convert.ToInt32(transformationTypeData.Description)));
        }

        public static async Task RemoveBuff(this Character character, BuffSkillType buffType)
        {
            var buffTypeData = buffType.Get();
            await character.RemoveSkill(new Skill(buffTypeData.Value));
        }

        public static async Task UseBuff(this Character character, BuffSkillType buffType, int[] itemIdList)
        {
            var buffTypeData = buffType.Get();
            if (int.TryParse(buffTypeData.Prompt, out int reqItemId) && !itemIdList.Contains(reqItemId)) return;

            var skill = new Skill(buffTypeData.Value);
            switch (buffType)
            {
                case BuffSkillType.BlessOfTemplateDefence:
                case BuffSkillType.BlessOfTemplateStat:
                    skill.UpdateMoralTypeBase(9999);
                    break;
            }

            await character.UseSkill(skill);
        }

        public static async Task UseSuicide(this Character character)
        {
            await character.Send("290103");
            await character.Send("1200");
        }

        public static async Task UseRainOfArrowShower(this Character character, bool different = true)
        {
            var targets = character.Targets.Where(x => x.Distance <= 50 && x.Id != character.Target.Id).Take(8).ToList();
            if (targets.Count >= 3 && targets.Count < 5)
            {
                await character.UseRainOfArrowMultiple(targets.ToArray(), false);
                return;
            }

            if (targets.Count >= 3 && targets.Count <= 5)
            {
                await character.UseRainOfArrowMultiple(targets.Take(3).ToArray(), false);
                await character.UseRainOfArrowShower(targets.Take(5).ToArray(), false);
                return;
            }

            if (targets.Count > 5)
            {
                await character.UseRainOfArrowMultiple(targets.Take(3).ToArray(), false);
                await character.UseRainOfArrowShower(targets.Skip(3).Take(5).ToArray(), false);
                return;
            }
        }

        public static async Task UseRainOfArrowMultiple(this Character character, bool different = true)
        {
            var targets = character.Targets.Where(x => x.Distance <= 50 && x.Id != character.Target.Id).Take(3);
            await character.UseRainOfArrowMultiple(targets.ToArray());
        }

        public static async Task UseRainOfArrowShower(this Character character, Character target)
        {
            await character.UseRainOfArrowShower(new Character[] { target, target, target, target, target });
        }

        public static async Task UseRainOfArrowShower(this Character character, Character[] targets, bool isSingle = true)
        {
            var skillId = Convert.ToInt32($"{character.GetCharacterClassId()}555");
            if (character.Mana < 150 || character.GetCharacterSkillPoint(CharacterSkillType.Tab1) < 55 || (isSingle && targets.First().Distance > 1)) return;

            for (int i = 0; i < targets.Length; i++)
            {
                if (i == 0)
                {
                    await character.Send("3101", skillId.ConvertToDword(), character.HexId, targets[i].HexId, "00000000000000000000000000000F00");
                    await character.Send("3102", skillId.ConvertToDword(), character.HexId, targets[i].HexId, "000000000000010000000000");
                }

                await character.Send("3103", skillId.ConvertToDword(), character.HexId, targets[i].HexId, "0000000000000", (i + 1).ToString(), "000000000000000000");
                await character.Send("3104", skillId.ConvertToDword(), character.HexId, targets[i].HexId, targets[i].GetSkillCoor(), "9BFF0", (i + 1).ToString(), "00000000000000");
            }
        }

        public static async Task UseRainOfArrowMultiple(this Character character, Character target)
        {
            await character.UseRainOfArrowMultiple(new Character[] { target, target, target });
        }

        public static async Task UseRainOfArrowMultiple(this Character character, Character[] targets, bool isSingle = true)
        {
            var skillId = Convert.ToInt32($"{character.GetCharacterClassId()}515");
            if (character.Mana < 40 || character.GetCharacterSkillPoint(CharacterSkillType.Tab1) < 15 || (isSingle && targets.First().Distance > 1)) return;

            for (int i = 0; i < targets.Length; i++)
            {
                if (i == 0)
                {
                    await character.Send("3101", skillId.ConvertToDword(), character.HexId, targets[i].HexId, "00000000000000000000000000000D00");
                    await character.Send("3102", skillId.ConvertToDword(), character.HexId, targets[i].HexId, "000000000000010000000000");
                }

                await character.Send("3103", skillId.ConvertToDword(), character.HexId, targets[i].HexId, "0000000000000", (i + 1).ToString(), "000000000000000000");
                await character.Send("3104", skillId.ConvertToDword(), character.HexId, targets[i].HexId, targets[i].GetSkillCoor(), "9BFF0", (i + 1).ToString(), "00000000000000");
            }
        }

        public static async Task RemoveSkill(this Character character, Skill skill)
        {
            await character.Send("3106", skill.BaseId.ConvertToDword(), character.HexId, character.HexId);
        }

        public static async Task UseSkill(this Character character, Skill skill, Character target = null)
        {
            if (skill == null || character.UsedSkills.Any(x => x.BaseId == skill.BaseId)) return;

            target = target ?? character;

            var targetCoor = skill.MoralTypeBase == 10 || skill.MoralTypeBase == 6 ? target.GetSkillCoor() : "000000000000";
            var sb = new StringBuilder[3];
            for (int i = 0; i <= 2; i++)
            {
                sb[i] = new StringBuilder();
                sb[i]
                    .Append("310")
                    .Append((i + 1).ToString())
                    .Append(skill.BaseId.ConvertToDword())
                    .Append(character.HexId);

                if (skill.MoralTypeBase == 10 || skill.MoralTypeBase == 6 || skill.MoralTypeBase == 9999)
                    sb[i].Append("FFFF");
                else
                    sb[i].Append(target.HexId);

                sb[i].Append(targetCoor);
            }

            sb[0].Append("0000000000000000");
            sb[0].Append(skill.CastEffectBase.ConvertToDword(2));

            if (skill.ExtensionNumber == 2)
            {
                if (skill.Extension?.ArrowCount > 1)
                {
                    sb[1].Append("0100");
                    sb[2].Append("0100");
                }
                else
                {
                    sb[1].Append("0000");
                    sb[2].Append("0000");
                }
            }
            else
            {
                sb[1].Append("0000");
                sb[2].Append("0000");
            }

            sb[1].Append("00000000");
            sb[2].Append("0000000000000000");

            if (skill.CastEffect > 0)
            {
                await character.Send(sb[0].ToString());

                if (skill.RequiredFlyEffect > 0)
                    await character.Send(sb[1].ToString());

                if (skill.ExtensionNumber == 2 && skill.Extension?.ArrowCount > 1)
                {
                    for (int i = 0; i < skill.Extension.ArrowCount; i++)
                    {
                        await character.Send("3103", sb[2].ToString().Substring(4, 16), "0000000000000", i.ToString(), "000000000000000000");
                        await character.Send("3104", sb[2].ToString().Substring(4, 16), targetCoor, "9BFF0", i.ToString(), "00000000000000");
                    }
                }
                else
                    await character.Send(sb[2].ToString());
            }
            else
            {
                await character.Send(sb[2].ToString());
            }

            skill.UpdateUseTime();
            skill.UpdateTargetId(target.Id);
            character.UpdateUsedSkills(character.UsedSkills.Append(skill).ToArray());
            return;
        }

        public static async Task UseSpecialSkill(this Character character, Character target)
        {
            var specialSkills = Client.Skills.Where(x => x.ClassType == character.ClassType && (x.Type == SkillType.Special || x.Type == SkillType.RainOfArrows));

            switch (character.ClassType)
            {
                case CharacterClassType.Rogue:
                    if (specialSkills.Any(x => x.Name.Contains("Multiple Shot")) || specialSkills.Any(x => x.Name.Contains("Arrow Shower")))
                    {
                        if (specialSkills.Any(x => x.Name.Contains("Multiple Shot")) &&
                            character.GetCharacterSkillPoint(CharacterSkillType.Tab1) >= 15 &&
                            character.Mana >= 40 &&
                            character.Target.Distance < 3)
                        {
                            await character.UseRainOfArrowMultiple(target);
                            character.UpdateAttackSkillExpireTime();
                        }

                        if (specialSkills.Any(x => x.Name.Contains("Arrow Shower")) &&
                            character.GetCharacterSkillPoint(CharacterSkillType.Tab1) >= 55 &&
                            character.Mana >= 190 &&
                            character.Target.Distance < 3)
                        {
                            await character.UseRainOfArrowShower(target);
                            character.UpdateAttackSkillExpireTime();
                        }
                        return;
                    }

                    if (specialSkills.Any(x => x.Name.Contains("Rain of Arrows")))
                    {
                        if (specialSkills.Any(x => x.Description == "4"))
                        {
                            await character.UseRainOfArrowMultiple(Client.CharacterTarget.SelectTargetStatusType == SelectTargetStatusType.Personal);
                            character.UpdateAttackSkillExpireTime();
                        }
                        else if (specialSkills.Any(x => x.Description == "9"))
                        {
                            await character.UseRainOfArrowShower(Client.CharacterTarget.SelectTargetStatusType == SelectTargetStatusType.Personal);
                            character.UpdateAttackSkillExpireTime();
                        }
                    }

                    break;
            }
        }
    }
}
