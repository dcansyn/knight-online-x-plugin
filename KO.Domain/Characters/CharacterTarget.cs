using System;
using System.ComponentModel.DataAnnotations;

namespace KO.Domain.Characters
{
    public class CharacterTarget
    {
        public SelectTargetStatusType SelectTargetStatusType { get; protected set; }
        public SelectTargetType SelectTargetType { get; protected set; }
        public SelectTargetRaceType SelectTargetRaceType { get; protected set; }
        public bool IsTargetRun { get; protected set; }
        public bool IsTargetRunBackToCenter { get; protected set; }
        public int TargetRunCenterX { get; protected set; }
        public int TargetRunCenterY { get; protected set; }
        public int TargetDistance { get; protected set; }
        public bool IsTargetRunWhenTargetDead { get; protected set; }
        public CharacterWalkType TargetRunWalkType { get; protected set; }
        public string[] TargetNameList { get; protected set; }
        public CharacterType[] TargetTypes { get; protected set; }
        public CharacterRaceType[] TargetRaceTypes { get; protected set; }
        public DateTime TargetExpireTime { get; protected set; } = DateTime.Now;

        public CharacterTarget(SelectTargetStatusType selectTargetStatusType,
            SelectTargetType selectTargetType,
            SelectTargetRaceType selectTargetRaceType,
            bool isTargetRun,
            bool isTargetRunBackToCenter,
            int targetRunCenterX,
            int targetRunCenterY,
            int targetDistance,
            bool isTargetRunWhenTargetDead,
            CharacterWalkType targetRunWalkType,
            string[] targetNameList)
        {
            SelectTargetStatusType = selectTargetStatusType;
            SelectTargetType = selectTargetType;
            SelectTargetRaceType = selectTargetRaceType;
            IsTargetRun = isTargetRun;
            IsTargetRunBackToCenter = isTargetRunBackToCenter;
            TargetRunCenterX = targetRunCenterX;
            TargetRunCenterY = targetRunCenterY;
            TargetDistance = targetDistance;
            IsTargetRunWhenTargetDead = isTargetRunWhenTargetDead;
            TargetRunWalkType = targetRunWalkType;
            TargetNameList = targetNameList;
        }

        public void UpdateTargetTypes(CharacterType[] targetTypes)
        {
            TargetTypes = targetTypes;
        }

        public void UpdateTargetRaceTypes(CharacterRaceType[] targetRaceTypes)
        {
            TargetRaceTypes = targetRaceTypes;
        }

        public void UpdateTargetExpireTime(int miliseconds = 2000)
        {
            TargetExpireTime = DateTime.Now.AddMilliseconds(miliseconds);
        }
    }

    public enum SelectTargetStatusType
    {
        [Display(Name = "Main Character", Order = 10)]
        Main,
        [Display(Name = "Personal Select", Order = 10)]
        Personal
    }

    public enum SelectTargetType
    {
        [Display(Name = "Automatic Monster", Order = 10)]
        AutomaticMonster,
        [Display(Name = "Manuel", Order = 20)]
        Manuel,
        [Display(Name = "Enemy Race", Order = 30)]
        EnemyRace,
        [Display(Name = "List", Order = 40)]
        List,
        [Display(Name = "Party Leader", Order = 50)]
        PartyLeader
    }

    public enum SelectTargetRaceType
    {
        [Display(Name = "Everyone", Order = 10)]
        Everyone,
        [Display(Name = "Monster", Order = 20)]
        Monster,
        [Display(Name = "Player", Order = 30)]
        Player
    }
}
