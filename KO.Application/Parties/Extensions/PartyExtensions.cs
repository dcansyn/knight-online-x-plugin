using KO.Application.Addresses.Extensions;
using KO.Core.Constants;
using KO.Domain.Characters;

namespace KO.Application.Parties.Extensions
{
    public static class PartyExtensions
    {
        public static int GetPartyUserCount(this Character character)
        {
            return character.ReadLong(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_PARTY_BASE) + Settings.KO_OFF_PARTY_COUNT);
        }

        public static int GetPartyUserBase(this Character character, int row)
        {
            var result = character.ReadLong(character.ReadLong(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_PARTY_BASE) + Settings.KO_OFF_PARTY_USER_BASE));

            for (int i = 0; i < row; i++)
                result = character.ReadLong(result);

            return result;
        }

        public static int GetPartyUserId(this Character character, int row)
        {
            return character.ReadLong(character.GetPartyUserBase(row) + Settings.KO_OFF_PARTY_ID);
        }

        public static int GetPartyUserClass(this Character character, int row)
        {
            return character.ReadLong(character.GetPartyUserBase(row) + Settings.KO_OFF_PARTY_CLASS);
        }

        public static int GetPartyUserCureStatus(this Character character, int row)
        {
            return character.ReadLong(character.GetPartyUserBase(row) + Settings.KO_OFF_PARTY_CURE);
        }

        public static int GetPartyUserHealth(this Character character, int row)
        {
            return character.ReadLong(character.GetPartyUserBase(row) + Settings.KO_OFF_PARTY_HP);
        }

        public static int GetPartyUserMaxHealth(this Character character, int row)
        {
            return character.ReadLong(character.GetPartyUserBase(row) + Settings.KO_OFF_PARTY_MAX_HP);
        }
    }
}
