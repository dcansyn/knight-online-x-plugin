using KO.Core.Constants;
using KO.Core.Enums.Memory;
using KO.Core.Extensions;
using KO.Core.Helpers.Memory;
using KO.Domain.Items;
using KO.Domain.Skills;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace KO.Domain.Characters
{
    public class Character
    {
        public bool IsMain { get; protected set; }
        public string GameName { get; protected set; }

        public IntPtr Handle { get; protected set; }
        public IntPtr ReceiveHandle { get; protected set; }
        public IntPtr SendHandle { get; protected set; }
        public IntPtr SendAddress { get; protected set; }
        public IntPtr RemoteAddress { get; protected set; }
        public IntPtr TargetBaseAddress { get; protected set; }
        public IntPtr RecvMailAddress { get; protected set; }
        public IntPtr RecvHookAddress { get; protected set; }
        public IntPtr SendMailAddress { get; protected set; }

        public int Id { get; protected set; }
        public string HexId { get; protected set; }
        public string Name { get; protected set; }
        public int Level { get; protected set; }
        public int X { get; protected set; }

        public int Y { get; protected set; }
        public int Z { get; protected set; }
        public int Distance { get; protected set; }

        public int CenterDistance { get; protected set; }
        public int Health { get; protected set; }
        public int MaxHealth { get; protected set; }
        public int LastHealth { get; protected set; }
        public int Mana { get; protected set; }
        public int MaxMana { get; protected set; }
        public int LastMana { get; protected set; }
        public int ClassId { get; protected set; }
        public int Exp { get; protected set; }
        public int MaxExp { get; protected set; }
        public int Gold { get; protected set; }
        public int Attack { get; protected set; }
        public int Defence { get; protected set; }
        public int Weight { get; protected set; }
        public int MaxWeight { get; protected set; }
        public int ZoneId { get; protected set; }
        public string Zone { get; protected set; }
        public string Server { get; protected set; }
        public bool NeedCureCurse { get; set; }
        public bool NeedCureDisease { get; set; }

        public CharacterType Type { get; protected set; }
        public CharacterAttackType AttackType { get; protected set; }
        public CharacterRaceType RaceType { get; protected set; }
        public CharacterClassType ClassType { get; protected set; }
        public CharacterClassNameType ClassNameType { get; protected set; }
        public CharacterMoveType MoveType { get; protected set; }
        public CharacterWalkType WalkType { get; protected set; }
        public CharacterStatusType StatusType { get; protected set; }

        public bool IsBoxCollector { get; protected set; } = true;
        public bool IsSupplyAction { get; protected set; } = false;

        public int SupplyActionRow { get; protected set; } = -1;
        public int BankFillActionRow { get; protected set; } = -1;

        public Item[] InventoryItems { get; protected set; } = new Item[] { };
        public Item[] BankItems { get; protected set; } = new Item[] { };
        public Skill[] UsedSkills { get; protected set; } = new Skill[] { };
        public Character[] Targets { get; protected set; } = new Character[] { };

        public DateTime? DeathTime { get; protected set; } = null;
        public DateTime AvailableExpireTime { get; protected set; } = DateTime.Now;
        public DateTime GlobalExpireTime { get; protected set; } = DateTime.Now;
        public DateTime EternityExpireTime { get; protected set; } = DateTime.Now;
        public DateTime TransformationScrollExpireTime { get; protected set; } = DateTime.Now;
        public DateTime MagicHammerExpireTime { get; protected set; } = DateTime.Now;
        public DateTime BufferScollExpireTime { get; protected set; } = DateTime.Now;
        public DateTime LastTargetAttackExpireTime { get; protected set; } = DateTime.Now;
        public DateTime FollowExpireTime { get; protected set; } = DateTime.Now;
        public DateTime WalkExpireTime { get; protected set; } = DateTime.Now;
        public DateTime TargetSelectExpireTime { get; protected set; } = DateTime.Now;
        public DateTime AttackSkillExpireTime { get; protected set; } = DateTime.Now;
        public DateTime RegularSkillExpireTime { get; protected set; } = DateTime.Now;
        public DateTime PersonalSkillExpireTime { get; protected set; } = DateTime.Now;
        public DateTime SupplyExpireTime { get; protected set; } = DateTime.Now;
        public DateTime BankFillExpireTime { get; protected set; } = DateTime.Now;
        public DateTime PartyBufferExpireTime { get; set; } = DateTime.Now;
        public DateTime PartyDefenceExpireTime { get; set; } = DateTime.Now;
        public DateTime PartyRestoreExpireTime { get; set; } = DateTime.Now;
        public DateTime PartyResistanceExpireTime { get; set; } = DateTime.Now;
        public DateTime PartyAutoStrengthExpireTime { get; set; } = DateTime.Now;
        public DateTime PartySkillHealExpireTime { get; protected set; } = DateTime.Now;
        public DateTime PartySkillGroupHealExpireTime { get; protected set; } = DateTime.Now;
        public DateTime PartySkillBufferExpireTime { get; set; } = DateTime.Now;
        public DateTime PartySkillDefenceExpireTime { get; set; } = DateTime.Now;
        public DateTime PartySkillRestoreExpireTime { get; set; } = DateTime.Now;
        public DateTime PartySkillResistanceExpireTime { get; set; } = DateTime.Now;
        public DateTime PartySkillCureExpireTime { get; set; } = DateTime.Now;

        public Thread SupplyPotionThread { get; set; } = null;
        public Thread SupplySundiresThread { get; set; } = null;
        public Thread SupplyBankThread { get; set; } = null;

        public Character Target { get; set; }

        public Character(int id,
            int health,
            int maxHealth,
            bool needCureCurse,
            bool needCureDisease,
            int x,
            int y,
            int z,
            int distance)
        {
            Id = id;
            HexId = Id.ConvertToDword(2);
            Health = health;
            MaxHealth = maxHealth;
            LastHealth = MaxHealth - Health;
            NeedCureCurse = needCureCurse;
            NeedCureDisease = needCureDisease;
            X = x;
            Y = y;
            Z = z;
            Distance = distance;
        }

        public Character(int id,
            string name,
            int level,
            int x,
            int y,
            int z,
            int distance,
            int centerDistance,
            int health,
            int maxHealth,
            int classId,
            CharacterType type,
            CharacterRaceType raceType,
            CharacterClassType classType,
            CharacterClassNameType classNameType,
            CharacterMoveType moveType,
            CharacterStatusType statusType)
        {
            Id = id;
            HexId = id.ConvertToDword(2);
            Name = name;
            Level = level;
            X = x;
            Y = y;
            Z = z;
            Distance = distance;
            CenterDistance = centerDistance;
            Health = health;
            MaxHealth = maxHealth;
            ClassId = classId;
            Type = type;
            RaceType = raceType;
            ClassType = classType;
            ClassNameType = classNameType;
            MoveType = moveType;
            StatusType = statusType;
        }

        public Character(int id, int x, int y, int z)
        {
            Id = id;
            X = x;
            Y = y;
            Z = z;
        }

        public Character(string hexId)
        {
            HexId = hexId;
        }

        public Character(string gameName, bool isMain)
        {
            GameName = gameName;
            IsMain = isMain;

            Handle = MemoryHelper.GetHandle(GameName);

            if (Handle == IntPtr.Zero) throw new Exception("Handle not found");
        }

        #region [Handle]
        public void StartGame()
        {
            if (Handle == IntPtr.Zero) return;

            for (int i = 0; i < 10; i++)
                SendAddress = WinApi.VirtualAllocEx(Handle, IntPtr.Zero, 1024, AllocationType.Commit, MemoryProtection.ExecuteReadWrite);

            RemoteAddress = WinApi.VirtualAllocEx(Handle, IntPtr.Zero, 1024, AllocationType.Commit, MemoryProtection.ExecuteReadWrite);
            TargetBaseAddress = WinApi.VirtualAllocEx(Handle, IntPtr.Zero, 1024, AllocationType.Commit, MemoryProtection.ExecuteReadWrite);
            RecvMailAddress = WinApi.VirtualAllocEx(Handle, IntPtr.Zero, 1024, AllocationType.Commit, MemoryProtection.ExecuteReadWrite);
            RecvHookAddress = WinApi.VirtualAllocEx(Handle, IntPtr.Zero, 1024, AllocationType.Commit, MemoryProtection.ExecuteReadWrite);
            SendMailAddress = WinApi.VirtualAllocEx(Handle, IntPtr.Zero, 1024, AllocationType.Commit, MemoryProtection.ExecuteReadWrite);
        }

        public void UpdateReceiveHookHandle(IntPtr handle)
        {
            ReceiveHandle = handle;
        }

        public void UpdateSendHookHandle(IntPtr handle)
        {
            SendHandle = handle;
        }
        #endregion

        #region [Information]
        public void UpdateInformation(string name, int level, CharacterClassNameType classNameType)
        {
            Name = name;
            Level = level;
            ClassNameType = classNameType;
        }

        public void UpdateInformation(int id,
            string name,
            int level,
            int x,
            int y,
            int z,
            int health,
            int maxHealth,
            int mana,
            int maxMana,
            int classId,
            CharacterRaceType raceType,
            CharacterClassType classType,
            CharacterClassNameType classNameType)
        {
            Id = id;
            HexId = id.ConvertToDword(2);
            Name = name;
            Level = level;
            X = x;
            Y = y;
            Z = z;
            Health = health;
            MaxHealth = maxHealth;
            Mana = mana;
            MaxMana = maxMana;
            ClassId = classId;
            RaceType = raceType;
            ClassType = classType;
            ClassNameType = classNameType;

            LastHealth = MaxHealth - Health;
            LastMana = MaxMana - Mana;
        }

        public void UpdateInformation(
            int distance,
            bool needCureCurse,
            bool needCureDisease,
            DateTime partyBufferExpireTime,
            DateTime partyDefenceExpireTime,
            DateTime partyRestoreExpireTime,
            DateTime partyResistanceExpireTime,
            DateTime partyAutoStrengthExpireTime)
        {
            Distance = distance;
            NeedCureCurse = needCureCurse;
            NeedCureDisease = needCureDisease;
            PartyBufferExpireTime = partyBufferExpireTime;
            PartyDefenceExpireTime = partyDefenceExpireTime;
            PartyRestoreExpireTime = partyRestoreExpireTime;
            PartyResistanceExpireTime = partyResistanceExpireTime;
            PartyAutoStrengthExpireTime = partyAutoStrengthExpireTime;
        }

        public void UpdateInformation(int id,
            int health,
            int maxHealth,
            bool needCureCurse,
            bool needCureDisease,
            int x,
            int y,
            int z,
            int distance,
            DateTime partyBufferExpireTime,
            DateTime partyDefenceExpireTime,
            DateTime partyRestoreExpireTime,
            DateTime partyResistanceExpireTime,
            DateTime partyAutoStrengthExpireTime)
        {
            Id = id;
            HexId = Id.ConvertToDword(2);
            Health = health;
            MaxHealth = maxHealth;
            LastHealth = MaxHealth - Health;
            NeedCureCurse = needCureCurse;
            NeedCureDisease = needCureDisease;
            X = x;
            Y = y;
            Z = z;
            Distance = distance;
            PartyBufferExpireTime = partyBufferExpireTime;
            PartyDefenceExpireTime = partyDefenceExpireTime;
            PartyRestoreExpireTime = partyRestoreExpireTime;
            PartyResistanceExpireTime = partyResistanceExpireTime;
            PartyAutoStrengthExpireTime = partyAutoStrengthExpireTime;
        }

        public void UpdateItems(Item[] items)
        {
            InventoryItems = items;
        }

        public void UpdateBankItems(Item[] bankItems)
        {
            BankItems = bankItems;
        }

        public void UpdateUsedSkills(Skill[] usedSkills)
        {
            UsedSkills = usedSkills;
        }

        public void UpdateTargets(Character[] targets)
        {
            Targets = targets;
        }

        public void UpdateTarget(Character target)
        {
            Target = target;
        }

        public void UpdateIsBoxCollector(bool isBoxCollector)
        {
            IsBoxCollector = isBoxCollector;
        }

        public void UpdateAttackType(CharacterAttackType attackType)
        {
            AttackType = attackType;
        }

        public void UpdateSupplyAction(bool isSupplyAction)
        {
            IsSupplyAction = isSupplyAction;
            SupplyActionRow = isSupplyAction ? 0 : -1;
        }

        public void UpdateSupplyActionRow()
        {
            SupplyActionRow++;
        }

        public void UpdateSupplyActionComplete()
        {
            SupplySundiresThread = null;
            SupplyPotionThread = null;
            IsSupplyAction = false;
            UpdateSupplyExpireTime(5000);
        }

        public void UpdateBankFillActionRow(int row)
        {
            BankFillActionRow = row;
        }

        public void UpdateBankFillActionRowIncrease()
        {
            BankFillActionRow++;
        }

        public void UpdateBankFillActionReset()
        {
            SupplyBankThread = null;
            SupplyPotionThread = null;
        }
        #endregion

        #region [Timing]
        public void UpdateAvailableExpireTime(int miliseconds = 18000)
        {
            AvailableExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdateLastTargetAttackExpireTime(int miliseconds = 2000)
        {
            LastTargetAttackExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdateDeathTime(DateTime? dateTime)
        {
            DeathTime = dateTime;
        }

        public void UpdateGlobalExpireTime(int miliseconds = 2000)
        {
            GlobalExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdateEternityExpireTime(int miliseconds = 2000)
        {
            EternityExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdateMagicHammerExpireTime(int miliseconds = 2000)
        {
            MagicHammerExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdateBufferScrollExpireTime(int miliseconds = 1000)
        {
            BufferScollExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdateTransformationScrollExpireTime(int miliseconds = 1000)
        {
            TransformationScrollExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdateFollowExpireTime(int miliseconds = 100)
        {
            FollowExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdateWalkExpireTime(int miliseconds = 2000)
        {
            WalkExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdateTargetSelectExpireTime(int miliseconds = 1000)
        {
            TargetSelectExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdateAttackSkillExpireTime()
        {
            switch (ClassType)
            {
                case CharacterClassType.Warrior:
                    AttackSkillExpireTime = DateTime.Now.AddMilliseconds(1050);
                    break;
                case CharacterClassType.Rogue:
                    if (AttackType == CharacterAttackType.Archery)
                    {
                        AttackSkillExpireTime = DateTime.Now.AddMilliseconds(1320);
                    }
                    else
                    {
                        AttackSkillExpireTime = DateTime.Now.AddMilliseconds(1050);
                    }
                    break;
                case CharacterClassType.Magician:
                    AttackSkillExpireTime = DateTime.Now.AddMilliseconds(1150);
                    break;
                case CharacterClassType.Priest:
                    AttackSkillExpireTime = DateTime.Now.AddMilliseconds(1050);
                    break;
                case CharacterClassType.Kurian:
                    AttackSkillExpireTime = DateTime.Now.AddMilliseconds(1050);
                    break;
            }
        }

        public void UpdatePersonalSkillExpireTime(int miliseconds = 2000)
        {
            PersonalSkillExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdateRegularSkillExpireTime(int miliseconds = 910)
        {
            RegularSkillExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdateSupplyExpireTime(int miliseconds = 1000)
        {
            SupplyExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdateBankFillExpireTime(int miliseconds = 1000)
        {
            BankFillExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdatePartyBufferExpireTime(int miliseconds = 1000 * 60 * 10 + 1000)
        {
            PartyBufferExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdatePartyDefenceExpireTime(int miliseconds = 1000 * 60 * 10 + 1000)
        {
            PartyDefenceExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdatePartyRestoreExpireTime(int miliseconds = 1000 * 25 + 1000)
        {
            PartyRestoreExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdatePartyResistanceExpireTime(int miliseconds = 1000 * 60 * 10 + 1000)
        {
            PartyResistanceExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdatePartyAutoStrengthExpireTime(int miliseconds = 1000 * 60 * 10 + 1000)
        {
            PartyAutoStrengthExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdatePartySkillHealExpireTime(int miliseconds = 2000)
        {
            PartySkillHealExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdatePartySkillGroupHealExpireTime(int miliseconds = 2000)
        {
            PartySkillGroupHealExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdatePartySkillBufferExpireTime(int miliseconds = 2000)
        {
            PartySkillBufferExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdatePartySkillDefenceExpireTime(int miliseconds = 2000)
        {
            PartySkillDefenceExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdatePartySkillRestoreExpireTime(int miliseconds = 2000)
        {
            PartySkillRestoreExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdatePartySkillResistanceExpireTime(int miliseconds = 2000)
        {
            PartySkillResistanceExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void UpdatePartySkillCureExpireTime(int miliseconds = 2000)
        {
            PartySkillCureExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }
        #endregion

        #region [Utility]
        public string GetSkillCoor()
        {
            return $"{X.ConvertToDword(2)}{Z.ConvertToDword(2)}{Y.ConvertToDword(2)}";
        }

        public bool GetHealthLimit(int limit)
        {
            return Health <= (MaxHealth * limit / 100);
        }

        public bool GetManaLimit(int limit)
        {
            return Mana <= (MaxMana * limit / 100);
        }

        public bool GetUndyLimit(int limit)
        {
            return limit > 0 && limit <= (MaxHealth * 60 / 100);
        }
        #endregion
    }

    public enum CharacterType : byte
    {
        NonPlayerCharacter,
        Player
    }

    public enum CharacterAttackType : byte
    {
        Melee,
        Archery
    }

    public enum CharacterRaceType : byte
    {
        Monster,
        Karus,
        Human,
        All
    }

    public enum CharacterClassType : byte
    {
        None = 0,
        Warrior = 1,
        Rogue = 2,
        Magician = 3,
        Priest = 4,
        Kurian = 5,
        GM = 255,
    }

    public enum CharacterClassNameType : byte
    {
        [Display(Name = "Unknown")]
        None,

        [Display(Name = "Warrior")]
        Warrior,
        [Display(Name = "Warrior")]
        KnightWarrior,
        [Display(Name = "Warrior")]
        MasterWarrior,

        [Display(Name = "Rogue")]
        Rogue,
        [Display(Name = "Rogue")]
        KnightRogue,
        [Display(Name = "Rogue")]
        MasterRogue,

        [Display(Name = "Magician")]
        Magician,
        [Display(Name = "Magician")]
        KnightMagician,
        [Display(Name = "Magician")]
        MasterMagician,

        [Display(Name = "Priest")]
        Priest,
        [Display(Name = "Priest")]
        KnightPriest,
        [Display(Name = "Priest")]
        MasterPriest,

        [Display(Name = "Kurian")]
        Kurian,
        [Display(Name = "Kurian")]
        KnightKurian,
        [Display(Name = "Kurian")]
        MasterKurian
    }

    public enum CharacterMoveType : byte
    {
        Unknown,
        Normal,
        Attack,
        Dead,
        Type4,
        Remove
    }

    public enum CharacterWalkType
    {
        Walk,
        Jump,
        Teleport
    }

    public enum CharacterStatusType : byte
    {
        Normal,
        Moving,
        Running,
        Attack,
        Dead,
        Unknown
    }
}