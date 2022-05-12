using KO.Application;
using KO.Application.Characters.Extensions;
using KO.Application.Skills.Extensions;
using KO.Application.Skills.Repository;
using KO.Core.Extensions;
using KO.Domain.Boxes;
using KO.Domain.Characters;
using KO.Domain.Parties;
using KO.Domain.Skills;
using KO.Domain.Supplies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KO.UI.Extensions
{
    public static class ChangeExtensions
    {
        public static async Task ConfirmChanges(this Main form)
        {
            await form.ConfirmGeneralTab();
            await form.ConfirmBoxCollect();
            await form.ConfirmTargetChanges();
            await form.ConfirmWalkChanges();
            await form.ConfirmSkillChanges();
            await form.ConfirmSupplyChanges();
            await form.ConfirmParty();
        }

        private static async Task ConfirmGeneralTab(this Main form)
        {
            Client.CharacterProtection = new CharacterProtection(
                form.RbAutomaticPotion.Checked,
                form.ChkManaPotion.Checked,
                form.ChkMultiManaPotion.Checked,
                Convert.ToInt32(form.TxtManaPotionPercent.Text),
                form.ChkHealthPotion.Checked,
                form.ChkMultiHealthPotion.Checked,
                Convert.ToInt32(form.TxtHealthPotionPercent.Text),
                form.ChkMinor.Checked,
                Convert.ToInt32(form.TxtMinorPercent.Text),
                form.ChkMagicHammer.Checked,
                form.ChkEternity.Checked,
                Convert.ToInt32(form.TxtEternityPercent.Text),
                form.ChkSuicide.Checked,
                Convert.ToInt32(form.TxtSuicidePercent.Text),
                form.ChkSuicideF4.Checked,
                form.ChkGetUpWhenYouDie.Checked,
                form.ChkReturnSlotAfter.Checked);

            Client.CharacterScroll = new CharacterScroll(
                form.ChkAkaraBlessing.Checked,
                form.ChkSeaCucumberPow.Checked,
                form.ChkStatIncrease.Checked,
                form.ChkDefenseIncrease.Checked,
                form.ChkWeaponEnchant.Checked,
                form.ChkArmorEnchant.Checked,
                form.ChkSpiritOfKaufmann.Checked,
                form.ChkTransformationScroll.Checked,
                (TransformationType)Convert.ToInt32(form.CmbTransformationScroll.SelectedIndex));

            Client.CharacterFeature = new CharacterFeature(
                form.ChkSpeed.Checked,
                form.ChkWallHack.Checked);

            Client.CharacterFollow = new CharacterFollow(form.ChkFollow.Checked, (FollowType)form.CmbFollow.SelectedIndex);

            await Task.CompletedTask;
        }

        private static async Task ConfirmBoxCollect(this Main form)
        {
            Client.BoxCollect = new BoxCollect(
                form.ChkCollect.Checked,
                form.ChkCollectGoBox.Checked,
                form.ChkOreads.Checked,
                form.CmbLooter.Text,
                form.RbBoxCollectItemInclude.Checked);

            var lootTypes = BoxCollectType.TypeUnique.List();
            var collectTypes = new List<BoxCollectType>();
            var doNotCollectTypes = new List<BoxCollectType>();

            for (int i = 0; i < form.TvCollectList.Nodes.Count; i++)
            {
                var parentNode = form.TvCollectList.Nodes[i];

                if (i == 0 && parentNode.Checked)
                    Client.BoxCollect.UpdateIsActive(parentNode.Checked);

                if (i == 1 && parentNode.Checked)
                    Client.BoxCollect.UpdateMoneyIsActive(parentNode.Checked);

                if (i > 1)
                    for (int j = 0; j < parentNode.Nodes.Count; j++)
                        for (int n = 0; n < parentNode.Nodes[j].Nodes.Count; n++)
                        {
                            var subNode = parentNode.Nodes[j];
                            var typeNode = subNode.Nodes[n];

                            if (typeNode.Checked)
                            {
                                var type = lootTypes.FirstOrDefault(x => x.Group == subNode.Text && x.DisplayName == typeNode.Text);
                                if (i == 2)
                                    collectTypes.Add((BoxCollectType)type.Self);
                                else
                                    doNotCollectTypes.Add((BoxCollectType)type.Self);
                            }
                        }
            }
            Client.BoxCollect.UpdateCollectTypes(collectTypes.ToArray());
            Client.BoxCollect.UpdateDoNotCollectTypes(doNotCollectTypes.ToArray());
            Client.BoxCollect.UpdateItemIdList(form.LvBoxCollectItems.Items.Cast<ListViewItem>().Select(x => Convert.ToInt32(x.Text)).ToArray());

            foreach (var character in Client.Characters)
                character.UpdateIsBoxCollector(Client.BoxCollect.IsPersonalCollect || Client.BoxCollect.LooterTitle == character.GameName);

            await Task.CompletedTask;
        }

        private static async Task ConfirmTargetChanges(this Main form)
        {
            form.LblMonsterCenter.Text = $"{Client.Main.GetCharacterX()} - {Client.Main.GetCharacterY()}";

            Client.CharacterTarget = new CharacterTarget(
                (SelectTargetStatusType)Convert.ToInt32(form.CmbTargetSelectStatus.SelectedIndex),
                (SelectTargetType)Convert.ToInt32(form.CmbTargetSelectType.SelectedIndex),
                (SelectTargetRaceType)Convert.ToInt32(form.CmbTargetSelectRaceType.SelectedIndex),
                form.ChkTargetRun.Checked,
                form.ChkTargetRunBackToCenter.Checked,
                Client.Main.GetCharacterX(),
                Client.Main.GetCharacterY(),
                Convert.ToInt32(form.TxtMonsterDistance.Text),
                form.ChkTargetDeadWait.Checked,
                form.RbMonsterRunTeleport.Checked ? CharacterWalkType.Teleport : CharacterWalkType.Walk,
                form.LstTargetList.Items.Cast<string>().ToArray());

            switch (Client.CharacterTarget.SelectTargetRaceType)
            {
                case SelectTargetRaceType.Everyone:
                    Client.CharacterTarget.UpdateTargetTypes(new[] { CharacterType.NonPlayerCharacter, CharacterType.Player });
                    break;
                case SelectTargetRaceType.Monster:
                    Client.CharacterTarget.UpdateTargetTypes(new[] { CharacterType.NonPlayerCharacter });
                    break;
                case SelectTargetRaceType.Player:
                    Client.CharacterTarget.UpdateTargetTypes(new[] { CharacterType.Player });
                    break;
            }

            await Task.CompletedTask;
        }

        private static async Task ConfirmWalkChanges(this Main form)
        {
            Client.CharacterWalk = new CharacterWalk(form.ChkRunCoordinate.Checked,
                form.LstRunCoordinate.Items.Cast<string>().ToArray(),
                form.RbRunCoorTeleport.Checked ? CharacterWalkType.Teleport : CharacterWalkType.Walk,
                form.ChkRunCoorTargetEmpty.Checked,
                Convert.ToInt32(form.TxtRunCoorSeconds.Text));

            await Task.CompletedTask;
        }

        private static async Task ConfirmSkillChanges(this Main form)
        {
            using (var skillRepository = new SkillRepository())
            using (var skillExtensionRepository = new SkillExtensionRepository())
            {
                var skills = await skillRepository.All();
                var arrowSkills = await skillExtensionRepository.GetAllById(2);
                Client.PotionSkills = await skillRepository.GetPotionSkills();
                Client.MagicHammerSkills = await skillRepository.GetMagicHammerSkills();
                Client.PartySkills = await skillRepository.GetPartySkills();

                var attackSkills = new List<Skill>();
                var skillTypes = SkillType.Personal.List();
                foreach (var character in Client.Characters)
                {
                    var listView = new ListView();

                    var classType = character.GetCharacterClassType();
                    switch (classType)
                    {
                        case CharacterClassType.Warrior:
                            listView = form.LvAttackWarrior;
                            break;

                        case CharacterClassType.Rogue:
                            listView = form.LvAttackRogue;
                            break;

                        case CharacterClassType.Magician:
                            listView = form.LvAttackMagician;
                            break;

                        case CharacterClassType.Priest:
                            listView = form.LvAttackPriest;
                            break;
                    }

                    var skillList = listView.Items.Cast<ListViewItem>()
                        .Where(x => x.Checked)
                        .Select(x =>
                        {
                            var skillType = (SkillType)(skillTypes.FirstOrDefault(s => s.Name == x.Tag.ToString())?.Self);
                            var skillTypeData = skillType.Get();

                            var skill = new Skill(0);

                            switch (skillType)
                            {
                                case SkillType.Unknown:
                                case SkillType.Regular:
                                case SkillType.Special:
                                case SkillType.RainOfArrows:
                                    skill = new Skill(x.Text, x.SubItems[1].Text);
                                    break;

                                case SkillType.Personal:
                                case SkillType.Melee:
                                case SkillType.Throw:
                                case SkillType.Area:
                                case SkillType.Archery:
                                    skill = skills.FirstOrDefault(s => s.BaseId == character.GetSkillId(x.SubItems[1].Text));

                                    if (skillType == SkillType.Archery)
                                        skill.UpdateSkillExtension(arrowSkills.FirstOrDefault(a => a.BaseSkillId == skill.BaseId));
                                    break;
                                default:
                                    throw new Exception("Skill type not found.");
                            }

                            skill.UpdateType(skillType);
                            skill.UpdateMaxRange(Convert.ToInt32(x.ToolTipText));
                            skill.UpdateCharacterClassType(classType);

                            return skill;
                        }).ToArray();


                    attackSkills.AddRange(skillList);
                }
                Client.Skills = attackSkills.Distinct().ToArray();
            }
        }

        private static async Task ConfirmSupplyChanges(this Main form)
        {
            Client.Supply = new Supply(form.ChkSupplyActive.Checked,
                form.ChkSupplyStartItemConsume.Checked,
                form.ChkSupplyStartInventoryFull.Checked,
                form.RbSupplyTeleport.Checked ? CharacterWalkType.Teleport : CharacterWalkType.Walk,
                form.ChkSupplyMethodWeapons.Checked,
                form.ChkSupplyMethodHelmet.Checked,
                form.ChkSupplyMethodPauldron.Checked,
                form.ChkSupplyMethodPants.Checked,
                form.ChkSupplyMethodGauntlet.Checked,
                form.ChkSupplyMethodBoots.Checked,
                form.ChkSupplyMethodHealthPotion.Checked,
                (SupplyHealthPotionType)form.CmbSupplyMethodHealthPotion.SelectedIndex,
                Convert.ToInt32(form.TxtSupplyMethodHealthPotionCount.Text),
                form.ChkSupplyMethodManaPotion.Checked,
                (SupplyManaPotionType)(form.CmbSupplyMethodManaPotion.SelectedIndex + 6),
                Convert.ToInt32(form.TxtSupplyMethodManaPotionCount.Text),
                form.ChkSupplyMethodWolf.Checked,
                Convert.ToInt32(form.TxtSupplyMethodWolfCount.Text),
                form.ChkSupplyMethodTransformationGem.Checked,
                Convert.ToInt32(form.TxtSupplyMethodTransformationGemCount.Text),
                form.ChkSupplyMethodArrow.Checked,
                Convert.ToInt32(form.TxtSupplyMethodArrowCount.Text),
                form.ChkSupplyMethodPrayerOfGods.Checked,
                Convert.ToInt32(form.TxtSupplyMethodPrayerOfGodsCount.Text),
                form.ChkSupplyBankMethodHealthPotion.Checked,
                (SupplyHealthPotionType)form.CmbSupplyBankMethodHealthPotion.SelectedIndex,
                Convert.ToInt32(form.TxtSupplyBankMethodHealthPotionCount.Text),
                form.ChkSupplyBankMethodManaPotion.Checked,
                (SupplyManaPotionType)(form.CmbSupplyBankMethodManaPotion.SelectedIndex + 6),
                Convert.ToInt32(form.TxtSupplyBankMethodManaPotionCount.Text),
                form.ChkFillToBankPotion.Checked,
                form.LstSupplyAction.Items.Cast<string>().Select(x =>
                {
                    var result = x.Split(new string[] { "-", "CODE", ":" }, StringSplitOptions.RemoveEmptyEntries);

                    return new SupplyAction(Convert.ToInt32(result[1]), Convert.ToInt32(result[2]), (SupplyActionType)Convert.ToInt32(result[0]));
                }).ToArray());

            await Task.CompletedTask;
        }

        private static async Task ConfirmParty(this Main form)
        {
            Client.Party = new Party(
                Convert.ToInt32(form.TxtPartyHealPercent.Text),
                (HealSkillType)form.CmbPartyHeal.SelectedIndex,
                (BufferSkillType)form.CmbPartyBuff.SelectedIndex,
                (DefenceSkillType)form.CmbPartyAcc.SelectedIndex,
                (RestoreSkillType)form.CmbPartyRestore.SelectedIndex,
                (ResistanceSkillType)form.CmbPartyResistance.SelectedIndex,
                form.ChkPartyEternity.Checked,
                form.ChkAutoCure.Checked,
                form.ChkAutoDisease.Checked,
                form.ChkPartyGroupHeal.Checked,
                Convert.ToInt32(form.TxtPartyGroupHealUserLimit.Text),
                Convert.ToInt32(form.TxtPartyGroupHealPercent.Text),
                form.ChkPartyAccept.Checked,
                form.LstPartyFriends.Items.Cast<string>().ToArray());

            await Task.CompletedTask;
        }
    }
}
