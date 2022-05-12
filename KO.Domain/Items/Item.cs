using KO.Core.Constants;
using SQLite.CodeFirst;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KO.Domain.Items
{
    [Table("Items")]
    public class Item
    {
        [Autoincrement, Key]
        public int Id { get; protected set; }

        public int BaseId { get; protected set; } // 0
        public int ExtensionNumber { get; protected set; } // 1
        public string Name { get; protected set; } // 2
        public bool ExtensionBaseIdActive { get; protected set; } // 4
        public int IconId { get; protected set; } // 6
        public int IconDxtId { get; protected set; } // 7
        public int KindId { get; protected set; } // 10
        public int RaceId { get; protected set; } // 13
        public int ClassId { get; protected set; } // 14
        public int Damage { get; protected set; } // 15 + Ext8
        public int Range { get; protected set; } // 17
        public int Weight { get; protected set; } // 18
        public int Durability { get; set; }
        public int MaxDurability { get; protected set; } // 19 + Ext12
        public int SellPrice { get; protected set; } // 20
        public int Defense { get; protected set; } // 22 + Ext14
        public int IsCountable { get; protected set; } // 23
        public int ReqMinLevel { get; protected set; } // 26
        public bool IsPet { get; protected set; } // 28
        public int ReqStatStrength { get; protected set; } // 30 + Ext49
        public int ReqStatHealth { get; protected set; } // 31 + Ext50
        public int ReqStatDexterity { get; protected set; } // 32 + Ext51
        public int ReqStatIntellience { get; protected set; } // 33 + Ext52
        public int ReqStatMagicPower { get; protected set; } // 34 + Ext53
        public int ItemScrollGrade { get; protected set; } // 36

        [NotMapped]
        public int ItemId { get; protected set; }
        [NotMapped]
        public int Page { get; protected set; }
        [NotMapped]
        public int Row { get; protected set; }
        [NotMapped]
        public int InventoryRow { get; protected set; }
        [NotMapped]
        public int Count { get; protected set; }
        [NotMapped]
        public ItemType Type { get; protected set; }
        [NotMapped]
        public ItemInventoryStatusType InventoryStatus { get; set; }
        [NotMapped]
        public ItemInventoryType InventoryType { get; set; }
        [NotMapped]
        public ItemExtension Extension { get; protected set; }

        public Item() { }

        public Item(int page, int row)
        {
            Page = page;
            Row = row;
        }

        public Item(int itemId, int row, int count, int durability, ItemInventoryType inventoryType, ItemInventoryStatusType inventoryStatus)
        {
            ItemId = itemId;
            Row = row;
            Count = count;
            Durability = durability;
            InventoryType = inventoryType;
            InventoryStatus = inventoryStatus;

            InventoryRow = Row - Settings.KO_OFF_INVENTORY_START_ROW;
        }

        public Item(int baseId,
            int itemExtensionNumber,
            string name,
            int extensionBaseIdActiveId,
            int iconId,
            int iconDxtId,
            int kindId,
            int raceId,
            int classId,
            int damage,
            int range,
            int weight,
            int maxDurability,
            int sellPrice,
            int defense,
            int isCountable,
            int reqMinLevel,
            int isPetId,
            int reqStatStrength,
            int reqStatHealth,
            int reqStatDexterity,
            int reqStatIntellience,
            int reqStatMagicPower,
            int itemScrollGrade)
        {
            BaseId = baseId;
            ExtensionNumber = itemExtensionNumber;
            Name = name;
            ExtensionBaseIdActive = Convert.ToBoolean(extensionBaseIdActiveId);
            IconId = iconId;
            IconDxtId = iconDxtId;
            KindId = kindId;
            RaceId = raceId;
            ClassId = classId;
            Damage = damage;
            Range = range;
            Weight = weight;
            MaxDurability = maxDurability;
            SellPrice = sellPrice;
            Defense = defense;
            IsCountable = isCountable;
            ReqMinLevel = reqMinLevel;
            IsPet = Convert.ToBoolean(isPetId);
            ReqStatStrength = reqStatStrength;
            ReqStatHealth = reqStatHealth;
            ReqStatDexterity = reqStatDexterity;
            ReqStatIntellience = reqStatIntellience;
            ReqStatMagicPower = reqStatMagicPower;
            ItemScrollGrade = itemScrollGrade;
        }

        public void UpdateExtension(ItemExtension extension)
        {
            Extension = extension;
        }

        public void UpdateItemId(int itemId)
        {
            ItemId = itemId;
        }

        public void UpdateInventoryItem(int itemId, int count, int durability, int row, ItemInventoryStatusType inventoryStatus)
        {
            ItemId = itemId;
            Count = count;
            Durability = durability;
            Row = row;
            Type = ItemType.Inventory;
            InventoryStatus = inventoryStatus;
            InventoryType = (ItemInventoryType)Row;

            InventoryRow = Row - Settings.KO_OFF_INVENTORY_START_ROW;
        }

        public void UpdateBankItem(int itemId, int count, int durability, int page, int row)
        {
            ItemId = itemId;
            Count = count;
            Durability = durability;
            Page = page;
            Row = row;
            Type = ItemType.Bank;
        }

        public ItemKindType GetItemKindType()
        {
            return (ItemKindType)KindId;
        }

        public int GetCurrentDamage()
        {
            return Extension != null ? Damage + Extension.Damage : Damage;
        }

        public int GetCurrentSellPrice()
        {
            return Extension != null ? (int)(SellPrice / 6.0) * (Extension.PriceMultiply == 0 ? 1 : Extension.PriceMultiply) : SellPrice;
        }

        public int GetCurrentDefense()
        {
            return Extension != null ? Defense + Extension.Defense : Defense;
        }

        public int GetCurrentReqStatStrength()
        {
            return Extension != null ? ReqStatStrength + Extension.ReqStatStrength : ReqStatStrength;
        }

        public int GetCurrentReqStatHealth()
        {
            return Extension != null ? ReqStatHealth + Extension.ReqStatHealth : ReqStatHealth;
        }

        public int GetCurrentReqStatDexterity()
        {
            return Extension != null ? ReqStatDexterity + Extension.ReqStatDexterity : ReqStatDexterity;
        }

        public int GetCurrentReqStatIntellience()
        {
            return Extension != null ? ReqStatIntellience + Extension.ReqStatIntellience : ReqStatIntellience;
        }

        public int GetCurrentReqStatMagicPower()
        {
            return Extension != null ? ReqStatMagicPower + Extension.ReqStatMagicPower : ReqStatMagicPower;
        }

        public int GetCurrentMaxDurability()
        {
            return Extension != null ? MaxDurability + Extension.MaxDurability : MaxDurability;
        }

        public string GetTitleWithGrade()
        {
            return Extension?.ItemBaseId > 0 ? Extension.Name : Name;
        }

        public string GetTitleWithoutGrade()
        {
            return !string.IsNullOrEmpty(GetTitleWithGrade()) ? GetTitleWithGrade().Split(new string[] { "(+" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim() : "";
        }

        public int GetDurabilityPercent()
        {
            return GetCurrentMaxDurability() > 0 ? GetCurrentMaxDurability() / 100 * Durability : 100;
        }

        public int Grade()
        {
            var titleWithGrade = GetTitleWithGrade();
            if (titleWithGrade.Contains("+") && int.TryParse(titleWithGrade.Substring(titleWithGrade.IndexOf('+') + 1).Replace(")", ""), out int gr))
                return gr;

            if (!string.IsNullOrEmpty(Extension?.Description))
            {
                var description = Extension.Description.Split(new string[] { "?", " " }, StringSplitOptions.RemoveEmptyEntries);
                if (description.Length > 0 && int.TryParse(description[description.Length - 1], out int descriptionGrade) && description.Length < 3)
                    return descriptionGrade;
            }

            return Extension?.ItemBaseId > 0 ? 0 : ItemId % 10;
        }
    }

    public enum ItemType
    {
        Inventory,
        Bank
    }

    public enum ItemKindType : int
    {
        [Display(Name = "Rewards", GroupName = "None")]
        Rewards = 0,
        [Display(Name = "Dagger", GroupName = "Rogue,Weapon")]
        Dagger = 11,
        [Display(Name = "[JP] Dagger Set", GroupName = "Rogue,Weapon")]
        JPDaggerSet = 12,
        [Display(Name = "One-handed Sword", GroupName = "Warrior,Weapon")]
        OnehandedSword = 21,
        [Display(Name = "Two-handed Sword", GroupName = "Warrior,Weapon")]
        TwohandedSword = 22,
        [Display(Name = "One-handed Axe", GroupName = "Warrior,Weapon")]
        OnehandedAxe = 31,
        [Display(Name = "Two-handed Axe", GroupName = "Warrior,Weapon")]
        TwohandedAxe = 32,
        [Display(Name = "One-handed Club", GroupName = "Warrior,Priest,Weapon")]
        OnehandedClub = 41,
        [Display(Name = "Two-handed Club", GroupName = "Warrior,Priest,Weapon")]
        TwohandedClub = 42,
        [Display(Name = "[JP] One-handed Set", GroupName = "Warrior,Weapon")]
        JPOnehandedSet = 43,
        [Display(Name = "One-handed Spear", GroupName = "Warrior,Weapon")]
        OnehandedSpear = 51,
        [Display(Name = "Two-handed Spear", GroupName = "Warrior,Weapon")]
        TwohandedSpear = 52,
        [Display(Name = "Shield", GroupName = "Warrior,Priest,Weapon")]
        Shield = 60,
        [Display(Name = "Pickaxe", GroupName = "Warrior,Weapon")]
        Pickaxe = 61,
        [Display(Name = "Long Spear", GroupName = "Warrior,Weapon")]
        LongSpear = 62,
        [Display(Name = "Fishing rod", GroupName = "Weapon")]
        Fishingrod = 63,
        [Display(Name = "Bow", GroupName = "Rogue,Weapon")]
        Bow = 70,
        [Display(Name = "Crossbow", GroupName = "Rogue,Weapon")]
        Crossbow = 71,
        [Display(Name = "Earring", GroupName = "Jewelry")]
        Earring = 91,
        [Display(Name = "Necklace", GroupName = "Jewelry")]
        Necklace = 92,
        [Display(Name = "Ring", GroupName = "Jewelry")]
        Ring = 93,
        [Display(Name = "Belt", GroupName = "Jewelry")]
        Belt = 94,
        [Display(Name = "Lune Item", GroupName = "None")]
        LuneItem = 95,
        [Display(Name = "Sundries", GroupName = "None")]
        Sundries = 97,
        [Display(Name = "Scroll", GroupName = "None")]
        Scroll = 98,
        [Display(Name = "Monster's Stone", GroupName = "None")]
        MonstersStone = 101,
        [Display(Name = "Staff", GroupName = "Magician,Weapon")]
        Staff = 110,
        [Display(Name = "Arrow", GroupName = "None")]
        Arrow = 120,
        [Display(Name = "Javelin", GroupName = "None")]
        Javelin = 130,
        [Display(Name = "Jamadhar", GroupName = "None")]
        Jamadhar = 140,
        [Display(Name = "Familiar Egg", GroupName = "None")]
        FamiliarEgg = 150,
        [Display(Name = "Familiar", GroupName = "None")]
        Familiar = 151,
        [Display(Name = "[US] Familiar", GroupName = "None")]
        USFamiliar = 152,
        [Display(Name = "Cypher Ring", GroupName = "None")]
        CypherRing = 160,
        [Display(Name = "Autoloot", GroupName = "None")]
        Autoloot = 170,
        [Display(Name = "Image Change Scroll", GroupName = "None")]
        ImageChangeScroll = 171,
        [Display(Name = "Familiar Attack", GroupName = "None")]
        FamiliarAttack = 172,
        [Display(Name = "Familiar Defense", GroupName = "None")]
        FamiliarDefense = 173,
        [Display(Name = "Familiar Loyality", GroupName = "None")]
        FamiliarLoyality = 174,
        [Display(Name = "Familiar Speciality Food", GroupName = "None")]
        FamiliarSpecialityFood = 175,
        [Display(Name = "Familiar Food", GroupName = "None")]
        FamiliarFood = 176,
        [Display(Name = "Priest Mace", GroupName = "Priest")]
        PriestMace = 181,
        [Display(Name = "Warrior Armor", GroupName = "Warrior,Armor")]
        WarriorArmor = 210,
        [Display(Name = "Rogue Armor", GroupName = "Rogue,Armor")]
        RogueArmor = 220,
        [Display(Name = "Magician Armor", GroupName = "Magician,Armor")]
        MagicianArmor = 230,
        [Display(Name = "Priest Armor", GroupName = "Priest,Armor")]
        PriestArmor = 240,
        [Display(Name = "Seal & Heal Scroll", GroupName = "None")]
        SealHealScroll = 250,
        [Display(Name = "Cosplay", GroupName = "None")]
        Cosplay = 252,
        [Display(Name = "Sealed item", GroupName = "None")]
        Sealeditem = 253,
        [Display(Name = "Chaos Skill Item", GroupName = "None")]
        ChaosSkillItem = 254,
        [Display(Name = "PUS Item", GroupName = "None")]
        PUSItem = 255,
    }

    public enum ItemInventoryStatusType
    {
        LeftEarring,
        Helmet,
        RightEarring,
        Pendant,
        Pauldron,
        Pet,
        LeftWeapon,
        Belt,
        RightWeapon,
        LeftRing,
        Pants,
        RightRing,
        Gauntlet,
        Boots,
        Row
    }

    public enum ItemInventoryType : byte
    {
        LeftEarring,
        Helmet,
        RightEarring,
        Pendant,
        Pauldron,
        Pet,
        LeftWeapon,
        Belt,
        RightWeapon,
        LeftRing,
        Pad,
        RightRing,
        Gauntlet,
        Boots,
        Row1,
        Row2,
        Row3,
        Row4,
        Row5,
        Row6,
        Row7,
        Row8,
        Row9,
        Row10,
        Row11,
        Row12,
        Row13,
        Row14,
        Row15,
        Row16,
        Row17,
        Row18,
        Row19,
        Row20,
        Row21,
        Row22,
        Row23,
        Row24,
        Row25,
        Row26,
        Row27,
        Row28
    }
}
