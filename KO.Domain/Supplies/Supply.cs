using KO.Domain.Characters;
using System.ComponentModel.DataAnnotations;

namespace KO.Domain.Supplies
{
    public class Supply
    {
        public bool IsActive { get; protected set; }
        public bool IsActiveWhenItemConsume { get; protected set; }
        public bool IsActiveWhenInventoryFull { get; protected set; }
        public CharacterWalkType WalkType { get; protected set; }
        public bool RepairWeapon { get; protected set; }
        public bool RepairHelmet { get; protected set; }
        public bool RepairPauldron { get; protected set; }
        public bool RepairPants { get; protected set; }
        public bool RepairGauntlet { get; protected set; }
        public bool RepairBoots { get; protected set; }
        public bool BuyHealthPotion { get; protected set; }
        public SupplyHealthPotionType HealthPotionType { get; protected set; }
        public int HealthPotionCount { get; protected set; }
        public bool BuyManaPotion { get; protected set; }
        public SupplyManaPotionType ManaPotionType { get; protected set; }
        public int ManaPotionCount { get; protected set; }
        public bool BuyWolf { get; protected set; }
        public int WolfCount { get; protected set; }
        public bool BuyTransformationGem { get; protected set; }
        public int TransformationGemCount { get; protected set; }
        public bool BuyArrow { get; protected set; }
        public int ArrowCount { get; protected set; }
        public bool BuyPrayerOfGodsPower { get; protected set; }
        public int PrayerOfGodsPowerCount { get; protected set; }
        public bool BankHealthPotion { get; protected set; }
        public SupplyHealthPotionType BankHealthPotionType { get; protected set; }
        public int BankHealthPotionCount { get; protected set; }
        public bool BankManaPotion { get; protected set; }
        public SupplyManaPotionType BankManaPotionType { get; protected set; }
        public int BankManaPotionCount { get; protected set; }
        public bool FillToBankWithPotion { get; protected set; }
        public SupplyAction[] Actions { get; protected set; }

        public Supply(bool isActive,
            bool isActiveWhenItemConsume,
            bool isActiveWhenInventoryFull,
            CharacterWalkType walkType,
            bool repairWeapon,
            bool repairHelmet,
            bool repairPauldron,
            bool repairPants,
            bool repairGauntlet,
            bool repairBoots,
            bool buyHealthPotion,
            SupplyHealthPotionType healthPotionType,
            int healthPotionCount,
            bool buyManaPotion,
            SupplyManaPotionType manaPotionType,
            int manaPotionCount,
            bool buyWolf,
            int wolfCount,
            bool buyTransformationGem,
            int transformationGemCount,
            bool buyArrow,
            int arrowCount,
            bool buyPrayerOfGodsPower,
            int prayerOfGodsPowerCount,
            bool bankHealthPotion,
            SupplyHealthPotionType bankHealthPotionType,
            int bankHealthPotionCount,
            bool bankManaPotion,
            SupplyManaPotionType bankManaPotionType,
            int bankManaPotionCount,
            bool fillToBankWithPotion,
            SupplyAction[] actions)
        {
            IsActive = isActive;
            IsActiveWhenItemConsume = isActiveWhenItemConsume;
            IsActiveWhenInventoryFull = isActiveWhenInventoryFull;
            WalkType = walkType;
            RepairWeapon = repairWeapon;
            RepairHelmet = repairHelmet;
            RepairPauldron = repairPauldron;
            RepairPants = repairPants;
            RepairGauntlet = repairGauntlet;
            RepairBoots = repairBoots;
            BuyHealthPotion = buyHealthPotion;
            HealthPotionType = healthPotionType;
            HealthPotionCount = healthPotionCount;
            BuyManaPotion = buyManaPotion;
            ManaPotionType = manaPotionType;
            ManaPotionCount = manaPotionCount;
            BuyWolf = buyWolf;
            WolfCount = wolfCount;
            BuyTransformationGem = buyTransformationGem;
            TransformationGemCount = transformationGemCount;
            BuyArrow = buyArrow;
            ArrowCount = arrowCount;
            BuyPrayerOfGodsPower = buyPrayerOfGodsPower;
            PrayerOfGodsPowerCount = prayerOfGodsPowerCount;
            BankHealthPotion = bankHealthPotion;
            BankHealthPotionType = bankHealthPotionType;
            BankHealthPotionCount = bankHealthPotionCount;
            BankManaPotion = bankManaPotion;
            BankManaPotionType = bankManaPotionType;
            BankManaPotionCount = bankManaPotionCount;
            FillToBankWithPotion = fillToBankWithPotion;
            Actions = actions;
        }

        public void UpdateFillToBankWithPotion(bool fillToBankWithPotion)
        {
            FillToBankWithPotion = fillToBankWithPotion;
        }
    }

    public enum SupplyHealthPotionType
    {
        [Display(GroupName = "389010000")]
        HealthPotion45,

        [Display(GroupName = "389011000")]
        HealthPotion90,

        [Display(GroupName = "389012000")]
        HealthPotion180,

        [Display(GroupName = "389013000")]
        HealthPotion360,

        [Display(GroupName = "389014000")]
        HealthPotion720,
    }

    public enum SupplyManaPotionType
    {
        [Display(GroupName = "389016000")]
        ManaPotion90 = 6,

        [Display(GroupName = "389017000")]
        ManaPotion180 = 7,

        [Display(GroupName = "389018000")]
        ManaPotion480 = 8,

        [Display(GroupName = "389019000")]
        ManaPotion960 = 9,

        [Display(GroupName = "389020000")]
        ManaPotion1920 = 10
    }

    public enum SupplyNpcType
    {
        [Display(Name = "Sundries", GroupName = "2101", ShortName = "18E40300")]
        SundriesBuy,

        [Display(Name = "Potion", GroupName = "2101", ShortName = "48DC0300")]
        PotionBuy,

        [Display(Name = "Portion", GroupName = "2101", ShortName = "48DC0300")]
        PortionBuy,

        [Display(Name = "Scroll", GroupName = "2101", ShortName = "30E00300")]
        ScrollBuy,
    }
}


//2101 48DC0300 CF4A0160F92F170D0100000A