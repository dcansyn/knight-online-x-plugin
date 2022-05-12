using KO.Application;
using KO.Core.Constants;
using KO.Core.Extensions;
using KO.Core.Handlers;
using KO.Core.Helpers.Message;
using KO.Core.Helpers.Storage;
using KO.Domain.Accounts;
using KO.Domain.Characters;
using KO.Infrastructure.Services.Tables;
using KO.UI.Events;
using KO.UI.Extensions;
using KO.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace KO.UI
{
    public partial class Login : Form
    {
        private Account _account;
        private Thread _openThread;
        public Login() { InitializeComponent(); Size = new Size(815, 500); }

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

        private void MenuTableEditor_Click(object sender, EventArgs e)
        {
            new Editor().Show();
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            NotifyIcon.Visible = false;
            System.Windows.Forms.Application.Exit();
        }
        #endregion

        private async void Login_Load(object sender, EventArgs e)
        {
            if (!StorageHelper.ExistsData())
            {
                MessageHelper.Send("Settings data not found, please choose game for data creation.", "An error occured!", icon: MessageBoxIcon.Error);

                var gamePath = TableHelper.GetDataPath();
                if (gamePath == null)
                {
                    MessageHelper.Send("Data folder not found.", "An error occured!", icon: MessageBoxIcon.Error);
                    System.Windows.Forms.Application.Exit();
                    return;
                }

                MessageHelper.Send("Data creation started. Please wait..");

                var _itemTable = new ItemTableService(gamePath);
                var _skillTable = new SkillTableService(gamePath);
                var _questTable = new QuestTableService(gamePath);
                var _nonPlayerCharacterTable = new NonPlayerCharacterService(gamePath);

                await _itemTable.SaveItems();
                await _itemTable.SaveItemExtensions();
                await _skillTable.SaveSkills();
                await _skillTable.SaveSkillExtensions();
                await _nonPlayerCharacterTable.SaveNonPlayerCharacters();
                await _nonPlayerCharacterTable.SaveNonPlayerCharacterMaps();
                await _questTable.SaveQuests();
                await _questTable.SaveQuestGuides();
                await _questTable.SaveQuestNonPlayerCharacterDescriptions();
                await _questTable.SaveQuestNonPlayerCharacterExchanges();
                await _questTable.SaveQuestItemExchanges();
            }

            if (!this.ReadSettings())
                MessageHelper.Send("Read settings could not be completed successfully.");

            BtnRefreshGame_Click(sender, e);
        }

        private void LvAccounts_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            LvAccounts.MouseDoubleClickDelete();
            TxtAccountDetail.Text = "";
        }

        private void LvAccounts_MouseClick(object sender, MouseEventArgs e)
        {
            var focusedItem = LvAccounts.FocusedItem;

            if (e.Button == MouseButtons.Left)
                TxtAccountDetail.Text = focusedItem != null ? focusedItem.SubItems[0].Text : "";

            if (e.Button == MouseButtons.Right && focusedItem != null && focusedItem.Bounds.Contains(e.Location))
                LvAccounts.LvAccountsMouseClick(e);
        }

        private void LvGames_MouseClick(object sender, MouseEventArgs e)
        {
            var focusedItem = LvGames.FocusedItem;

            if (e.Button == MouseButtons.Right && focusedItem != null && focusedItem.Bounds.Contains(e.Location))
                LvGames.LvGamesMouseClick(e);
        }

        private void LvGames_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
                LvGames.MouseDoubleClickDelete();
        }

        private void BtnAddAccount_Click(object sender, EventArgs e)
        {
            _account = Helpers.AccountHelper.GetAccountInformation();

            if (_account == null)
                return;

            LvAccounts.Items.Add(new ListViewItem(new[]
            {
                _account.Detail,
                _account.GameName,
                _account.UserName,
                _account.UserPassword,
                _account.GamePath
            }));
        }

        private void BtnOpenAccount_Click(object sender, EventArgs e)
        {
            if (LvAccounts.Items.Count == 1)
                LvAccounts.Items[0].Focused = true;

            var focusedItem = LvAccounts.FocusedItem;

            if (focusedItem == null)
            {
                MessageHelper.Send("Please select the game you want to open.");
                return;
            }

            if (_openThread?.IsAlive == true)
            {
                MessageHelper.Send("Please wait for open the game process.");
                return;
            }

            _openThread = ThreadHandler.Start(async () =>
            {
                var gameName = focusedItem.SubItems[1].Text;
                var userName = focusedItem.SubItems[2].Text;
                var userPassword = focusedItem.SubItems[3].Text;
                var gamePath = focusedItem.SubItems[4].Text;

                var account = new Account(gameName, gamePath, userName, userPassword);

                var result = await Application.Accounts.Helpers.AccountHelper.Open(account.GamePath, account.GameName, account.Arguments);
                if (!result)
                {
                    MessageHelper.Send("Please check KO.Processor.exe");
                    return;
                }
            });
        }

        private async void BtnRefreshGame_Click(object sender, EventArgs e)
        {
            LvGames.Items.Clear();

            var games = await Application.Accounts.Helpers.AccountHelper.GetAll();
            for (int i = 0; i < games.Length; i++)
            {
                var game = games[i];

                LvGames.Items.Add(new ListViewItem(new[]
                {
                    game.GameName,
                    game.Name,
                    game.ClassNameType.Get().DisplayName,
                    game.Level.ToString(),
                    i == 0 ? "YES" : "NO"
                }));
            }
        }

        private void BtnLoadGame_Click(object sender, EventArgs e)
        {
            if (LvGames.Items.Count == 1)
                LvGames.Items[0].Checked = true;

            var characters = new List<Character>();
            foreach (ListViewItem item in LvGames.Items)
                if (item.Checked)
                    characters.Add(new Character(item.Text, item.SubItems[4].Text == "YES"));

            Client.Characters = characters.OrderBy(x => x.IsMain).ToArray();
            if (Client.Main == null)
            {
                MessageHelper.Send("Main not found! Use right click for select main.");
                return;
            }

            var main = new Main();
            main.Show();
            Hide();
        }

        private void ChkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in LvGames.Items)
                item.Checked = ChkSelectAll.Checked;
        }
    }
}
