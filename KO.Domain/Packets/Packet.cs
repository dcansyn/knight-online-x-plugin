using KO.Core.Extensions;
using System.ComponentModel.DataAnnotations;

namespace KO.Domain.Packets
{
    public class Packet
    {
        public string Code { get; protected set; }
        public byte Value { get; protected set; }
        public PacketType Type { get; protected set; }
        public string Name { get; protected set; }
        public PacketStatusType Status { get; protected set; }

        public Packet(string code, byte value, PacketType type, PacketStatusType status)
        {
            Code = code;
            Value = value;
            Type = type;
            Status = status;

            Name = Type.Get().DisplayName;
        }
    }

    public enum PacketStatusType : byte
    {
        Send,
        Receive
    }

    public enum PacketType : byte
    {
        [Display(Name = "Unknown", Order = 000)]
        Unknown = 0,
        [Display(Name = "Login", Order = 100)]
        Login = 1,
        [Display(Name = "New Character", Order = 200)]
        NewCharacter = 2,
        [Display(Name = "Delete Character", Order = 300)]
        DeleteCharacter = 3,
        [Display(Name = "Sel Character", Order = 400)]
        SelCharacter = 4,
        [Display(Name = "Sel Nation", Order = 500)]
        SelNation = 5,
        [Display(Name = "Move", Order = 600)]
        Move = 6,
        [Display(Name = "User In Out", Order = 700)]
        UserInOut = 7,
        [Display(Name = "Attack", Order = 800)]
        Attack = 8,
        [Display(Name = "Rotate", Order = 900)]
        Rotate = 9,
        [Display(Name = "NPC In Out", Order = 1000)]
        NpcInOut = 10,
        [Display(Name = "NPC Move", Order = 1100)]
        NpcMove = 11,
        [Display(Name = "All Character Info Request", Order = 1200)]
        AllCharacterInfoReq = 12,
        [Display(Name = "Game Start", Order = 1300)]
        GameStart = 13,
        [Display(Name = "My Info", Order = 1400)]
        MyInfo = 14,
        [Display(Name = "Logout", Order = 1500)]
        Logout = 15,
        [Display(Name = "Chat", Order = 1600)]
        Chat = 16,
        [Display(Name = "Dead", Order = 1700)]
        Dead = 17,
        [Display(Name = "Regene", Order = 1800)]
        Regene = 18,
        [Display(Name = "Time", Order = 1900)]
        Time = 19,
        [Display(Name = "Weather", Order = 2000)]
        Weather = 20,
        [Display(Name = "Region Change", Order = 2100)]
        RegionChange = 21,
        [Display(Name = "Request User In", Order = 2200)]
        ReqUserIn = 22,
        [Display(Name = "Health Change", Order = 2300)]
        HealthChange = 23,
        [Display(Name = "MSP Change", Order = 2400)]
        MspChange = 24,
        [Display(Name = "Item Log", Order = 2500)]
        ItemLog = 25,
        [Display(Name = "Exp Change", Order = 2600)]
        ExpChange = 26,
        [Display(Name = "Level Change", Order = 2700)]
        LevelChange = 27,
        [Display(Name = "NPC Region", Order = 2800)]
        NpcRegion = 28,
        [Display(Name = "Request NPC In", Order = 2900)]
        ReqNpcIn = 29,
        [Display(Name = "Warp", Order = 3000)]
        Warp = 30,
        [Display(Name = "Item Move", Order = 3100)]
        ItemMove = 31,
        [Display(Name = "NPC Event", Order = 3200)]
        NpcEvent = 32,
        [Display(Name = "Item Trade", Order = 3300)]
        ItemTrade = 33,
        [Display(Name = "Target Health", Order = 3400)]
        TargetHealth = 34,
        [Display(Name = "Item Drop", Order = 3500)]
        ItemDrop = 35,
        [Display(Name = "Box Open Request", Order = 3600)]
        BoxOpenReq = 36,
        [Display(Name = "Trade NPC", Order = 3700)]
        TradeNpc = 37,
        [Display(Name = "Collect Box", Order = 3750)]
        CollectBox = 38,
        [Display(Name = "State Change", Order = 3800)]
        StateChange = 41,
        [Display(Name = "User Look Change", Order = 3900)]
        UserLookChange = 45,
        [Display(Name = "Notice", Order = 4000)]
        Notice = 46,
        [Display(Name = "Party", Order = 4100)]
        Party = 47,
        [Display(Name = "Magic Process", Order = 4200)]
        MagicProcess = 49,
        [Display(Name = "Object Event", Order = 4300)]
        ObjectEvent = 51,
        [Display(Name = "NPC Repair", Order = 4400)]
        NpcRepair = 58,
        [Display(Name = "Item Repair", Order = 4500)]
        ItemRepair = 59,
        [Display(Name = "Knight Process", Order = 4600)]
        KnightProcess = 60,
        [Display(Name = "Item Remove", Order = 4700)]
        ItemRemove = 63,
        [Display(Name = "Compress Packet", Order = 4800)]
        CompressPacket = 66,
        [Display(Name = "Bank Trade", Order = 4900)]
        BankInOut = 69,
        [Display(Name = "Friend Report", Order = 5000)]
        FriendReport = 73,
        [Display(Name = "Area Change", Order = 5010)]
        AreaChange = 74,
        [Display(Name = "Gate", Order = 5020)]
        Gate = 75,
        [Display(Name = "Weight Change", Order = 5100)]
        WeightChange = 84,
        [Display(Name = "Select Message", Order = 5200)]
        SelectMessage = 85,
        [Display(Name = "Upgrade", Order = 5300)]
        Upgrade = 91,
        [Display(Name = "Quest", Order = 5400)]
        Quest = 100,
        [Display(Name = "Merchant", Order = 5500)]
        Merchant = 104,
        [Display(Name = "Item Refresh", Order = 5600)]
        ItemRefresh = 106,
        [Display(Name = "GameCaptcha", Order = 5600)]
        GameCaptcha = 192
    }

