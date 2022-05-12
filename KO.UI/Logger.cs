using KO.Application;
using KO.Application.Addresses.Handlers;
using KO.Application.Characters.Extensions;
using KO.Application.Targets.Extensions;
using KO.Core.Extensions;
using KO.Core.Handlers;
using KO.Domain.Packets;
using KO.UI.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KO.UI
{
    public partial class Logger : Form
    {
        private Task ReceiveTask = Task.CompletedTask;
        private Task SendTask = Task.CompletedTask;
        public Logger() { InitializeComponent(); }
        private void Logger_Load(object sender, EventArgs e)
        {
            var packets = PacketType.Unknown.List().OrderBy(x => x.DisplayName).ToArray();
            foreach (var item in packets)
                ChkLstPackets.Items.Add(item.DisplayName);

            Action();
        }

        public Thread Action()
        {
            return ThreadHandler.Start(() =>
            {
                do
                {
                    TxtCharacterId.InvokeControl(() => { TxtCharacterId.Text = Client.Main.GetCharacterId().ConvertToDword(2); });
                    TxtCharacterX.InvokeControl(() => { TxtCharacterX.Text = Client.Main.GetCharacterX().ConvertToDword(2); });
                    TxtCharacterY.InvokeControl(() => { TxtCharacterY.Text = Client.Main.GetCharacterY().ConvertToDword(2); });
                    TxtCharacterZ.InvokeControl(() => { TxtCharacterZ.Text = Client.Main.GetCharacterZ().ConvertToDword(2); });
                    TxtTargetId.InvokeControl(() => { TxtTargetId.Text = Client.Main.GetTargetHexId(); });
                    TxtTargetX.InvokeControl(() => { TxtTargetX.Text = Client.Main.GetTargetX().ConvertToDword(2); });
                    TxtTargetY.InvokeControl(() => { TxtTargetY.Text = Client.Main.GetTargetY().ConvertToDword(2); });
                    TxtTargetZ.InvokeControl(() => { TxtTargetZ.Text = Client.Main.GetTargetZ().ConvertToDword(2); });

                    if (ReceiveTask.IsCompleted)
                        ReceiveTask = ReceiveAction();

                    if (SendTask.IsCompleted)
                        SendTask = SendAction();

                } while (true);
            });
        }

        private async Task ReceiveAction()
        {
            if (!ChkReadReceive.Checked) return;

            await Client.Main.ReadReceiveMessageAction(async (Packet packet) =>
            {
                var filter = new List<string>();
                ChkLstPackets.InvokeControl(() =>
                {
                    for (int i = 0; i < ChkLstPackets.Items.Count; i++)
                        if (ChkLstPackets.GetItemChecked(i))
                            filter.Add(ChkLstPackets.Items[i].ToString());
                });
                if (!filter.Contains(packet.Name)) return;

                var item = new ListViewItem(new[]
                {
                        DateTime.Now.ToString("HH:mm:ss"),
                        $"0x{packet.Value.ConvertToDword(1)}",
                        packet.Name,
                        packet.Code
                })
                {
                    ForeColor = Color.DarkGreen
                };

                LvPackets.InvokeControl(() => { LvPackets.Items.Insert(0, item); });

                await Task.CompletedTask;
            });
        }

        private async Task SendAction()
        {
            if (!ChkReadSend.Checked) return;

            await Client.Main.ReadSendMessageAction(async (Packet packet) =>
            {
                var filter = new List<string>();
                ChkLstPackets.InvokeControl(() =>
                {
                    for (int i = 0; i < ChkLstPackets.Items.Count; i++)
                        if (ChkLstPackets.GetItemChecked(i))
                            filter.Add(ChkLstPackets.Items[i].ToString());
                });
                if (!filter.Contains(packet.Name)) return;

                var item = new ListViewItem(new[]
                {
                        DateTime.Now.ToString("HH:mm:ss"),
                        $"0x{packet.Value.ConvertToDword(1)}",
                        packet.Name,
                        packet.Code
                })
                {
                    ForeColor = Color.DarkRed
                };

                LvPackets.InvokeControl(() => { LvPackets.Items.Insert(0, item); });

                await Task.CompletedTask;
            });
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            LvPackets.Items.Clear();
        }

        private void ChkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < ChkLstPackets.Items.Count; i++)
                ChkLstPackets.SetItemChecked(i, ChkSelectAll.Checked);
        }

        private void LvPackets_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (LvPackets.FocusedItem != null)
            {
                var packet = LvPackets.FocusedItem.SubItems[3].Text.Trim();
                TxtPacket.Text = packet;
                Clipboard.SetText(packet);
            }
        }

        private void LvPackets_MouseClick(object sender, MouseEventArgs e)
        {
            if (LvPackets.FocusedItem != null)
                TxtPacket.Text += LvPackets.FocusedItem.SubItems[3].Text.Trim() + Environment.NewLine;
        }

        private async void BtnSend_Click(object sender, EventArgs e)
        {
            var packets = TxtPacket.Text?.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var packet in packets)
                await Client.Main.Send(packet);
        }

        private async void BtnSendAll_Click(object sender, EventArgs e)
        {
            var packets = TxtPacket.Text?.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in Client.Characters)
            {
                foreach (var packet in packets)
                    await item.Send(packet);
            }
        }
    }
}
