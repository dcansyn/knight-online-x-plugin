using KO.Application;
using KO.Application.Addresses.Handlers;
using KO.Application.Characters.Extensions;
using KO.Application.Features.Handlers;
using KO.Application.Items.Extensions;
using KO.Application.Parties.Handlers;
using KO.Application.Supplies.Handlers;
using KO.Application.Targets.Extensions;
using KO.Core.Constants;
using KO.Core.Extensions;
using KO.Core.Helpers.Memory;
using KO.Core.Helpers.Message;
using KO.Core.Helpers.Utility;
using KO.Domain.Characters;
using KO.Domain.Items;
using KO.Domain.Packets;
using KO.Domain.Supplies;
using KO.UI.Extensions;
using KO.UI.Helpers;
using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KO.UI
{
    public partial class Main : Form
    {
        private readonly ClientHandler _clientHandler = new ClientHandler();
        public Main() { InitializeComponent(); Name = Settings.Name; NotifyIcon.Text = Settings.Name; Size = new Size(595, 712); }

        #region [Main]
        private async void Main_Load(object sender, EventArgs e)
        {
            await Task.WhenAll(
                this.CollectFollowList(),
                this.CollectLooterList(),
                this.CollectSupplyMethods(),
                this.CollectBoxCollectorList(),
                this.CollectTransformationScroll(),
                this.CollectGates()
                );

            await GameHelper.InjectGame();

            if (!this.ReadSettings())
                MessageHelper.Send("Read settings could not be completed successfully.");

            MenuConfirmChanges_Click(sender, e);

            _clientHandler.Action();
        }

        private async void BtnActivePassive_Click(object sender, EventArgs e)
        {
            MenuConfirmChanges_Click(sender, e);

            await this.ProgramActivePassive();

            if (Client.AttackIsActive && !Client.ProgramIsActive)
                await this.AttackActivePassive();
        }

        private async void BtnStartStopAttack_Click(object sender, EventArgs e)
        {
            MenuConfirmChanges_Click(sender, e);

            await this.AttackActivePassive();

            if (!Client.ProgramIsActive)
                await this.ProgramActivePassive();
        }
        #endregion

        #region [Utility Events]
        private void MenuTopMost_Click(object sender, EventArgs e)
        {
            MenuTopMost.Checked = !MenuTopMost.Checked;
        }

        private void MenuTopMost_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = MenuTopMost.Checked;
        }

        private void MenuHide_Click(object sender, EventArgs e)
        {
            NotifyIcon.Text = Settings.Name;
            NotifyIcon.Visible = true;
            NotifyIcon.ShowBalloonTip(0, Settings.Name, $"{Settings.Name} is still running.", ToolTipIcon.Info);
            Visible = false;
        }

        private void MenuSave_Click(object sender, EventArgs e)
        {
            if (!this.SaveSettings())
                MessageHelper.Send("Saving settings could not be completed successfully.");
        }

        private void MenuClose_Click(object sender, EventArgs e)
        {
            if (MessageHelper.Send("Are you sure to close?", MessageBoxButtons.YesNo))
                System.Windows.Forms.Application.Exit();
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            NotifyIcon.Visible = false;
            Visible = true;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            NotifyIcon.Visible = false;
            System.Windows.Forms.Application.Exit();
        }

        private async void MenuConfirmChanges_Click(object sender, EventArgs e)
        {
            try
            {
                await this.ConfirmChanges();

                MenuSave_Click(sender, e);
            }
            catch
            {
                MessageHelper.Send("Settings aren't saved, maybe your input types can be a wrong type or empty.");
            }
        }

        private void MenuLogger_Click(object sender, EventArgs e)
        {
            new Logger().Show();
        }

        private async void InjectAgainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var confirm = MessageHelper.Send("Are you sure to inject again?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm)
            {
                await GameHelper.InjectGame();
                MessageHelper.Send("Completed!");
            }
        }

        private void MenuCloseAll_Click(object sender, EventArgs e)
        {
            var confirm = MessageHelper.Send("Are you sure to close all game?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm)
                foreach (var item in Client.Characters)
                    MemoryHelper.Kill(item.GameName, false);
        }
        #endregion

        #region [Scroll]
        private async void ChkWallHack_CheckedChanged(object sender, EventArgs e)
        {
            await FeatureHandler.DisableWall(ChkWallHack.Checked);
        }
        #endregion

        #region [Box Collect]
        private async void ChkOreads_CheckedChanged(object sender, EventArgs e)
        {
            await FeatureHandler.Oreads(ChkOreads.Checked);
        }

        private async void BtnBoxCollectAddItemFirstRow_Click(object sender, EventArgs e)
        {
            var item = await Client.Main.GetDataItemByType(ItemInventoryType.Row1);
            LvBoxCollectItems.AddItem(new[] { item.ItemId.ToString(), item.GetTitleWithGrade() });
        }

        private async void BtnBoxCollectAddAllItems_Click(object sender, EventArgs e)
        {
            var items = await Client.Main.GetDataItems();
            foreach (var item in items)
                LvBoxCollectItems.AddItem(new[] { item.ItemId.ToString(), item.GetTitleWithGrade() });
        }

        private void LvBoxCollectItems_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (LvBoxCollectItems.FocusedItem != null)
                LvBoxCollectItems.FocusedItem.Remove();
        }

        private void BtnBoxCollectItemsClear_Click(object sender, EventArgs e)
        {
            if (MessageHelper.Send("Are you sure you want to clear all collect items?", MessageBoxButtons.YesNo))
                LvBoxCollectItems.Items.Clear();
        }
        #endregion

        #region [Target]
        private void ChkSelectAllWarrior_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in LvAttackWarrior.Items)
                item.Checked = ChkSelectAllWarrior.Checked;
        }

        private void ChkSelectAllRogue_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in LvAttackRogue.Items)
                item.Checked = ChkSelectAllRogue.Checked;
        }

        private void ChkSelectAllMagician_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in LvAttackMagician.Items)
                item.Checked = ChkSelectAllMagician.Checked;
        }

        private void ChkSelectAllPriest_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in LvAttackPriest.Items)
                item.Checked = ChkSelectAllPriest.Checked;
        }

        private void BtnTargetListAddAround_Click(object sender, EventArgs e)
        {
            var targets = Client.Main.GetAllTarget(CharacterType.NonPlayerCharacter);

            foreach (var item in targets)
                if (!LstTargetList.Items.Cast<string>().Contains(item.Name))
                    LstTargetList.Items.Add(item.Name);
        }

        private async void BtnTargetListAddSelect_Click(object sender, EventArgs e)
        {
            var target = await Client.Main.GetTarget();
            if (target?.Id > 0 && !LstTargetList.Items.Cast<string>().Contains(target.Name))
                LstTargetList.Items.Add(target.Name);
        }

        private void BtnTargetListClear_Click(object sender, EventArgs e)
        {
            if (MessageHelper.Send("Are you sure you want to clear all target list?", MessageBoxButtons.YesNo))
                LstTargetList.Items.Clear();
        }

        private void LstTargetList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (LstTargetList.SelectedIndex > -1)
                LstTargetList.Items.RemoveAt(LstTargetList.SelectedIndex);
        }

        private void BtnRunCoordinateAddCoordinate_Click(object sender, EventArgs e)
        {
            LstRunCoordinate.Items.Add($"{Client.Main.GetCharacterX()}-{Client.Main.GetCharacterY()}");
        }

        private void BtnRunCoordinateClear_Click(object sender, EventArgs e)
        {
            if (MessageHelper.Send("Are you sure you want to clear all coordinates?", MessageBoxButtons.YesNo))
                LstRunCoordinate.Items.Clear();
        }

        private void LstRunCoordinate_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (LstRunCoordinate.SelectedIndex > -1)
                LstRunCoordinate.Items.RemoveAt(LstRunCoordinate.SelectedIndex);
        }

        private void TmrReader_Tick(object sender, EventArgs e)
        {
            if (!Client.ProgramIsActive) return;

            var targetId = Client.Main.GetTargetId();
            if (targetId <= 0) return;

            TxtTargetId.Text = Client.Main.GetTargetHexId();
            TxtTargetX.Text = Client.Main.GetTargetX().ToString();
            TxtTargetY.Text = Client.Main.GetTargetY().ToString();
            TxtTargetZ.Text = Client.Main.GetTargetZ().ToString();
            TxtTargetDistance.Text = DistanceHelper.GetDistance(Client.Main.GetCharacterX(), Client.Main.GetCharacterY(), Client.Main.GetTargetX(), Client.Main.GetTargetY()).ToString();
        }
        #endregion

        #region [Supply]
        private async void BtnSupplyActionAddMethod_Click(object sender, EventArgs e)
        {
            await this.AddSupplyMethod();
        }

        private void LstSupplyAction_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (LstSupplyAction.SelectedIndex > -1)
                LstSupplyAction.Items.RemoveAt(LstSupplyAction.SelectedIndex);
        }

        private void BtnSupplyFunctionClear_Click(object sender, EventArgs e)
        {
            if (MessageHelper.Send("Are you sure you want to clear all methods?", MessageBoxButtons.YesNo))
                LstSupplyAction.Items.Clear();
        }

        private async void ChkFillToBankPotion_CheckedChanged(object sender, EventArgs e)
        {
            await SupplyActionHandler.ResetBankFillActioRow();
        }
        #endregion

        #region [Party]
        private async void BtnAddSelectPartyFriend_Click(object sender, EventArgs e)
        {
            var target = await Client.Main.GetTarget();
            if (target?.Id > 0 && !LstPartyFriends.Items.Cast<string>().Contains(target.Name) && target.Type == CharacterType.Player)
                LstPartyFriends.Items.Add(target.Name);
        }

        private void BtnAddAllPartyFriends_Click(object sender, EventArgs e)
        {
            foreach (var item in Client.PartyCharacters)
                if (!LstPartyFriends.Items.Cast<string>().Contains(item.Name))
                    LstPartyFriends.Items.Add(item.Name);
        }

        private void BtnPartyFriendsClear_Click(object sender, EventArgs e)
        {
            if (MessageHelper.Send("Are you sure you want to clear all friend list?", MessageBoxButtons.YesNo))
                LstPartyFriends.Items.Clear();
        }

        private void LstPartyFriends_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (LstPartyFriends.SelectedIndex > -1)
                LstPartyFriends.Items.RemoveAt(LstPartyFriends.SelectedIndex);
        }

        private void BtnAddClientPartyFriends_Click(object sender, EventArgs e)
        {
            foreach (var item in Client.Characters)
                if (!LstPartyFriends.Items.Cast<string>().Contains(item.Name) && item.Name != null)
                    LstPartyFriends.Items.Add(item.Name);
        }

        private async void BtnRefreshBuffer_Click(object sender, EventArgs e)
        {
            await PartyHandler.RefreshBuffer();
        }
        #endregion

        #region [Quest]
        private async void BtnSearch_Click(object sender, EventArgs e)
        {
            await this.GetAllQuest(TxtQuestSeach.Text);
        }

        private async void LvQuests_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                await this.RemoveQuest();
            }
            else
            {
                await this.TakeQuest();
            }
        }

        private async void BtnQuestTake_Click(object sender, EventArgs e)
        {
            await this.TakeQuest();
        }

        private async void BtnQuestRemove_Click(object sender, EventArgs e)
        {
            await this.RemoveQuest();
        }

        private async void LvQuests_MouseClick(object sender, MouseEventArgs e)
        {
            await this.GetQuestDetail();
        }

        private async void ChkQuestFilterVisible_CheckedChanged(object sender, EventArgs e)
        {
            ChkLstQuestFilter.Visible = ChkQuestFilterVisible.Checked;
            if (!ChkLstQuestFilter.Visible)
                await this.GetAllQuest();
        }

        private async void BtnQuestTakeReward_Click(object sender, EventArgs e)
        {
            await this.RewardQuest();
        }

        private async void BtnQuestTeleport_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(TxtQuestTeleportX.Text, out int x) || !int.TryParse(TxtQuestTeleportY.Text, out int y))
                return;

            await FeatureHandler.Teleport(x, y);
        }
        #endregion

        #region [Action]
        private async void BtnTown_Click(object sender, EventArgs e)
        {
            await FeatureHandler.Town();
        }

        private async void BtnActionCoordinateTeleport_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(TxtActionCoordinateX.Text, out int x) || !int.TryParse(TxtActionCoordinateY.Text, out int y))
                return;

            await FeatureHandler.Teleport(x, y);
        }

        private void BtnActionSupplySundries_Click(object sender, EventArgs e)
        {
            MenuConfirmChanges_Click(sender, e);

            Parallel.ForEach(Client.Characters, (character) =>
            {
                new Thread(async () => { await character.SupplySundires(); }) { IsBackground = true }.Start();
            });
        }

        private void BtnActionSupplyPotion_Click(object sender, EventArgs e)
        {
            MenuConfirmChanges_Click(sender, e);

            Parallel.ForEach(Client.Characters, (character) =>
            {
                new Thread(async () => { await character.SupplyPotion(); }) { IsBackground = true }.Start();
            });
        }

        private void BtnActionSupplyBank_Click(object sender, EventArgs e)
        {
            MenuConfirmChanges_Click(sender, e);

            Parallel.ForEach(Client.Characters, (character) =>
            {
                new Thread(async () => { await character.SupplyBank(); }) { IsBackground = true }.Start();
            });
        }

        private async void BtnBuyHighScroll_Click(object sender, EventArgs e)
        {
            try
            {
                await Client.Main.SupplyItem(new SupplyItem[] { new SupplyItem(379021000, 10, 0, 19, false) }, SupplyNpcType.ScrollBuy);
            }
            catch (Exception ex)
            {
                MessageHelper.Send(ex);
            }
        }

        private async void BtnBuyBlessedScroll_Click(object sender, EventArgs e)
        {
            try
            {
                await Client.Main.SupplyItem(new SupplyItem[] { new SupplyItem(379025000, 10, 0, 20, false) }, SupplyNpcType.ScrollBuy);
            }
            catch (Exception ex)
            {
                MessageHelper.Send(ex);
            }
        }

        private async void BtnFeatureMaradonBuff_Click(object sender, EventArgs e)
        {
            await FeatureHandler.MoradonBuff();
        }

        private async void BtnActionShortcutGetAllMS_Click(object sender, EventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                await FeatureHandler.GoMonsterStoneArea();
            }
            else
            {
                await FeatureHandler.GetMonsterStones();
            }
        }

        private async void BtnAllUpgrade_Click(object sender, EventArgs e)
        {
            await FeatureHandler.UpgradeAll();
        }

        private async void BtnGate_Click(object sender, EventArgs e)
        {
            var gate = GateType.DelosAbyssDungeon.List()
                   .FirstOrDefault(x => x.DisplayName == CmbGates.Text && x.ShortName.Contains(Client.Main.GetCharacterRaceType().ToString()));

            if (gate != null)
                foreach (var character in Client.Characters)
                    await character.Send("4B", Convert.ToInt32(gate.Description).ConvertToDword(2), Convert.ToInt32(gate.Prompt).ConvertToDword(2));
        }

        private async void BtnGoCoordinate_Click(object sender, EventArgs e)
        {
            if (LstCoordinates.SelectedIndex < 0) return;

            var coors = LstCoordinates.Text.Split('-').Select(x => Convert.ToInt32(x)).ToArray();
            await FeatureHandler.Teleport(coors[0], coors[1]);

            if (LstCoordinates.SelectedIndex < LstCoordinates.Items.Count - 1)
                LstCoordinates.SelectedIndex++;
            else
                LstCoordinates.SelectedIndex = 0;
        }

        private void BtnAddCoordinate_Click(object sender, EventArgs e)
        {
            LstCoordinates.Items.Add($"{Client.Main.GetCharacterX()}-{Client.Main.GetCharacterY()}");
        }

        private void BtnExportCoor_Click(object sender, EventArgs e)
        {
            LstRunCoordinate.Items.Clear();

            foreach (var item in LstCoordinates.Items)
                LstRunCoordinate.Items.Add(item.ToString());
        }

        private void LstCoordinates_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (LstCoordinates.SelectedIndex > -1)
                LstCoordinates.Items.RemoveAt(LstCoordinates.SelectedIndex);
        }

        private void BtnClearCoordinates_Click(object sender, EventArgs e)
        {
            if (MessageHelper.Send("Are you sure you want to clear all coordinates?", MessageBoxButtons.YesNo))
                LstCoordinates.Items.Clear();
        }

        private async void BtnCreateParty_Click(object sender, EventArgs e)
        {
            foreach (var item in LstPartyFriends.Items)
                await Client.Main.SendPartyInvite(item.ToString());
        }
        #endregion

        private void BtnTest_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
