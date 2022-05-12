using KO.Domain.Boxes;
using KO.Domain.Characters;
using KO.Domain.Parties;
using KO.Domain.Skills;
using KO.Domain.Supplies;
using System;
using System.Linq;

namespace KO.Application
{
    public class Client
    {
        public static bool ProgramIsActive { get; set; } = false;
        public static bool AttackIsActive { get; set; } = false;

        public static Character[] Characters { get; set; }
        public static Character Main => Characters.FirstOrDefault(x => x.IsMain);

        public static CharacterProtection CharacterProtection { get; set; }
        public static CharacterScroll CharacterScroll { get; set; }
        public static CharacterFeature CharacterFeature { get; set; }
        public static CharacterFollow CharacterFollow { get; set; }
        public static CharacterTarget CharacterTarget { get; set; }
        public static CharacterWalk CharacterWalk { get; set; }
        public static BoxCollect BoxCollect { get; set; }
        public static Supply Supply { get; set; }
        public static Party Party { get; set; }

        public static DateTime WalkTime { get; set; }
        public static Box[] BoxLoots { get; set; } = new Box[] { };
        public static Skill[] PotionSkills { get; set; } = new Skill[] { };
        public static Skill[] MagicHammerSkills { get; set; } = new Skill[] { };
        public static Skill[] PartySkills { get; set; } = new Skill[] { };
        public static Skill[] Skills { get; set; } = new Skill[] { };
        public static Character[] Targets { get; set; } = new Character[] { };
        public static Character[] PartyCharacters { get; set; } = new Character[] { };
        public static DeadCharacter[] DeadCharacters { get; set; } = new DeadCharacter[] { };
    }
}