    public enum GateType
    {
        [Display(Name = "Delos > AbyssDungeon", GroupName = "Delos", ShortName = "Karus,Human", Description = "4034", Prompt = "3055")]
        DelosAbyssDungeon,
        [Display(Name = "Delos > Moradon", GroupName = "Delos", ShortName = "Karus", Description = "4030", Prompt = "3014")]
        KarusDelosMoradon,
        [Display(Name = "Delos > Moradon", GroupName = "Delos", ShortName = "Human", Description = "4029", Prompt = "3024")]
        HumanDelosMoradon,

        [Display(Name = "Moradon > Folk", GroupName = "Moradon", ShortName = "Karus", Description = "4014", Prompt = "2111")]
        MoradonKarusFolk,
        [Display(Name = "Moradon > Tale", GroupName = "Moradon", ShortName = "Karus", Description = "4014", Prompt = "2112")]
        MoradonKarusTale,
        [Display(Name = "Moradon > Luferson", GroupName = "Moradon", ShortName = "Karus", Description = "4014", Prompt = "2113")]
        MoradonLuferson,
        [Display(Name = "Moradon > Lunar Valley", GroupName = "Moradon", ShortName = "Karus", Description = "4014", Prompt = "2114")]
        MoradonKarusLunarValley,
        [Display(Name = "Moradon > Delos", GroupName = "Moradon", ShortName = "Karus", Description = "4014", Prompt = "2115")]
        MoradonKarusDelos,
        [Display(Name = "Moradon > Ronark Land Base", GroupName = "Moradon", ShortName = "Karus", Description = "4014", Prompt = "2117")]
        MoradonKarusRonarkLandBase,
        [Display(Name = "Moradon > Ronark Land", GroupName = "Moradon", ShortName = "Karus", Description = "4014", Prompt = "2118")]
        MoradonKarusRonarkLand,

        [Display(Name = "Moradon > Folk", GroupName = "Moradon", ShortName = "Human", Description = "4013", Prompt = "2121")]
        MoradonElMoradFolk,
        [Display(Name = "Moradon > Tale", GroupName = "Moradon", ShortName = "Human", Description = "4013", Prompt = "2122")]
        MoradonElMoradTale,
        [Display(Name = "Moradon > El Morad", GroupName = "Moradon", ShortName = "Human", Description = "4013", Prompt = "2123")]
        MoradonElMorad,
        [Display(Name = "Moradon > Lunar Valley", GroupName = "Moradon", ShortName = "Human", Description = "4013", Prompt = "2124")]
        MoradonElMoradLunarValley,
        [Display(Name = "Moradon > Delos", GroupName = "Moradon", ShortName = "Human", Description = "4013", Prompt = "2125")]
        MoradonElMoradDelos,
        [Display(Name = "Moradon > Ronark Land Base", GroupName = "Moradon", ShortName = "Human", Description = "4013", Prompt = "2127")]
        MoradonElMoradRonarkLandBase,
        [Display(Name = "Moradon > Ronark Land", GroupName = "Moradon", ShortName = "Human", Description = "4013", Prompt = "2128")]
        MoradonElMoradRonarkLand,

