using KO.Application.Addresses.Handlers;
using KO.Application.Parties.Extensions;
using KO.Application.Skills.Extensions;
using KO.Application.Skills.Handlers;
using KO.Application.Targets.Extensions;
using KO.Core.Extensions;
using KO.Domain.Characters;
using KO.Domain.Parties;
using KO.Domain.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KO.Application.Parties.Handlers
{
    public static class PartyHandler
    {
        public static async Task RefreshBuffer()
        {
            Parallel.ForEach(Client.Characters, async (character) =>
            {
                var buffers = BufferSkillType.Closed.List();
                var defences = DefenceSkillType.Closed.List();
                var usedSkills = character.GetAllUsedSkill();

                foreach (var item in buffers)
                {
                    var result = usedSkills.FirstOrDefault(x => x.ToString().Contains(item.Group));
                    if (result <= 0) continue;
                    await character.RemoveSkill(new Skill(result));
                }

                foreach (var item in defences)
                {
                    var result = usedSkills.FirstOrDefault(x => x.ToString().Contains(item.Group));
                    if (result <= 0) continue;
                    await character.RemoveSkill(new Skill(result));
                }
            });

            await Task.CompletedTask;
        }

        public static async Task SendPartyAccept(this Character character)
        {
            await character.Send("2F0201");
        }

        public static async Task SendPartyInvite(this Character character, string name)
        {
            await character.Send("2F01", name.Length.ConvertToDword(1), "00", name.ConvertStringToHex());
            await character.Send("2F03", name.Length.ConvertToDword(1), "00", name.ConvertStringToHex());
        }

        public static async Task CollectPartyInformation(this Character character)
        {
            var partyCharacters = new List<Character>();

            for (int i = 0; i < character.GetPartyUserCount(); i++)
            {
                var id = character.GetPartyUserId(i);
                var clientCharater = Client.Characters.FirstOrDefault(x => x.Id == id);
                if (clientCharater != null)
                {
                    clientCharater.UpdateInformation(
                        character.GetTargetDistance(clientCharater.X, clientCharater.Y),
                        clientCharater.GetPartyCureStatus(),
                        clientCharater.GetPartyDiseaseStatus(),
                        clientCharater.GetPartyBufferExists() && !clientCharater.GetPartyReverseLifeStatus() ? DateTime.Now.AddMinutes(10) : DateTime.Now,
                        clientCharater.GetPartyDefenceExists() ? DateTime.Now.AddMinutes(10) : DateTime.Now,
                        clientCharater.GetPartyRestoreExists() ? DateTime.Now.AddMinutes(10) : DateTime.Now,
                        clientCharater.GetPartyResistanceExists() ? DateTime.Now.AddMinutes(10) : DateTime.Now,
                        clientCharater.GetPartyStrengthExists() ? DateTime.Now.AddMinutes(10) : DateTime.Now);

                    partyCharacters.Add(clientCharater);
                    continue;
                }

                var partyCharater = Client.PartyCharacters.FirstOrDefault(x => x.Id == id);

                var health = character.GetPartyUserHealth(i);
                var maxHealth = character.GetPartyUserMaxHealth(i);
                var cureStatus = character.GetPartyUserCureStatus(i);
                var needCureCurse = cureStatus == 256;
                var needCureDisease = cureStatus == 257 || cureStatus == 1 || cureStatus == 65536;
                var baseId = await character.GetTargetBase(id);
                var characterBase = baseId > 0 ? character.GetTargetDetailByBase(baseId) : null;
                var charaterX = characterBase?.X ?? 0;
                var charaterY = characterBase?.Y ?? 0;
                var charaterZ = characterBase?.Z ?? 0;
                var characterDistance = characterBase?.Distance ?? 999;
                var partyBufferExpireTime = partyCharater?.PartyBufferExpireTime ?? DateTime.Now;
                var partyDefenceExpireTime = partyCharater?.PartyDefenceExpireTime ?? DateTime.Now;
                var partyRestoreExpireTime = partyCharater?.PartyRestoreExpireTime ?? DateTime.Now;
                var partyResistanExpireTime = partyCharater?.PartyResistanceExpireTime ?? DateTime.Now;
                var partyAutoStrengthExpireTime = partyCharater?.PartyAutoStrengthExpireTime ?? DateTime.Now;

                if (partyCharater != null)
                {
                    partyCharater.UpdateInformation(
                        id,
                        health,
                        maxHealth,
                        needCureCurse,
                        needCureDisease,
                        charaterX,
                        charaterY,
                        charaterZ,
                        characterDistance,
                        partyBufferExpireTime,
                        partyDefenceExpireTime,
                        partyRestoreExpireTime,
                        partyResistanExpireTime,
                        partyAutoStrengthExpireTime);

                    partyCharacters.Add(partyCharater);
                }
                else
                {
                    partyCharacters.Add(new Character(
                        id,
                        health,
                        maxHealth,
                        needCureCurse,
                        needCureDisease,
                        charaterX,
                        charaterY,
                        charaterZ,
                        characterDistance));
                }
            }

            Client.PartyCharacters = partyCharacters.ToArray();

            await Task.CompletedTask;
        }
    }
}
