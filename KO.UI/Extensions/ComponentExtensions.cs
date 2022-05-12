using KO.Application;
using KO.Application.Characters.Extensions;
using KO.Core.Extensions;
using KO.Domain.Boxes;
using KO.Domain.Characters;
using KO.Domain.Packets;
using KO.Domain.Quests;
using KO.Domain.Supplies;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KO.UI.Extensions
{
    public static class ComponentExtensions
    {
        public static void InvokeControl(this Control control, Action action)
        {
            try
            {
                if (control.InvokeRequired)
                    control.Invoke(new MethodInvoker(delegate { action(); }));
                else
                    action();
            }
            catch (Exception) { }
        }

        public static void AddItem(this ListView listView, string[] values, bool isMultiple = false)
        {
            var itemIdList = listView.Items.Cast<ListViewItem>().Select(x => x.Text).ToList();

            if (isMultiple || !itemIdList.Contains(values.FirstOrDefault()))
                listView.Items.Add(new ListViewItem(values));
        }

        public static async Task ProgramActivePassive(this Main form)
        {
            var activeColor = Color.FromArgb(255, 255, 255);
            var passiveColor = Color.FromArgb(224, 224, 224);

            Client.ProgramIsActive = form.BtnActivePassive.Text == "Active";
            form.BtnActivePassive.Text = Client.ProgramIsActive ? "Passive" : "Active";
            form.BtnActivePassive.BackColor = Client.ProgramIsActive ? activeColor : passiveColor;

            await Task.CompletedTask;
        }

        public static async Task AttackActivePassive(this Main form)
        {
            var activeColor = Color.FromArgb(255, 255, 255);
            var passiveColor = Color.FromArgb(224, 224, 224);

            Client.AttackIsActive = form.BtnStartStopAttack.Text == "Start Attack";
            form.BtnStartStopAttack.Text = Client.AttackIsActive ? "Stop Attack" : "Start Attack";
            form.BtnStartStopAttack.BackColor = Client.AttackIsActive ? activeColor : passiveColor;

            await Task.CompletedTask;
        }

        public static async Task CollectGates(this Main form)
        {
            form.CmbGates.Items.Clear();
            var gateTypes = GateType.DelosAbyssDungeon.List()
                .Where(x => x.ShortName.Contains(Client.Main.GetCharacterRaceType().ToString()));

            foreach (var item in gateTypes)
                form.CmbGates.Items.Add(item.DisplayName);

            form.CmbGates.SelectedIndex = 0;

            await Task.CompletedTask;
        }

        public static async Task CollectSupplyMethods(this Main form)
        {
            form.CmbSupplyAction.Items.Clear();
            var supplyActionTypes = SupplyActionType.Coordinate.List()
                .Where(x => string.IsNullOrEmpty(x.Group) || x.Group == Client.Main.GetCharacterZone());

            foreach (var item in supplyActionTypes)
                form.CmbSupplyAction.Items.Add($"{item.Value}-{item.DisplayName}");
            form.CmbSupplyAction.SelectedIndex = 0;

            await Task.CompletedTask;
        }

        public static async Task AddSupplyMethod(this Main form)
        {
            var actionType = (SupplyActionType)form.CmbSupplyAction.SelectedIndex;
            var actionTypeData = actionType.Get();

            form.LstSupplyAction.Items.Add($"CODE:{actionTypeData.Value}:{Client.Main.GetCharacterX()}:{Client.Main.GetCharacterY()}-{actionTypeData.DisplayName}");

            await Task.CompletedTask;
        }

        public static async Task CollectTransformationScroll(this Main form)
        {
            form.CmbTransformationScroll.Items.Clear();

            foreach (var item in TransformationType.OrcBowman.List())
                form.CmbTransformationScroll.Items.Add(item.DisplayName);

            form.CmbTransformationScroll.SelectedIndex = 0;

            await Task.CompletedTask;
        }

        public static async Task CollectFollowList(this Main form)
        {
            form.CmbFollow.Items.Clear();

            foreach (var item in FollowType.MainCharacter.List())
                form.CmbFollow.Items.Add(item.DisplayName);

            form.CmbFollow.SelectedIndex = 0;

            await Task.CompletedTask;
        }

        public static async Task CollectLooterList(this Main form)
        {
            form.CmbLooter.Items.Clear();
            form.CmbLooter.Items.Add("Personal Collect");

            foreach (var item in Client.Characters)
                form.CmbLooter.Items.Add(item.GameName);

            form.CmbLooter.SelectedIndex = 0;

            await Task.CompletedTask;
        }

        public static async Task CollectBoxCollectorList(this Main form)
        {
            form.TvCollectList.Nodes.Clear();

            form.TvCollectList.Nodes.Add("Active");
            form.TvCollectList.Nodes.Add("Money");
            var lootTypes = BoxCollectType.TypeUnique.List().GroupBy(x => x.Group).ToList();

            foreach (var root in new[] { "Collect", "Do Not Collect" })
            {
                var node = new TreeNode(root);
                foreach (var group in lootTypes)
                {
                    var subNode = new TreeNode(group.Key);
                    foreach (var type in group.ToList())
                        subNode.Nodes.Add(type.DisplayName);
                    node.Nodes.Add(subNode);
                }
                form.TvCollectList.Nodes.Add(node);
            }

            await Task.CompletedTask;
        }
    }
}
