using System.ComponentModel.DataAnnotations;

namespace KO.Domain.Parties
{
    public class Party
    {
        public int HealPercent { get; protected set; }
        public HealSkillType HealSkillType { get; protected set; }
        public BufferSkillType BufferSkillType { get; protected set; }
        public DefenceSkillType DefenceSkillType { get; protected set; }
        public RestoreSkillType RestoreSkillType { get; protected set; }
        public ResistanceSkillType ResistanceSkillType { get; protected set; }
        public bool AutoEternity { get; protected set; }
        public bool AutoCurseCure { get; protected set; }
        public bool AutoDiseaseCure { get; protected set; }
        public bool GroupHeal { get; protected set; }
        public int GroupHealCount { get; protected set; }
        public int GroupHealPercent { get; protected set; }
        public bool AutoPartyAccept { get; protected set; }
        public string[] AutoPartyFriendList { get; protected set; }

        public Party(int healPercent,
            HealSkillType healSkillType,
            BufferSkillType bufferSkillType,
            DefenceSkillType defenceSkillType,
            RestoreSkillType restoreSkillType,
            ResistanceSkillType resistanceSkillType,
            bool autoEternity,
            bool autoCurseCure,
            bool autoDiseaseCure,
            bool groupHeal,
            int groupHealCount,
            int groupHealPercent,
            bool autoPartyAccept,
            string[] autoPartyFriendList)
        {
            HealPercent = healPercent;
            HealSkillType = healSkillType;
            BufferSkillType = bufferSkillType;
            DefenceSkillType = defenceSkillType;
            RestoreSkillType = restoreSkillType;
            ResistanceSkillType = resistanceSkillType;
            AutoEternity = autoEternity;
            AutoCurseCure = autoCurseCure;
            AutoDiseaseCure = autoDiseaseCure;
            GroupHeal = groupHeal;
            GroupHealCount = groupHealCount;
            GroupHealPercent = groupHealPercent;
            AutoPartyAccept = autoPartyAccept;
            AutoPartyFriendList = autoPartyFriendList;
        }
    }

    public enum HealSkillType
    {
        [Display(Name = "Closed", GroupName = "closed", Order = 0)]
        Closed,
        [Display(Name = "Automatic", GroupName = "auto", Order = 0)]
        Automatic,
        [Display(Name = "Minor Healing (60)", GroupName = "500", Order = 60)]
        MinorHealing,
        [Display(Name = "Healing (240)", GroupName = "509", Order = 240)]
        Healing,
        [Display(Name = "Major Healing (360)", GroupName = "518", Order = 360)]
        MajorHealing,
        [Display(Name = "Great Healing (720)", GroupName = "527", Order = 720)]
        GreathHealing,
        [Display(Name = "Massive Healing (960)", GroupName = "536", Order = 960)]
        MassiveHealing,
        [Display(Name = "Superior Healing (1.920)", GroupName = "545", Order = 1920)]
        SuperiorHealing,
        [Display(Name = "Complete Healing (10.000)", GroupName = "554", Order = 2500)]
        CompleteHealing,
    }

    public enum BufferSkillType
    {
        [Display(Name = "Closed", GroupName = "closed", Order = 0)]
        Closed,
        [Display(Name = "Automatic", GroupName = "auto", Order = 0)]
        Automatic,
        [Display(Name = "Grace (60)", GroupName = "606", Order = 60)]
        Grace,
        [Display(Name = "Brave (240)", GroupName = "615", Order = 240)]
        Brave,
        [Display(Name = "Strong (360)", GroupName = "624", Order = 360)]
        Strong,
        [Display(Name = "Hardness (720)", GroupName = "633", Order = 720)]
        Hardness,
        [Display(Name = "Mightness (960)", GroupName = "642", Order = 960)]
        Mightness,
        [Display(Name = "Undying (%60)", GroupName = "654", Order = 0)]
        Undying,
        [Display(Name = "Heapness (1.200)", GroupName = "655", Order = 1200)]
        Heapness,
        [Display(Name = "Massiveness (1.500)", GroupName = "657", Order = 1500)]
        Massiveness,
        [Display(Name = "Imposingness (2.000)", GroupName = "670", Order = 2000)]
        Imposingness,
        [Display(Name = "Superioris (2.500)", GroupName = "678", Order = 2500)]
        Superioris
    }

    public enum DefenceSkillType
    {
        [Display(Name = "Closed", GroupName = "closed", Order = 0)]
        Closed,
        [Display(Name = "Automatic", GroupName = "auto", Order = 0)]
        Automatic,
        [Display(Name = "Skin (20)", GroupName = "603", Order = 20)]
        Skin,
        [Display(Name = "Shell (40)", GroupName = "612", Order = 40)]
        Shell,
        [Display(Name = "Armor (80)", GroupName = "621", Order = 80)]
        Armor,
        [Display(Name = "Shield (120)", GroupName = "630", Order = 120)]
        Shield,
        [Display(Name = "Barrier (160)", GroupName = "639", Order = 160)]
        Barrier,
        [Display(Name = "Protector (200)", GroupName = "651", Order = 200)]
        Protector,
        [Display(Name = "Peel (300)", GroupName = "660", Order = 300)]
        Peel,
        [Display(Name = "Guard (350)", GroupName = "674", Order = 350)]
        Guard
    }

    public enum RestoreSkillType
    {
        [Display(Name = "Closed", GroupName = "closed", Order = 0)]
        Closed,
        [Display(Name = "Automatic", GroupName = "auto", Order = 0)]
        Automatic,
        [Display(Name = "Light Restore (100)", GroupName = "503", Order = 100)]
        LightRestore,
        [Display(Name = "Restore (400)", GroupName = "512", Order = 400)]
        Restore,
        [Display(Name = "Major Restore (600)", GroupName = "521", Order = 600)]
        MajorRestore,
        [Display(Name = "Great Restore (800)", GroupName = "530", Order = 800)]
        GreatRestore,
        [Display(Name = "Massive Restore (1.500)", GroupName = "539", Order = 1500)]
        MassiveRestore,
        [Display(Name = "Superior Restore (2.500)", GroupName = "548", Order = 2500)]
        SuperiorRestore,
        [Display(Name = "Critical Restore (3.000)", GroupName = "570", Order = 3000)]
        CriticalRestore,
        [Display(Name = "Past Recovery (2.500)", GroupName = "575", Order = 2500)]
        PastRecovery,
        [Display(Name = "Past Restore (6.000)", GroupName = "580", Order = 6000)]
        PastRestore
    }

    public enum ResistanceSkillType
    {
        [Display(Name = "Closed", GroupName = "closed", Order = 0)]
        Closed,
        [Display(Name = "Automatic", GroupName = "auto", Order = 0)]
        Automatic,
        [Display(Name = "Resist All (20)", GroupName = "609", Order = 20)]
        ResistAll,
        [Display(Name = "Bright Mind (40)", GroupName = "627", Order = 40)]
        BrightMind,
        [Display(Name = "Calm Mind (60)", GroupName = "636", Order = 60)]
        CalmMind,
        [Display(Name = "Fresh Mind (80)", GroupName = "645", Order = 80)]
        FreshMind
    }

    public enum GroupHealSkillType
    {
        [Display(Name = "Group Massive Healing (960)", GroupName = "557", Order = 960)]
        GroupMassiveHealing,
        [Display(Name = "Group Complete Healing (10.000)", GroupName = "560", Order = 2500)]
        GroupCompleteHealing
    }
}