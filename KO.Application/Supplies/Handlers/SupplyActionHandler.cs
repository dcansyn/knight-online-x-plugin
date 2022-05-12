using KO.Application.Addresses.Handlers;
using KO.Application.Characters.Extensions;
using KO.Application.Characters.Handlers;
using KO.Application.Supplies.Extensions;
using KO.Application.Targets.Extensions;
using KO.Domain.Characters;
using KO.Domain.Supplies;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KO.Application.Supplies.Handlers
{
    public static class SupplyActionHandler
    {
        private static Task SupplyRepairTask = Task.CompletedTask;
        private static Task BankFillTask = Task.CompletedTask;

        public static async Task SupplyAction(this Character character)
        {
            if (SupplyRepairTask.IsCompleted)
                SupplyRepairTask = character.SupplyRepairAction();

            if (BankFillTask.IsCompleted)
                BankFillTask = character.BankFillAction();

            await Task.CompletedTask;
        }

        public static async Task BankFillAction(this Character character)
        {
            if (character.GetCharacterZone() != "Moradon" || !Client.Supply.FillToBankWithPotion || DateTime.Now < character.BankFillExpireTime) return;

            switch (character.BankFillActionRow)
            {
                case 0:
                    if (character.GetTargetDistance(377, 571) > 1)
                    {
                        await character.Walk(377, 571, CharacterWalkType.Teleport);
                        character.UpdateBankFillExpireTime(2000);
                        return;
                    }

                    character.UpdateBankFillActionReset();
                    character.UpdateBankFillActionRowIncrease();
                    break;

                case 1:
                    if (character.SupplyPotionThread != null) return;

                    character.SupplyPotionThread = new Thread(async () =>
                    {
                        Thread.Sleep(500);

                        await character.SupplyPotion();
                        character.UpdateBankFillActionRowIncrease();
                        character.UpdateBankFillExpireTime(2000);
                    });

                    if (character.SupplyPotionThread == null || character.SupplyPotionThread.IsAlive == false)
                        character.SupplyPotionThread.Start();
                    break;

                case 2:
                    if (character.GetTargetDistance(408, 555) > 1)
                    {
                        await character.Walk(408, 555, CharacterWalkType.Teleport);
                        character.UpdateBankFillExpireTime(2000);
                        return;
                    }

                    character.UpdateBankFillActionRowIncrease();
                    break;

                case 3:
                    if (character.SupplyBankThread != null) return;

                    character.SupplyBankThread = new Thread(async () =>
                    {
                        Thread.Sleep(500);

                        await character.SupplySendBankItems();
                        character.UpdateBankFillActionRow(0);
                        character.UpdateBankFillExpireTime(2000);
                    });

                    if (character.SupplyBankThread == null || character.SupplyBankThread.IsAlive == false)
                        character.SupplyBankThread.Start();
                    break;


            }
        }

        public static async Task ResetBankFillActioRow()
        {
            Parallel.ForEach(Client.Characters, (character) =>
            {
                character.UpdateBankFillActionRow(0);
            });

            await Task.CompletedTask;
        }

        public static async Task SupplyRepairAction(this Character character)
        {
            if (!Client.Supply.IsActive) return;
            if (!Client.Supply.Actions.Any()) return;

            if (!character.IsSupplyAction && character.SupplyIsStart())
                character.UpdateSupplyAction(true);

            if (!character.IsSupplyAction || DateTime.Now < character.SupplyExpireTime) return;

            var action = Client.Supply.Actions.ElementAt(character.SupplyActionRow);
            if (action == null) return;

            if (character.GetTargetDistance(action.X, action.Y) > 1 && action.Type != SupplyActionType.Town)
            {
                await character.Walk(action.X, action.Y, Client.Supply.WalkType);
                character.UpdateSupplyExpireTime();
                return;
            }

            switch (action.Type)
            {
                case SupplyActionType.Coordinate:
                    if (character.GetTargetDistance(action.X, action.Y) < 2)
                    {
                        character.UpdateSupplyActionRow();
                        character.UpdateSupplyExpireTime();
                    }
                    break;

                case SupplyActionType.Town:
                    await character.Send("4800");
                    character.UpdateSupplyActionRow();
                    character.UpdateSupplyExpireTime(3000);
                    break;

                case SupplyActionType.Sundires:
                    if (character.SupplySundiresThread != null) return;

                    character.SupplySundiresThread = new Thread(async () =>
                    {
                        Thread.Sleep(1000);

                        await character.SupplySundires();
                        character.UpdateSupplyActionRow();
                        character.UpdateSupplyExpireTime(2000);
                    });

                    if (character.SupplySundiresThread == null || character.SupplySundiresThread.IsAlive == false)
                        character.SupplySundiresThread.Start();
                    break;

                case SupplyActionType.Potion:
                    if (character.SupplyPotionThread != null) return;

                    character.SupplyPotionThread = new Thread(async () =>
                    {
                        Thread.Sleep(1000);

                        await character.SupplyPotion();
                        character.UpdateSupplyActionRow();
                        character.UpdateSupplyExpireTime(2000);
                    });

                    if (character.SupplyPotionThread == null || character.SupplyPotionThread.IsAlive == false)
                        character.SupplyPotionThread.Start();
                    break;

                case SupplyActionType.Completed:
                    character.UpdateSupplyActionComplete();
                    break;

                default:
                    break;
            }
        }
    }
}
