using KO.Application.Characters.Extensions;
using KO.Application.Characters.Handlers;
using KO.Application.Skills.Handlers;
using KO.Application.Targets.Extensions;
using KO.Domain.Characters;
using KO.Domain.Skills;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KO.Application.Targets.Handlers
{
    public static class TargetActionHandler
    {
        private static Task RunTask = Task.CompletedTask;
        private static Task AttackTask = Task.CompletedTask;

        public static async Task TargetAction(this Character character)
        {
            await character.TargetSelectAction();

            if (RunTask.IsCompleted)
                RunTask =
                    Client.CharacterTarget.SelectTargetStatusType == SelectTargetStatusType.Main ?
                    Client.Main.TargetRunAction() : character.TargetRunAction();

            if (AttackTask.IsCompleted)
                AttackTask = character.TargetAttackAction();
        }

        private static async Task TargetSelectAction(this Character character)
        {
            Client.DeadCharacters = Client.DeadCharacters.Where(x => x.DeadExpireTime > DateTime.Now).ToArray();

            if (character.IsMain)
            {
                Client.Targets = character.CollectTargets(Client.CharacterTarget.TargetDistance, Client.CharacterTarget.TargetTypes);
                character.UpdateTargets(Client.Targets.ToArray());
            }
            else if (Client.CharacterTarget.SelectTargetStatusType != SelectTargetStatusType.Main)
                character.UpdateTargets(character.CollectTargets(Client.CharacterTarget.TargetDistance, Client.CharacterTarget.TargetTypes));

            switch (Client.CharacterTarget.SelectTargetStatusType)
            {
                case SelectTargetStatusType.Main:
                    switch (Client.CharacterTarget.SelectTargetType)
                    {
                        case SelectTargetType.AutomaticMonster:
                            var automaticTarget = Client.Main.Targets.FirstOrDefault();

                            character.UpdateTargets(Client.Main.Targets);
                            character.UpdateTarget(automaticTarget);
                            break;

                        case SelectTargetType.Manuel:
                            var target = Client.Main.Targets.FirstOrDefault(x => x.Id == Client.Main.GetTargetId());
                            character.UpdateTarget(target);
                            character.UpdateTargets(Client.Targets);
                            break;

                        case SelectTargetType.EnemyRace:
                            var enemyTarget = Client.Main.Targets.FirstOrDefault(x => x.RaceType != character.RaceType);

                            character.UpdateTargets(Client.Main.Targets.Where(x => x.RaceType != character.RaceType).ToArray());
                            character.UpdateTarget(enemyTarget);
                            break;

                        case SelectTargetType.List:
                            var listTarget = Client.Main.Targets.FirstOrDefault(x => Client.CharacterTarget.TargetNameList.Contains(x.Name));

                            character.UpdateTargets(Client.Main.Targets.Where(x => Client.CharacterTarget.TargetNameList.Contains(x.Name)).ToArray());
                            character.UpdateTarget(listTarget);
                            break;
                    }
                    break;

                case SelectTargetStatusType.Personal:
                    switch (Client.CharacterTarget.SelectTargetType)
                    {
                        case SelectTargetType.AutomaticMonster:
                            var personalTarget = character.Targets.FirstOrDefault();

                            character.UpdateTargets(character.Targets);
                            character.UpdateTarget(personalTarget);
                            break;

                        case SelectTargetType.Manuel:
                            var target = character.Targets.FirstOrDefault(x => x.Id == character.GetTargetId());
                            character.UpdateTarget(target);
                            character.UpdateTargets(character.Targets);
                            break;

                        case SelectTargetType.EnemyRace:
                            var enemyTarget = character.Targets.FirstOrDefault(x => x.RaceType != character.RaceType);

                            character.UpdateTargets(character.Targets.Where(x => x.RaceType != character.RaceType).ToArray());
                            character.UpdateTarget(enemyTarget);
                            break;

                        case SelectTargetType.List:
                            var listTarget = character.Targets.FirstOrDefault(x => Client.CharacterTarget.TargetNameList.Contains(x.Name));

                            character.UpdateTargets(character.Targets.Where(x => Client.CharacterTarget.TargetNameList.Contains(x.Name)).ToArray());
                            character.UpdateTarget(listTarget);
                            break;
                    }
                    break;
            }

            if (character.Target != null && character.Target?.Id > 0)
            {
                await character.SelectTarget(character.Target.Id);
            }
        }

        private static async Task TargetRunAction(this Character character)
        {
            if (!Client.CharacterTarget.IsTargetRun ||
                (Client.CharacterTarget.IsTargetRunWhenTargetDead && Client.CharacterTarget.TargetExpireTime > DateTime.Now) ||
                (character.IsBoxCollector && Client.BoxLoots.Any())) return;

            if (character.Target != null && character.Target.Distance > 1 && character.Target.CenterDistance < Client.CharacterTarget.TargetDistance)
                await character.Walk(character.Target.X, character.Target.Y, Client.CharacterTarget.TargetRunWalkType);

            if (Client.CharacterTarget.IsTargetRunBackToCenter && character.Target == null && character.GetTargetDistance(Client.CharacterTarget.TargetRunCenterX, Client.CharacterTarget.TargetRunCenterY) > 1)
                await character.Walk(Client.CharacterTarget.TargetRunCenterX, Client.CharacterTarget.TargetRunCenterY, Client.CharacterTarget.TargetRunWalkType);
        }

        private static async Task TargetAttackAction(this Character character)
        {
            if (character.Target == null || character.Target.Id == 0 || Client.DeadCharacters.Any(x => x.Id == character.Target.Id)) return;

            if (DateTime.Now > character.RegularSkillExpireTime &&
                character.AttackType == CharacterAttackType.Melee &&
                Client.Skills.Any(x => x.ClassType == character.ClassType && x.Type == SkillType.Regular))
            {
                _ = character.UseRegular(character.Target);
                character.UpdateRegularSkillExpireTime(2000);
            }

            if (DateTime.Now < character.AttackSkillExpireTime) return;

            var usedSkillIdList = character.UsedSkills.Select(x => x.BaseId).ToArray();
            var skill = Client.Skills
                .Where(x => !usedSkillIdList.Contains(x.BaseId))
                .Where(x => x.Mastery == 0 || x.Point <= character.GetCharacterSkillPointByRow(x.Mastery))
                .Where(x => x.RequiredItem == 0 || character.InventoryItems.Any(i => i.ItemId == x.RequiredItem))
                .Where(x => x.MaxRange > character.Target.Distance)
                .Where(x => x.ClassId == character.ClassId)
                .Where(x => x.Mana <= character.Mana)
                .Where(x => x.Type == SkillType.Melee || x.Type == SkillType.Throw || x.Type == SkillType.Area || x.Type == SkillType.Archery)
                .Where(x =>
                {
                    if (character.Target.Name.Contains("'s Chest") && x.Type == SkillType.Melee && character.AttackType == CharacterAttackType.Archery)
                        return false;

                    return true;
                })
                .OrderByDescending(x => x.Cooldown)
                .FirstOrDefault();

            if (skill == null) return;

            await character.UseSkill(skill, character.Target);
            character.UpdateAttackSkillExpireTime();

            if (!character.Target.Name.Contains("'s Chest"))
                await character.UseSpecialSkill(character.Target);
        }
    }
}
