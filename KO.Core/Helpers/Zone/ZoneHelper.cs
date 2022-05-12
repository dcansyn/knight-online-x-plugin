namespace KO.Core.Helpers.Zone
{
    public class ZoneHelper
    {
        public static string GetNameById(int zoneId)
        {
            switch (zoneId)
            {
                case 1:
                case 5: return "Luferson";
                case 2: return "El Morad";
                case 11:
                case 13: return "Karus - Eslant";
                case 12:
                case 14: return "Human - Eslant";
                case 21:
                case 22:
                case 23: return "Moradon";
                case 30: return "Delos";
                case 32: return "Abys";
                case 34: return "Felankor's Lair";
                case 48: return "Arena";
                case 71: return "Ronark Land";
                case 72: return "Ardream";
                case 73: return "Ronark Land Base";
                case 82: return "Adonis";
                case 85: return "Chaos";
                default: return $"{zoneId} (Unknown)";
            }
        }
    }
}
