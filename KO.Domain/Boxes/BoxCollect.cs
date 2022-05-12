using System.ComponentModel.DataAnnotations;

namespace KO.Domain.Boxes
{
    public class BoxCollect
    {
        public bool IsCollect { get; protected set; }
        public bool IsPersonalCollect { get; protected set; }
        public bool IsGotoBox { get; protected set; }
        public bool IsOreads { get; protected set; }
        public string LooterTitle { get; protected set; }
        public bool IsListInclude { get; protected set; }
        public bool IsActive { get; protected set; }
        public bool MoneyIsActive { get; protected set; }
        public int[] ItemIdList { get; protected set; }
        public BoxCollectType[] CollectTypes { get; protected set; }
        public BoxCollectType[] DoNotCollectTypes { get; protected set; }

        public BoxCollect(bool isCollect, bool isGotoBox, bool isOreads, string looterTitle, bool isListInclude)
        {
            IsCollect = isCollect;
            IsGotoBox = isGotoBox;
            IsOreads = isOreads;
            LooterTitle = looterTitle;
            IsListInclude = isListInclude;

            IsPersonalCollect = LooterTitle == "Personal Collect";
        }

        public void UpdateIsActive(bool isActive)
        {
            IsActive = isActive;
        }

        public void UpdateMoneyIsActive(bool moneyIsActive)
        {
            MoneyIsActive = moneyIsActive;
        }

        public void UpdateItemIdList(int[] itemIdList)
        {
            ItemIdList = itemIdList;
        }

        public void UpdateCollectTypes(BoxCollectType[] collectTypes)
        {
            CollectTypes = collectTypes;
        }

        public void UpdateDoNotCollectTypes(BoxCollectType[] doNotCollectTypes)
        {
            DoNotCollectTypes = doNotCollectTypes;
        }
    }

    public enum BoxCollectType
    {
        [Display(Name = "Unique", GroupName = "Type")]
        TypeUnique,
        [Display(Name = "Stackable", GroupName = "Type")]
        TypeStackable,
        [Display(Name = "Scroll", GroupName = "Type")]
        TypeScroll,
        [Display(Name = "Rare", GroupName = "Type")]
        TypeRare,
        [Display(Name = "Craft", GroupName = "Type")]
        TypeCraft,
        [Display(Name = "Quest", GroupName = "Type")]
        TypeQuest,
        [Display(Name = "HP Recovery", GroupName = "Type")]
        TypeHpRecovery,
        [Display(Name = "MP Recovery", GroupName = "Type")]
        TypeMpRecovery,

        [Display(Name = "Low", GroupName = "Scroll")]
        ScrollLow,
        [Display(Name = "Middle", GroupName = "Scroll")]
        ScrollMiddle,
        [Display(Name = "High", GroupName = "Scroll")]
        ScrollHigh,

        [Display(Name = "+1", GroupName = "Grade")]
        Grade1,
        [Display(Name = "+2", GroupName = "Grade")]
        Grade2,
        [Display(Name = "+3", GroupName = "Grade")]
        Grade3,
        [Display(Name = "+4", GroupName = "Grade")]
        Grade4,
        [Display(Name = "+5", GroupName = "Grade")]
        Grade5,
        [Display(Name = "+6", GroupName = "Grade")]
        Grade6,
        [Display(Name = "+7", GroupName = "Grade")]
        Grade7,
        [Display(Name = "+8", GroupName = "Grade")]
        Grade8,

        [Display(Name = "Rogue", GroupName = "Class")]
        ClassRogue,
        [Display(Name = "Warrior", GroupName = "Class")]
        ClassWarrior,
        [Display(Name = "Priest", GroupName = "Class")]
        ClassPriest,
        [Display(Name = "Magician", GroupName = "Class")]
        ClassMagician,

        [Display(Name = "Weapon", GroupName = "Wearable")]
        WearableWeapon,
        [Display(Name = "Armor", GroupName = "Wearable")]
        WearableArmor,
        [Display(Name = "Jewelry", GroupName = "Wearable")]
        WearableJewelry,
    }
}
