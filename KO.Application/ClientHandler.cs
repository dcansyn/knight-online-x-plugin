using KO.Application.Addresses.Handlers;
using KO.Application.Boxes.Handlers;
using KO.Application.Characters.Handlers;
using KO.Application.Features.Handlers;
using KO.Application.Parties.Handlers;
using KO.Application.Skills.Handlers;
using KO.Application.Supplies.Handlers;
using KO.Application.Targets.Handlers;
using KO.Core.Handlers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KO.Application
{
    public class ClientHandler
    {
        private Task ReceiveTask = Task.CompletedTask;
        private Task SendTask = Task.CompletedTask;
        private Task SpeedHackTask = Task.CompletedTask;
        private Task ProtectionTask = Task.CompletedTask;
        private Task PersonalSkillTask = Task.CompletedTask;
        private Task ScrollSkillTask = Task.CompletedTask;
        private Task FollowTask = Task.CompletedTask;
        private Task TargetTask = Task.CompletedTask;
        private Task BoxCollectTask = Task.CompletedTask;
        private Task CoordinateWalkTask = Task.CompletedTask;
        private Task SupplyTask = Task.CompletedTask;
        private Task PartyTask = Task.CompletedTask;

        public Thread Action()
        {
            return ThreadHandler.Start(() =>
            {
                do
                {
                    if (!Client.ProgramIsActive) continue;

                    if (SpeedHackTask.IsCompleted)
                        SpeedHackTask = Client.Main.SpeedHackAction();

                    Parallel.ForEach(Client.Characters, async (character) =>
                    {
                        await character.CollectCharaterInformation();

                        if (ReceiveTask.IsCompleted)
                            ReceiveTask = character.ReceiveAction();

                        if (SendTask.IsCompleted)
                            SendTask = character.SendAction();

                        if (ProtectionTask.IsCompleted)
                            ProtectionTask = character.ProtectionAction();

                        if (DateTime.Now < character.AvailableExpireTime) return;

                        if (ScrollSkillTask.IsCompleted)
                            ScrollSkillTask = character.ScrollSkillAction();

                        if (PersonalSkillTask.IsCompleted)
                            PersonalSkillTask = character.PersonalSkillAction();

                        if (SupplyTask.IsCompleted)
                            SupplyTask = character.SupplyAction();

                        if (character.IsSupplyAction) return;

                        if (FollowTask.IsCompleted)
                            FollowTask = character.FollowAction();

                        if (BoxCollectTask.IsCompleted)
                            BoxCollectTask = character.BoxCollectAction();

                        if (PartyTask.IsCompleted)
                            PartyTask = character.PartyAction();

                        if (!Client.AttackIsActive) return;

                        if (TargetTask.IsCompleted)
                            TargetTask = character.TargetAction();

                        if (CoordinateWalkTask.IsCompleted)
                            CoordinateWalkTask = character.CoordinateWalkAction();
                    });

                } while (true);
            });
        }
    }
}
