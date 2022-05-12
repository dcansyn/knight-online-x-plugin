using KO.Application.Addresses.Extensions;
using KO.Application.Addresses.Handlers;
using KO.Application.Characters.Extensions;
using KO.Core.Constants;
using KO.Core.Extensions;
using KO.Core.Helpers.Utility;
using KO.Domain.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KO.Application.Targets.Extensions
{
    public static class TargetExtensions
    {
        public static async Task<Character> GetBaseTargetById(this Character character, int targetId)
        {
            var baseAddress = await character.GetTargetBase(targetId);
            return character.GetTargetDetailByBase(baseAddress);
        }

        public static async Task<Character> GetTarget(this Character character)
        {
            var baseAddress = await character.GetTargetBase(character.GetTargetId());
            return character.GetTargetDetailByBase(baseAddress);
        }

        public static Character GetTargetByName(this Character character, string name)
        {
            var targets = character.GetAllTarget(findNpc: true);
            return targets.Where(x => x.Name.Contains(name)).OrderBy(x => x.Distance).FirstOrDefault();
        }

        public static Character GetTargetById(this Character character, int id, CharacterType targetType = CharacterType.NonPlayerCharacter, bool excludeDead = true)
        {
            var targets = character.GetAllTarget(targetType, excludeDead);
            return targets.FirstOrDefault(x => x.Id == id);
        }

        public static int GetTargetDistance(this Character character)
        {
            if (character.GetTargetId() <= 0)
                return 255;
            return DistanceHelper.GetDistance(character.GetCharacterX(), character.GetCharacterY(), character.GetTargetX(), character.GetTargetY());
        }

        public static int GetTargetDistance(this Character character, int targetX, int targetY)
        {
            return DistanceHelper.GetDistance(character.GetCharacterX(), character.GetCharacterY(), targetX, targetY);
        }

        public static int GetTargetId(this Character character)
        {
            return character.ReadLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_MOB);
        }

        public static string GetTargetHexId(this Character character)
        {
            return character.GetTargetId().ConvertToDword(2);
        }

        public static int GetTargetX(this Character character)
        {
            return (int)character.ReadFloat(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_TARGET_COR_BASE) + Settings.KO_OFF_TARGET_X);
        }

        public static int GetTargetY(this Character character)
        {
            return (int)character.ReadFloat(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_TARGET_COR_BASE) + Settings.KO_OFF_TARGET_Y);
        }

        public static int GetTargetZ(this Character character)
        {
            return (int)character.ReadFloat(character.ReadLong(character.ReadLong(Settings.KO_PTR_DLG) + Settings.KO_OFF_TARGET_COR_BASE) + Settings.KO_OFF_TARGET_Z);
        }

        public static async Task<int> GetTargetBase(this Character character, int targetId)
        {
            var targetBase = Settings.KO_PTR_FPBS;
            if (targetId > 9999)
                targetBase = Settings.KO_PTR_FMBS;

            if (targetId <= 0)
                return 0;

            await character.ExecuteCode(character.TargetBaseAddress, "608B0D", Settings.KO_PTR_FLDB.ConvertToDword(), "6A0168", targetId.ConvertToDword(), "BF", targetBase.ConvertToDword(), "FFD7A3", ((long)character.TargetBaseAddress).ConvertToDword(), "61C3");
            return character.ReadLong((int)character.TargetBaseAddress);
        }

        public static Character[] CollectTargets(this Character character, int maxDistance, CharacterType[] targetTypes)
        {
            var targets = new List<Character>();

            foreach (var targetType in targetTypes)
                targets.AddRange(character.GetAllTarget(targetType));

            if (targets.Count == 0) return new Character[] { };

            return targets.Where(x => x.Distance <= maxDistance).OrderBy(x => x.Distance).ToArray();
        }

        public static Character[] GetAllTarget(this Character character, CharacterType targetType = CharacterType.NonPlayerCharacter, bool excludeDead = true, bool findNpc = false)
        {
            var targets = new List<Character>();
            int Ebp, Esi, Eax, Fend, baseAddress, targetOff;

            if (targetType == CharacterType.NonPlayerCharacter)
                targetOff = Settings.KO_OFF_BASE_TARGET;
            else
                targetOff = Settings.KO_OFF_BASE_PLAYER;

            Ebp = character.ReadLong(character.ReadLong(Settings.KO_PTR_FLDB) + targetOff);
            Fend = character.ReadLong(character.ReadLong(Ebp + 4) + 4);
            Esi = character.ReadLong(Ebp);

            var date = DateTime.Now.AddMilliseconds(30);
            while (Esi != Ebp)
            {
                if (DateTime.Now > date) break;

                baseAddress = character.ReadLong(Esi + 0x10);
                if (baseAddress == 0) { break; }

                Eax = character.ReadLong(Esi + 8);
                if (character.ReadLong(Esi + 8) != Fend)
                {
                    while (character.ReadLong(Eax) != Fend)
                    {
                        if (DateTime.Now > date) break;

                        Eax = character.ReadLong(Eax);
                    }
                    Esi = Eax;
                }
                else
                {
                    Eax = character.ReadLong(Esi + 4);
                    while (Esi == character.ReadLong(Eax + 8))
                    {
                        if (DateTime.Now > date) break;

                        Esi = Eax;
                        Eax = character.ReadLong(Eax + 4);
                    }
                    if (character.ReadLong(Esi + 8) != Eax)
                    {
                        Esi = Eax;
                    }
                }

                var id = character.GetTargetId(baseAddress);
                var health = character.GetTargetHealth(baseAddress);
                var raceType = character.GetTargetRaceType(baseAddress);
                var moveType = character.GetTargetMoveType(baseAddress);
                var statusType = character.GetTargetStatusType(baseAddress);



                if (!findNpc && targetType == CharacterType.NonPlayerCharacter && raceType != CharacterRaceType.Monster && !Client.CharacterTarget.TargetTypes.Contains(CharacterType.Player)) continue;
                if (!findNpc && excludeDead && (moveType == CharacterMoveType.Dead || moveType == CharacterMoveType.Remove || statusType == CharacterStatusType.Dead)) continue;
                if (Client.DeadCharacters.Any(x => x.Id == id)) continue;

                targets.Add(character.GetTargetDetailByBase(baseAddress));
            }
            return targets.ToArray();
        }

        public static Character GetTargetDetailByBase(this Character character, int baseAddress)
        {
            var id = character.GetTargetId(baseAddress);
            var name = character.GetTargetName(baseAddress);
            var level = character.GetTargetLevel(baseAddress);
            var x = character.GetTargetX(baseAddress);
            var y = character.GetTargetY(baseAddress);
            var z = character.GetTargetZ(baseAddress);
            var distance = DistanceHelper.GetDistance(character.GetCharacterX(), character.GetCharacterY(), x, y);
            var centerDistance = DistanceHelper.GetDistance(Client.CharacterTarget.TargetRunCenterX, Client.CharacterTarget.TargetRunCenterY, x, y);
            var health = character.GetTargetHealth(baseAddress);
            var maxHealth = character.GetTargetMaxHealth(baseAddress);
            var classId = character.GetTargetClassId(baseAddress);
            var type = id > 9999 ? CharacterType.NonPlayerCharacter : CharacterType.Player;
            var raceType = character.GetTargetRaceType(baseAddress);
            var classType = character.GetTargetClassType(baseAddress);
            var classNameType = character.GetTargetClassNameType(baseAddress);
            var moveType = character.GetTargetMoveType(baseAddress);
            var statuType = character.GetTargetStatusType(baseAddress);

            if (id <= 0) return null;
            return new Character(id, name, level, x, y, z, distance, centerDistance, health, maxHealth, classId, type, raceType, classType, classNameType, moveType, statuType);
        }

        public static int GetTargetId(this Character character, int targetBase)
        {
            return character.ReadLong(targetBase + Settings.KO_OFF_ID);
        }

        public static string GetTargetName(this Character character, int targetBase)
        {
            var nickLen = character.ReadLong(targetBase + Settings.KO_OFF_NAME_LENGTH);
            var nick = targetBase + Settings.KO_OFF_NAME;
            if (nickLen > 15)
                return character.ReadByteArray(character.ReadLong(nick), nickLen).ConvertByteArrayToString();
            return character.ReadByteArray(nick, nickLen).ConvertByteArrayToString();
        }

        public static int GetTargetLevel(this Character character, int targetBase)
        {
            return character.ReadLong(targetBase + Settings.KO_OFF_LEVEL);
        }

        public static int GetTargetX(this Character character, int targetBase)
        {
            return (int)character.ReadFloat(targetBase + Settings.KO_OFF_X);
        }

        public static int GetTargetY(this Character character, int targetBase)
        {
            return (int)character.ReadFloat(targetBase + Settings.KO_OFF_Y);
        }

        public static int GetTargetZ(this Character character, int targetBase)
        {
            return (int)character.ReadFloat(targetBase + Settings.KO_OFF_Z);
        }

        public static int GetTargetHealth(this Character character, int targetBase)
        {
            return character.ReadLong(targetBase + Settings.KO_OFF_HP);
        }

        public static int GetTargetMaxHealth(this Character character, int targetBase)
        {
            return character.ReadLong(targetBase + Settings.KO_OFF_MAX_HP);
        }

        public static int GetTargetClassId(this Character character, int targetBase)
        {
            return character.ReadLong(targetBase + Settings.KO_OFF_CLASS);
        }

        public static CharacterRaceType GetTargetRaceType(this Character character, int targetBase)
        {
            var nation = character.ReadLong(targetBase + Settings.KO_OFF_NT);
            switch (nation)
            {
                case 0:
                    return CharacterRaceType.Monster;
                case 1:
                    return CharacterRaceType.Karus;
                case 2:
                    return CharacterRaceType.Human;
                default:
                    return CharacterRaceType.All;
            }
        }

        public static CharacterClassType GetTargetClassType(this Character character, int targetBase)
        {
            switch (character.GetTargetClassId(targetBase))
            {
                case 101:
                case 105:
                case 205:
                case 106:
                case 206:
                    return CharacterClassType.Warrior;
                case 102:
                case 107:
                case 207:
                case 108:
                case 208:
                    return CharacterClassType.Rogue;
                case 103:
                case 109:
                case 209:
                case 110:
                case 210:
                    return CharacterClassType.Magician;
                case 104:
                case 111:
                case 211:
                case 112:
                case 212:
                    return CharacterClassType.Priest;
                case 113:
                case 213:
                case 114:
                case 214:
                case 115:
                case 215:
                    return CharacterClassType.Kurian;
                case 255:
                    return CharacterClassType.GM;
                default:
                    return CharacterClassType.None;
            }
        }

        public static CharacterClassNameType GetTargetClassNameType(this Character character, int targetBase)
        {
            switch (character.GetTargetClassId(targetBase))
            {
                case 101:
                    return CharacterClassNameType.Warrior;
                case 105:
                case 205:
                    return CharacterClassNameType.KnightWarrior;
                case 106:
                case 206:
                    return CharacterClassNameType.MasterWarrior;
                case 102:
                    return CharacterClassNameType.Rogue;
                case 107:
                case 207:
                    return CharacterClassNameType.KnightRogue;
                case 108:
                case 208:
                    return CharacterClassNameType.MasterRogue;
                case 103:
                    return CharacterClassNameType.Magician;
                case 109:
                case 209:
                    return CharacterClassNameType.KnightMagician;
                case 110:
                case 210:
                    return CharacterClassNameType.MasterMagician;
                case 104:
                    return CharacterClassNameType.Priest;
                case 111:
                case 211:
                    return CharacterClassNameType.KnightPriest;
                case 112:
                case 212:
                    return CharacterClassNameType.MasterPriest;
                case 113:
                case 213:
                    return CharacterClassNameType.Kurian;
                case 114:
                case 214:
                    return CharacterClassNameType.KnightKurian;
                case 115:
                case 215:
                    return CharacterClassNameType.MasterKurian;
                default:
                    return CharacterClassNameType.None;
            }
        }

        public static CharacterMoveType GetTargetMoveType(this Character character, int targetBase)
        {
            var val = character.ReadByte(targetBase + Settings.KO_OFF_TARGET_MOVE);
            switch (val)
            {
                case 0:
                    return CharacterMoveType.Normal;
                case 1:
                    return CharacterMoveType.Attack;
                case 4:
                    return CharacterMoveType.Dead;
                default:
                    return CharacterMoveType.Unknown;
            }
        }

        public static CharacterStatusType GetTargetStatusType(this Character character, int targetBase)
        {
            var val = character.ReadByte(targetBase + Settings.KO_OFF_TARGET_STATU);
            switch (val)
            {
                case 0:
                    return CharacterStatusType.Normal;
                case 1:
                    return CharacterStatusType.Moving;
                case 2:
                    return CharacterStatusType.Running;
                case 4:
                    return CharacterStatusType.Attack;
                case 10:
                    return CharacterStatusType.Dead;
                default:
                    return CharacterStatusType.Unknown;
            }
        }
    }
}
