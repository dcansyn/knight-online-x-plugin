using KO.Core.Extensions;
using KO.Domain.Characters;
using SQLite.CodeFirst;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KO.Domain.Skills
{
    [Table("Skills")]
    public class Skill
    {
        [Autoincrement, Key]
        public int Id { get; protected set; }

        public int BaseId { get; protected set; } // 0
        public string Name { get; protected set; } // 2
        public string Description { get; protected set; } // 3
        public int SelfEffect { get; protected set; } // 7
        public int RequiredFlyEffect { get; protected set; } // 11
        public int MoralTypeBase { get; protected set; } // 14
        public int Point { get; protected set; } // 15
        public int ClassBaseId { get; protected set; } // 16
        public int Mana { get; protected set; } // 17
        public int RequiredItem { get; protected set; } // 21
        public int CastEffectBase { get; protected set; } // 22
        public int CooldownBase { get; protected set; } // 23
        public int MaxRange { get; protected set; } // 27
        public int ExtensionNumber { get; protected set; } // 28

        public int SkillNumber { get; protected set; }
        public int SkillCode { get; protected set; }
        public string PacketCode { get; protected set; }
        public int Mastery { get; protected set; }
        public int ClassId { get; protected set; }
        public int CastEffect { get; protected set; }
        public int Cooldown { get; protected set; }

        [NotMapped]
        public SkillExtension Extension { get; protected set; }
        [NotMapped]
        public DateTime UseTime { get; set; } = DateTime.Now;
        [NotMapped]
        public CharacterClassType ClassType { get; protected set; }
        [NotMapped]
        public SkillType Type { get; protected set; }
        [NotMapped]
        public int TargetId { get; protected set; }

        public Skill() { }

        public Skill(int baseId)
        {
            BaseId = baseId;
        }

        public Skill(int baseId, int moralTypeBase = 0, int cooldownBase = 0, int castEffectBase = 0)
        {
            BaseId = baseId;
            MoralTypeBase = moralTypeBase;
            CooldownBase = cooldownBase;
            CastEffectBase = castEffectBase;

            SkillNumber = BaseId % 1000;
            SkillCode = BaseId % 100;
            PacketCode = BaseId.ConvertToDword(4);
            Mastery = (SkillNumber > 100 ? (SkillNumber - (SkillNumber % 100)) / 100 : 0) - 5;
            Mastery = Mastery < 0 ? 0 : Mastery;
            ClassId = (ClassBaseId - (ClassBaseId % 10)) / 10;
            CastEffect = CastEffectBase * 100;
            Cooldown = (CooldownBase * 100) + 100;
        }

        public Skill(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public Skill(int baseId, string name, string description, int selfEffect, int requiredFlyEffect, int moralTypeBase, int point, int classBaseId, int mana, int requiredItem, int castEffect, int cooldownBase, int maxRange, int extensionNumber)
        {
            BaseId = baseId;
            Name = name;
            Description = description;
            SelfEffect = selfEffect;
            RequiredFlyEffect = requiredFlyEffect;
            MoralTypeBase = moralTypeBase;
            Point = point;
            ClassBaseId = classBaseId;
            Mana = mana;
            RequiredItem = requiredItem;
            CastEffectBase = castEffect;
            CooldownBase = cooldownBase;
            MaxRange = maxRange;
            ExtensionNumber = extensionNumber;

            SkillNumber = BaseId % 1000;
            SkillCode = BaseId % 100;
            PacketCode = BaseId.ConvertToDword(4);
            Mastery = (SkillNumber > 100 ? (SkillNumber - (SkillNumber % 100)) / 100 : 0) - 5;
            Mastery = Mastery < 0 ? 0 : Mastery;
            ClassId = (ClassBaseId - (ClassBaseId % 10)) / 10;
            CastEffect = CastEffectBase * 100;
            Cooldown = (CooldownBase * 100) + 100;
        }

        public SkillMoralType GetMoralType()
        {
            return (SkillMoralType)MoralTypeBase;
        }

        public void UpdateSkillExtension(SkillExtension extension)
        {
            Extension = extension;
        }

        public void UpdateMoralTypeBase(int moralTypeBase)
        {
            MoralTypeBase = moralTypeBase;
        }

        public void UpdateMaxRange(int maxRange)
        {
            MaxRange = maxRange;
        }

        public void UpdateUseTime()
        {
            UseTime = DateTime.Now.AddMilliseconds(Cooldown + 500);
        }

        public void UpdateType(SkillType type)
        {
            Type = type;
        }

        public void UpdateCharacterClassType(CharacterClassType classType)
        {
            ClassType = classType;
        }

        public void UpdateCooldown(int cooldown)
        {
            Cooldown = cooldown;
        }

        public void UpdateTargetId(int targetId)
        {
            TargetId = targetId;
        }
    }

    public enum SkillType : byte
    {
        Unknown,
        Personal,
        Regular,
        Special,
        Melee,
        Throw,
        Area,
        Archery,
        RainOfArrows,
    }

    public enum SkillMoralType
    {
        NonEffect,
        Personal = 1,
        Global = 2,
        Scroll = 3,
        Party1 = 4,
        AttackAbyss = 5,
        Party2 = 6,
        Attack = 7,
        AttackArea = 10
    }

    public enum CharacterSkillType : byte
    {
        Basic,
        Tab1,
        Tab2,
        Tab3,
        Master,
        Point
    }

    public enum CharacterStatType : byte
    {
        Point,
        Strength,
        Health,
        Dexterity,
        MagicPower,
        Intelligence
    }

    public enum BuffSkillType
    {
        [Display(GroupName = "492061")]
        AkaraBlessing = 492024,

        [Display(GroupName = "492061")]
        SeaCucumperPow = 492023,

        SpiritOfKaufman = 500125,

        BlessOfTemplateStat = 492059,

        [Display(GroupName = "492024,492023")]
        BlessOfTemplateDefence = 492061,

        [Display(Prompt = "389155000")]
        WeaponEnchant = 500049,

        [Display(Prompt = "389156000")]
        ArmorEnchant = 500050,
    }
}
