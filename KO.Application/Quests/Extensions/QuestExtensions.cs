using KO.Application.Characters.Extensions;
using KO.Application.Items.Repositories;
using KO.Domain.Characters;
using KO.Domain.Items;
using KO.Domain.Quests.Data;
using KO.Domain.Skills;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KO.Application.Quests.Extensions
{
    public static class QuestExtensions
    {
        public static async Task<int> GetQuestAvailableItem(this Character character, QuestRewardData[] rewards)
        {
            using (var itemRepository = new ItemRepository())
            {
                var maxStat = character.GetCharacterMaxStat();
                if (maxStat == CharacterStatType.Point)
                    throw new ArgumentNullException("Character stat not found.");

                for (int i = 0; i < rewards.Length; i++)
                {
                    var reward = rewards[i];
                    var item = await itemRepository.GetByItemId(reward.ItemId);

                    if (item.KindId == 200 + ((int)character.ClassType * 10))
                        return i;

                    switch (maxStat)
                    {
                        case CharacterStatType.Strength:
                            if (item.Extension.StrengthBonus > 0) return i;
                            break;
                        case CharacterStatType.Health:
                            if (item.Extension.HealthBonus > 0) return i;
                            break;
                        case CharacterStatType.Dexterity:
                            if (item.Extension.DexterityBonus > 0) return i;
                            break;
                        case CharacterStatType.MagicPower:
                            if (item.Extension.MagicPowerBonus > 0) return i;
                            break;
                        case CharacterStatType.Intelligence:
                            if (item.Extension.IntellienceBonus > 0) return i;
                            break;
                    }

                    switch (maxStat)
                    {
                        case CharacterStatType.Strength:
                            if (item.GetCurrentReqStatStrength() > 10) return i;
                            break;
                        case CharacterStatType.Health:
                            if (item.GetCurrentReqStatHealth() > 10) return i;
                            break;
                        case CharacterStatType.Dexterity:
                            if (item.GetCurrentReqStatDexterity() > 10)
                            {
                                switch (item.GetItemKindType())
                                {
                                    case ItemKindType.Bow:
                                    case ItemKindType.Crossbow:
                                        if (character.AttackType == CharacterAttackType.Archery)
                                            return i;
                                        break;

                                    case ItemKindType.Dagger:
                                    case ItemKindType.JPDaggerSet:
                                        if (character.AttackType == CharacterAttackType.Melee)
                                            return i;
                                        break;

                                    default:
                                        return i;
                                }
                            }
                            break;
                        case CharacterStatType.MagicPower:
                            if (item.GetCurrentReqStatMagicPower() > 10) return i;
                            break;
                        case CharacterStatType.Intelligence:
                            if (item.GetCurrentReqStatIntellience() > 10) return i;
                            break;
                    }

                    var typeList = new[] { 11, 21, 22, 31, 32, 41, 42, 51, 52, 60, 70, 71, 110 };
                    if (typeList.Contains(item.KindId))
                    {
                        switch (character.ClassType)
                        {
                            case CharacterClassType.Warrior:
                            case CharacterClassType.Kurian:
                            case CharacterClassType.Priest:
                                break;
                            case CharacterClassType.Rogue:
                                switch (item.KindId)
                                {
                                    case 11:
                                    case 71:
                                    case 70:
                                        return (int)character.GetCharacterMaxSkill();
                                }
                                break;
                            case CharacterClassType.Magician:
                                switch (item.KindId)
                                {
                                    case 110:
                                        return (int)character.GetCharacterMaxSkill();
                                }
                                break;
                        }
                    }
                }
            }

            return -1;
        }
    }
}