        [Display(Name = "Luferson > Moradon", GroupName = "Luferson", ShortName = "Karus", Description = "4006", Prompt = "111")]
        LufersonMoradon,
        [Display(Name = "Luferson > Ronark Land", GroupName = "Luferson", ShortName = "Karus", Description = "4006", Prompt = "118")]
        LufersonRonarkLand,
        [Display(Name = "Luferson > Ronark Land Base", GroupName = "Luferson", ShortName = "Karus", Description = "4006", Prompt = "117")]
        LufersonRonarkLandBase,
        [Display(Name = "Luferson > Belluga", GroupName = "Luferson", ShortName = "Karus", Description = "4006", Prompt = "112")]
        LufersonBelluga,
        [Display(Name = "Luferson > Kalluga", GroupName = "Luferson", ShortName = "Karus", Description = "4006", Prompt = "213")]
        LufersonKalluga,
        [Display(Name = "Luferson > Roan Camp", GroupName = "Luferson", ShortName = "Karus", Description = "4006", Prompt = "114")]
        LufersonRoan,
        [Display(Name = "Luferson > Eslant Entrance", GroupName = "Luferson", ShortName = "Karus", Description = "4006", Prompt = "115")]
        LufersonEslant,
        [Display(Name = "Eslant Entrance > Linart", GroupName = "Luferson", ShortName = "Karus", Description = "4002", Prompt = "152")]
        LufersonLinart,
        [Display(Name = "Eslant Entrance > Eslant", GroupName = "Luferson", ShortName = "Karus", Description = "4002", Prompt = "153")]
        LufersonEslantEntranceEslant,

        [Display(Name = "El Morad > Moradon", GroupName = "El Morad", ShortName = "Human", Description = "4003", Prompt = "211")]
        ElMoradMoradon,
        [Display(Name = "El Morad > Ronark Land", GroupName = "El Morad", ShortName = "Human", Description = "4003", Prompt = "218")]
        ElMoradRonarkLand,
        [Display(Name = "El Morad > Ronark Land Base", GroupName = "El Morad", ShortName = "Human", Description = "4003", Prompt = "217")]
        ElMoradRonarkLandBase,
        [Display(Name = "El Morad > Asga", GroupName = "El Morad", ShortName = "Human", Description = "4003", Prompt = "212")]
        ElMoradAsga,
        [Display(Name = "El Morad > Kalluga", GroupName = "El Morad", ShortName = "Human", Description = "4003", Prompt = "214")]
        ElMoradKalluga,
        [Display(Name = "El Morad > Doda", GroupName = "El Morad", ShortName = "Human", Description = "4003", Prompt = "213")]
        ElMoradDoda,
        [Display(Name = "El Morad > Eslant Entrance", GroupName = "El Morad", ShortName = "Human", Description = "4003", Prompt = "215")]
        ElMoradEslant,
        [Display(Name = "Eslant Entrance > Labia", GroupName = "El Morad", ShortName = "Human", Description = "4001", Prompt = "252")]
        ElMoradLabia,
        [Display(Name = "Eslant Entrance > Eslant", GroupName = "El Morad", ShortName = "Human", Description = "4001", Prompt = "253")]
        ElMoradEslantEntranceEslant,

        [Display(Name = "Ronark Land > Moradon", GroupName = "Ronark Land", ShortName = "Karus", Description = "4020", Prompt = "7114")]
        RonarkLandKarusMoradon,
        [Display(Name = "Ronark Land > Luferson", GroupName = "Ronark Land", ShortName = "Karus", Description = "4020", Prompt = "7111")]
        RonarkLandKarusLuferson,
        [Display(Name = "Ronark Land > Moradon", GroupName = "Ronark Land", ShortName = "Human", Description = "0000", Prompt = "7124")]
        RonarkLandHumanMoradon,
        [Display(Name = "Ronark Land > El Morad", GroupName = "Ronark Land", ShortName = "Human", Description = "0000", Prompt = "7121")]
        RonarkLandHumanElMorad,

        [Display(Name = "Ronark Land Base > Moradon", GroupName = "Ronark Land", ShortName = "Karus", Description = "4020", Prompt = "7314")]
        RonarkLandBaseKarusMoradon,
        [Display(Name = "Ronark Land Base > Luferson", GroupName = "Ronark Land", ShortName = "Karus", Description = "4020", Prompt = "7311")]
        RonarkLandBaseKarusLuferson,
        [Display(Name = "Ronark Land Base > Moradon", GroupName = "Ronark Land", ShortName = "Human", Description = "0000", Prompt = "7324")]
        RonarkLandBaseHumanMoradon,
        [Display(Name = "Ronark Land Base > El Morad", GroupName = "Ronark Land", ShortName = "Human", Description = "0000", Prompt = "7321")]
        RonarkLandBaseHumanElMorad,
    }
}
