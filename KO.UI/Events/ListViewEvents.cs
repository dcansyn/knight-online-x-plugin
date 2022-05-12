using KO.Core.Helpers.Message;
using KO.UI.Helpers;
using Microsoft.VisualBasic;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace KO.UI.Events
{
    public static class ListViewEvents
    {
        public static void MouseDoubleClickDelete(this ListView listView)
        {
            var focusedItem = listView.FocusedItem;
            if (focusedItem != null && MessageHelper.Send("Are you sure to delete?", MessageBoxButtons.YesNo))
                focusedItem.Remove();
        }

        public static void LvAccountsMouseClick(this ListView listView, MouseEventArgs e)
        {
            var focusedItem = listView.FocusedItem;

            var contextMenu = new ContextMenu();

            var changeDetail = new MenuItem("Change Detail");
            changeDetail.Click += delegate (object menuItemSender, EventArgs menuItemEvent)
            {
                var result = Interaction.InputBox("Account Detail", $"Please enter the account detail.", focusedItem.SubItems[0].Text);
                if (string.IsNullOrEmpty(result)) return;

                focusedItem.SubItems[0].Text = result;
            };

            var changeName = new MenuItem("Change Name");
            changeName.Click += delegate (object menuItemSender, EventArgs menuItemEvent)
            {
                var result = Interaction.InputBox("Account Name", $"Please enter the account name.", focusedItem.SubItems[1].Text);
                if (string.IsNullOrEmpty(result)) return;

                focusedItem.SubItems[1].Text = result;
            };

            var changePath = new MenuItem("Change Path");
            changePath.Click += delegate (object menuItemSender, EventArgs menuItemEvent)
            {
                var path = GameHelper.GetGameFilePath();

                if (string.IsNullOrEmpty(path))
                    return;

                focusedItem.SubItems[4].Text = path;
            };

            contextMenu.MenuItems.AddRange(new[] { changeDetail, changeName, changePath });
            contextMenu.Show(listView, new Point(e.X, e.Y));
        }

        public static void LvGamesMouseClick(this ListView listView, MouseEventArgs e)
        {
            var focusedItem = listView.FocusedItem;

            var contextMenu = new ContextMenu();

            var contextMenuItemSetMain = new MenuItem("Set Main");
            contextMenuItemSetMain.Click += delegate (object menuItemSender, EventArgs menuItemEvent)
            {
                for (int i = 0; i < listView.Items.Count; i++)
                    listView.Items[i].SubItems[4].Text = "NO";

                focusedItem.SubItems[4].Text = "YES";
            };

            contextMenu.MenuItems.AddRange(new[] { contextMenuItemSetMain });
            contextMenu.Show(listView, new Point(e.X, e.Y));
        }
    }
}
