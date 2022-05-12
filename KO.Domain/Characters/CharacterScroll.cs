using System.ComponentModel.DataAnnotations;

namespace KO.Domain.Characters
{
    public class CharacterScroll
    {
        public bool IsAkaraBlessing { get; protected set; }
        public bool IsSeaCucumberPow { get; protected set; }
        public bool IsStatIncrease { get; protected set; }
        public bool IsDefenseIncrease { get; protected set; }
        public bool IsWeaponEnchant { get; protected set; }
        public bool IsArmorEnchant { get; protected set; }
        public bool IsSpiritOfKaufmann { get; protected set; }
        public bool IsTransformationScroll { get; protected set; }
        public TransformationType TransformationType { get; protected set; }

        public CharacterScroll(bool isAkaraBlessing,
            bool isSeaCucumberPow,
            bool isStatIncrease,
            bool isDefenseIncrease,
            bool isWeaponEnchant,
            bool isArmorEnchant,
            bool isSpiritOfKaufmann,
            bool isTransformationScroll,
            TransformationType transformationType)
        {
            IsAkaraBlessing = isAkaraBlessing;
            IsSeaCucumberPow = isSeaCucumberPow;
            IsStatIncrease = isStatIncrease;
            IsDefenseIncrease = isDefenseIncrease;
            IsWeaponEnchant = isWeaponEnchant;
            IsArmorEnchant = isArmorEnchant;
            IsSpiritOfKaufmann = isSpiritOfKaufmann;
            IsTransformationScroll = isTransformationScroll;
            TransformationType = transformationType;
        }
    }

    public enum TransformationType
    {
        [Display(Name = "Orc Bowman", Description = "472310", GroupName = "Archery", Order = 1)]
        OrcBowman,
        [Display(Name = "Kecon", Description = "472020", GroupName = "Melee", Order = 1)]
        Kecon,
        [Display(Name = "Bulture", Description = "472040", GroupName = "Melee", Order = 1)]
        Bulture,
        [Display(Name = "Zombie", Description = "472050", GroupName = "Melee", Order = 20)]
        Zombie,
        [Display(Name = "Lycan", Description = "472070", GroupName = "Melee", Order = 20)]
        Lycan,
        [Display(Name = "Stripter Scorpion", Description = "472080", GroupName = "Melee", Order = 20)]
        StripterScorpion,
        [Display(Name = "Kobolt", Description = "472090", GroupName = "Melee", Order = 30)]
        Kobolt,
        [Display(Name = "Mastadon", Description = "472130", GroupName = "Melee", Order = 30)]
        Mastadon,
        [Display(Name = "Black Window", Description = "472132", GroupName = "Melee", Order = 30)]
        BlackWindow,
        [Display(Name = "Death Knight", Description = "472150", GroupName = "Melee", Order = 40)]
        DeathKnight,
        [Display(Name = "Burning Skeloton", Description = "472202", GroupName = "Melee", Order = 40)]
        BurningSkeloton,
        [Display(Name = "Raven Harpy", Description = "472250", GroupName = "Melee", Order = 60)]
        RavenHarpy,
        [Display(Name = "UrukTron", Description = "472260", GroupName = "Melee", Order = 60)]
        UrukTron,
        [Display(Name = "Centaur", Description = "472276", GroupName = "Archery", Order = 60)]
        Centaur,
        [Display(Name = "Stone Golem", Description = "472280", GroupName = "Melee", Order = 60)]
        StoneGolem,
    }

}
