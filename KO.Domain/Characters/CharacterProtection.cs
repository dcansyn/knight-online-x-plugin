namespace KO.Domain.Characters
{
    public class CharacterProtection
    {
        public bool IsAutomaticPotion { get; protected set; }
        public bool IsManaPotion { get; protected set; }
        public bool IsMultipleManaPotion { get; protected set; }
        public int ManaPotionPercent { get; protected set; }
        public bool IsHealthPotion { get; protected set; }
        public bool IsMultipleHealthPotion { get; protected set; }
        public int HealthPotionPercent { get; protected set; }
        public bool IsMinor { get; protected set; }
        public int MinorPercent { get; protected set; }
        public bool IsMagicHammer { get; protected set; }
        public bool IsEternity { get; protected set; }
        public int EternityPercent { get; protected set; }
        public bool IsSuicide { get; protected set; }
        public int SuicidePercent { get; protected set; }
        public bool IsSuicideF4 { get; protected set; }
        public bool IsGetUpWhenYouDie { get; protected set; }
        public bool IsGetUpReturnSlot { get; protected set; }

        public CharacterProtection(bool isAutomaticPotion,
            bool isManaPotion,
            bool isMultipleManaPotion,
            int manaPotionPercent,
            bool isHealthPotion,
            bool isMultipleHealthPotion,
            int healthPotionPercent,
            bool isMinor,
            int minorPercent,
            bool isMagicHammer,
            bool isEternity,
            int eternityPercent,
            bool isSuicide,
            int suicidePercent,
            bool isSuicideF4,
            bool isGetUpWhenYouDie,
            bool isGetUpReturnSlot)
        {
            IsAutomaticPotion = isAutomaticPotion;
            IsManaPotion = isManaPotion;
            IsMultipleManaPotion = isMultipleManaPotion;
            ManaPotionPercent = manaPotionPercent;
            IsHealthPotion = isHealthPotion;
            IsMultipleHealthPotion = isMultipleHealthPotion;
            HealthPotionPercent = healthPotionPercent;
            IsMinor = isMinor;
            MinorPercent = minorPercent;
            IsMagicHammer = isMagicHammer;
            IsEternity = isEternity;
            EternityPercent = eternityPercent;
            IsSuicide = isSuicide;
            SuicidePercent = suicidePercent;
            IsSuicideF4 = isSuicideF4;
            IsGetUpWhenYouDie = isGetUpWhenYouDie;
            IsGetUpReturnSlot = isGetUpReturnSlot;
        }
    }
}
