using System.Configuration;

namespace KO.Core.Constants
{
    public class Settings
    {
        public static string Name = ConfigurationManager.AppSettings["ApplicationName"];
        public static string GameDefaultTitle = ConfigurationManager.AppSettings["GameDefaultTitle"];
        public static string GameDefaultName = ConfigurationManager.AppSettings["GameDefaultName"];

        public static int KO_PTR_CHR = 0x10131C0;
        public static int KO_PTR_DLG = 0x100D654;
        public static int KO_PTR_PKT = 0x100D620;
        public static int KO_PTR_SND = 0x4BD350;
        public static int KO_PTR_FMBS = 0x546510;
        public static int KO_PTR_FPBS = 0x5478C0;
        public static int KO_PTR_FLDB = 0x10131BC;
        public static int KO_PTR_FAKE_ITEM = 0x5CB5B0;
        public static int KO_PTR_DESTROY_1 = 0x100E87C;
        public static int KO_PTR_DESTROY_2 = 0x100E810;
        public static int KO_PTR_DESTROY_FNC = 0x63E480;

        public static int KO_OFF_ID = 0x680;
        public static int KO_OFF_CLASS = 0x6B0;
        public static int KO_OFF_NT = 0x6A8;
        public static int KO_OFF_WH = 0x6C0;
        public static int KO_OFF_MOB = 0x644;
        public static int KO_OFF_ZONE = 0xBE0;
        public static int KO_OFF_TARGET_STATU = 0x2A0;
        public static int KO_OFF_TARGET_MOVE = 0x3E4;
        public static int KO_OFF_MOVE = 0xF88;
        public static int KO_OFF_MOVE_TYPE = 0x3F0;
        public static int KO_OFF_BASE_TARGET = 0x34;
        public static int KO_OFF_BASE_PLAYER = 0x40;

        public static int KO_OFF_X = 0xD8;
        public static int KO_OFF_Y = 0xE0;
        public static int KO_OFF_Z = 0xDC;

        public static int KO_OFF_MOUSE_X = 0xF94;
        public static int KO_OFF_MOUSE_Y = 0xF9C;
        public static int KO_OFF_MOUSE_Z = 0xF98;

        public static int KO_OFF_TARGET_COR_BASE = 0x408;
        public static int KO_OFF_TARGET_X = 0xB8;
        public static int KO_OFF_TARGET_Y = 0xC0;
        public static int KO_OFF_TARGET_Z = 0xBC;

        public static int KO_OFF_EXP = 0xB58;
        public static int KO_OFF_MAX_EXP = 0xB50;
        public static int KO_OFF_GOLD = 0xB48;
        public static int KO_OFF_LEVEL = 0x6B4;
        public static int KO_OFF_ATTACK = 0xB9C;
        public static int KO_OFF_DEFENCE = 0xBA4;
        public static int KO_OFF_WEIGHT = 0xB70;
        public static int KO_OFF_MAX_WEIGHT = 0xB68;

        public static int KO_OFF_NAME = 0x688;
        public static int KO_OFF_NAME_LENGTH = 0x698;
        public static int KO_OFF_MP = 0xB3C;
        public static int KO_OFF_MAX_MP = 0xB38;
        public static int KO_OFF_HP = 0x6BC;
        public static int KO_OFF_MAX_HP = 0x6B8;

        public static int KO_OFF_STAT_POINT = 0xB30;
        public static int KO_OFF_STAT_STR = 0xB74;
        public static int KO_OFF_STAT_HP = 0xB7C;
        public static int KO_OFF_STAT_DEX = 0xB84;
        public static int KO_OFF_STAT_INT = 0xB8C;
        public static int KO_OFF_STAT_MP = 0xB94;

        public static int KO_OFF_SKILL_BASE = 0x1E8;
        public static int KO_OFF_SKILL_POINT = 0x16C;
        public static int KO_OFF_SKILL_TAB1 = 0x180;
        public static int KO_OFF_SKILL_TAB2 = 0x184;
        public static int KO_OFF_SKILL_TAB3 = 0x188;
        public static int KO_OFF_SKILL_TAB4 = 0x18C;

        public static int KO_OFF_ITEM_BASE = 0x1B4;
        public static int KO_OFF_ITEM_ROW_BASE = 0x20C;

        public static int KO_OFF_BANK_OPEN = 0xFC;
        public static int KO_OFF_BANK_BASE = 0x204;
        public static int KO_OFF_BANK_ROW_BASE = 0x128;

        public static int KO_OFF_USE_SKILL_BASE = 0x1CC;
        public static int KO_OFF_USE_SKILL_ID = 0x12C;

        public static int KO_OFF_ITEM_ID = 0x68;
        public static int KO_OFF_ITEM_EXTENSION = 0x6C;
        public static int KO_OFF_ITEM_COUNT = 0x70;
        public static int KO_OFF_ITEM_DURABILITY = 0x74;
        public static int KO_OFF_ITEM_MAX_DURABILITY = 0x74;
        public static int KO_OFF_ITEM_NAME_LENGTH = 0x1C;
        public static int KO_OFF_ITEM_NAME = 0xC;

        public static int KO_OFF_PARTY_BASE = 0x1E4;
        public static int KO_OFF_PARTY_COUNT = 0x300;
        public static int KO_OFF_PARTY_USER_BASE = 0x2FC;
        public static int KO_OFF_PARTY_ID = 0x8;
        public static int KO_OFF_PARTY_CLASS = 0x10;
        public static int KO_OFF_PARTY_HP = 0x14;
        public static int KO_OFF_PARTY_MAX_HP = 0x18;
        public static int KO_OFF_PARTY_CURE = 0x24;

        public static int KO_OFF_ITEM_EXTENSION_COUNT = 45;
        public static int KO_OFF_SKILL_EXTENSION_COUNT = 9;
        public static int KO_OFF_INVENTORY_START_ROW = 14;
        public static int KO_OFF_INVENTORY_COUNT = 42;
        public static int KO_OFF_BANK_ROW_COUNT = 192;
        public static int KO_OFF_BANK_PAGE_COUNT = 24;
    }
}
