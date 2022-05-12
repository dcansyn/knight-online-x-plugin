using SQLite.CodeFirst;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KO.Domain.Items
{
    [Table("ItemExtensions")]
    public class ItemExtension
    {
        [Autoincrement, Key]
        public int Id { get; protected set; }

        public int Number { get; protected set; }
        public int BaseId { get; protected set; } // 0
        public string Name { get; protected set; } // 1
        public int ItemBaseId { get; protected set; } // 2
        public string Description { get; protected set; } // 3
        public int IconDxtId { get; protected set; } // 5
        public int IconId { get; protected set; } // 6
        public int TypeId { get; protected set; } // 7
        public int Damage { get; protected set; } // 8 + Org15
        public int AttackIntervalPercentage { get; protected set; } // 9
        public int AttackPowerRate { get; protected set; } // 10
        public int DodgeRate { get; protected set; } // 11
        public int MaxDurability { get; protected set; } // 12 + Org19
        public int PriceMultiply { get; protected set; } // 13
        public int Defense { get; protected set; } // 14 + Org22
        public int DaggerDefense { get; protected set; } // 15
        public int JamadarDefense { get; protected set; } // 16
        public int SwordDefense { get; protected set; } // 17
        public int ClubDefense { get; protected set; } // 18
        public int AxeDefense { get; protected set; } // 19
        public int SpearDefense { get; protected set; } // 20
        public int ArrowDefense { get; protected set; } // 21
        public int FireDamage { get; protected set; } // 22
        public int GlacierDamage { get; protected set; } // 23
        public int LightningDamage { get; protected set; } // 24
        public int PosionDamage { get; protected set; } // 25
        public int HpRecovery { get; protected set; } // 26
        public int MpDamage { get; protected set; } // 27
        public int MpRecovery { get; protected set; } // 28
        public int ReturnPhysicalDamage { get; protected set; } // 29
        public int StrengthBonus { get; protected set; } // 31
        public int HealthBonus { get; protected set; } // 32
        public int DexterityBonus { get; protected set; } // 33
        public int IntellienceBonus { get; protected set; } // 34
        public int MagicPowerBonus { get; protected set; } // 35
        public int HpBonus { get; protected set; } // 36
        public int MpBonus { get; protected set; } // 37
        public int ResistanceToFlame { get; protected set; } // 38
        public int ResistanceToGlacier { get; protected set; } // 39
        public int ResistanceToLightning { get; protected set; } // 40
        public int ResistanceToMagic { get; protected set; } // 41
        public int ResistanceToPosion { get; protected set; } // 42
        public int ResistanceToCurse { get; protected set; } // 43
        public int ReqStatStrength { get; protected set; } // 49 + Org30
        public int ReqStatHealth { get; protected set; } // 50 + Org31
        public int ReqStatDexterity { get; protected set; } // 51 + Org32
        public int ReqStatIntellience { get; protected set; } // 52 + Org33
        public int ReqStatMagicPower { get; protected set; } // 53 + Org34

        public ItemExtension() { }

        public ItemExtension(int number,
            int baseId,
            string name,
            int itemBaseId,
            string description,
            int iconDxtId,
            int iconId,
            int typeId,
            int damage,
            int attackIntervalPercentage,
            int attackPowerRate,
            int dodgeRate,
            int maxDurability,
            int priceMultiply,
            int defense,
            int daggerDefense,
            int jamadarDefense,
            int swordDefense,
            int clubDefense,
            int axeDefense,
            int spearDefense,
            int arrowDefense,
            int fireDamage,
            int glacierDamage,
            int lightningDamage,
            int posionDamage,
            int hpRecovery,
            int mpDamage,
            int mpRecovery,
            int returnPhysicalDamage,
            int strengthBonus,
            int healthBonus,
            int dexterityBonus,
            int intellienceBonus,
            int magicPowerBonus,
            int hpBonus,
            int mpBonus,
            int resistanceToFlame,
            int resistanceToGlacier,
            int resistanceToLightning,
            int resistanceToMagic,
            int resistanceToPosion,
            int resistanceToCurse,
            int reqStatStrength,
            int reqStatHealth,
            int reqStatDexterity,
            int reqStatIntellience,
            int reqStatMagicPower)
        {
            Number = number;
            BaseId = baseId;
            Name = name;
            ItemBaseId = itemBaseId;
            Description = description;
            IconDxtId = iconDxtId;
            IconId = iconId;
            TypeId = typeId;
            Damage = damage;
            AttackIntervalPercentage = attackIntervalPercentage;
            AttackPowerRate = attackPowerRate;
            DodgeRate = dodgeRate;
            MaxDurability = maxDurability;
            PriceMultiply = priceMultiply;
            Defense = defense;
            DaggerDefense = daggerDefense;
            JamadarDefense = jamadarDefense;
            SwordDefense = swordDefense;
            ClubDefense = clubDefense;
            AxeDefense = axeDefense;
            SpearDefense = spearDefense;
            ArrowDefense = arrowDefense;
            FireDamage = fireDamage;
            GlacierDamage = glacierDamage;
            LightningDamage = lightningDamage;
            PosionDamage = posionDamage;
            HpRecovery = hpRecovery;
            MpDamage = mpDamage;
            MpRecovery = mpRecovery;
            ReturnPhysicalDamage = returnPhysicalDamage;
            StrengthBonus = strengthBonus;
            HealthBonus = healthBonus;
            DexterityBonus = dexterityBonus;
            IntellienceBonus = intellienceBonus;
            MagicPowerBonus = magicPowerBonus;
            HpBonus = hpBonus;
            MpBonus = mpBonus;
            ResistanceToFlame = resistanceToFlame;
            ResistanceToGlacier = resistanceToGlacier;
            ResistanceToLightning = resistanceToLightning;
            ResistanceToMagic = resistanceToMagic;
            ResistanceToPosion = resistanceToPosion;
            ResistanceToCurse = resistanceToCurse;
            ReqStatStrength = reqStatStrength;
            ReqStatHealth = reqStatHealth;
            ReqStatDexterity = reqStatDexterity;
            ReqStatIntellience = reqStatIntellience;
            ReqStatMagicPower = reqStatMagicPower;
        }

        public ItemExtType GetExtensionType()
        {
            return (ItemExtType)TypeId;
        }
    }

    public enum ItemExtType : byte
    {
        Normal = 0,
        Magic = 1,
        Rare = 2,
        Craft = 3,
        Unique = 4,
        Upgrade = 5,
        Event = 6,
        Pet = 7,
        Cospre = 8,
        Minevra = 9,
        Rebith = 11,
        Reverse = 12,
    }
}
