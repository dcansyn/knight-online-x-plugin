using KO.Application.Addresses.Extensions;
using KO.Application.Addresses.Handlers;
using KO.Application.Characters.Extensions;
using KO.Application.Items.Extensions;
using KO.Core.Constants;
using KO.Core.Extensions;
using KO.Core.Helpers.Utility;
using KO.Domain.Characters;
using KO.Domain.Items;
using KO.Domain.Skills;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KO.Application.Characters.Handlers
{
    public static class CharacterHandler
    {
        public static async Task CollectCharaterInformation(this Character character)
        {
            character.UpdateInformation(
                id: character.GetCharacterId(),
                name: character.GetCharacterName(),
                level: character.GetCharacterLevel(),
                x: character.GetCharacterX(),
                y: character.GetCharacterY(),
                z: character.GetCharacterZ(),
                health: character.GetCharacterHealth(),
                maxHealth: character.GetCharacterMaxHealth(),
                mana: character.GetCharacterMana(),
                maxMana: character.GetCharacterMaxMana(),
                classId: character.GetCharacterClassId(),
                raceType: character.GetCharacterRaceType(),
                classType: character.GetCharacterClassType(),
                classNameType: character.GetCharacterClassNameType()
                );

            var items = character.GetItems();

            character.UpdateItems(items);

            if (character.ClassType == CharacterClassType.Rogue)
            {
                var leftWeapon = items.FirstOrDefault(x => x.InventoryStatus == ItemInventoryStatusType.LeftWeapon && x.ItemId > 0);
                var rightWeapon = items.FirstOrDefault(x => x.InventoryStatus == ItemInventoryStatusType.RightWeapon && x.ItemId > 0);
                character.UpdateAttackType(leftWeapon != null && rightWeapon != null ? CharacterAttackType.Melee : CharacterAttackType.Archery);
            }
            else
                character.UpdateAttackType(CharacterAttackType.Melee);

            character.UpdateUsedSkills(character.UsedSkills.Where(x => DateTime.Now < x.UseTime).ToArray());

            await Task.CompletedTask;
        }

        public static async Task DisableWall(this Character character, bool val)
        {
            await character.WriteLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_WH, Convert.ToInt32(!val));
        }

        public static async Task Walk(this Character character, int goX, int goY, CharacterWalkType walkType = CharacterWalkType.Walk, bool afterNormalWalk = true)
        {
            if (DistanceHelper.GetDistance(character.GetCharacterX(), character.GetCharacterY(), goX, goY) < 1 || goX == 0 || goY == 0)
                return;

            switch (walkType)
            {
                case CharacterWalkType.Jump:
                    await character.Jump(goX, goY, afterNormalWalk);
                    break;

                case CharacterWalkType.Teleport:
                    await character.Teleport(goX, goY, afterNormalWalk);
                    break;

                case CharacterWalkType.Walk:
                default:
                    await character.WriteLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_MOVE, 1);
                    await character.WriteFloat(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_MOUSE_X, goX);
                    await character.WriteFloat(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_MOUSE_Y, goY);
                    await character.WriteLong(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_MOVE_TYPE, 2);
                    break;
            }
        }

        private static async Task Jump(this Character character, int goX, int goY, bool afterNormalWalk = true)
        {
            if (DistanceHelper.GetDistance(character.GetCharacterX(), character.GetCharacterY(), goX, goY) < 1 || goX == 0 || goY == 0)
                return;

            var jumpX = 6;
            var jumpY = 6;
            var distanceX = goX - character.GetCharacterX();
            var distanceY = goY - character.GetCharacterY();

            for (int i = 0; i <= 6; i++)
            {
                if (distanceX == -1 * i || distanceX == i)
                {
                    jumpX = 1;
                }
                else if (distanceY == -1 * i || distanceY == i)
                    jumpY = 1;
            }
            if (distanceX < 0)
                await character.WriteFloat(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_X, character.GetCharacterX() - jumpX);
            else if (distanceX > 0)
                await character.WriteFloat(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_X, character.GetCharacterX() + jumpX);

            if (distanceY < 0)
                await character.WriteFloat(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_Y, character.GetCharacterY() - jumpY);
            else if (distanceY > 0)
                await character.WriteFloat(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_Y, character.GetCharacterY() + jumpY);

            await character.Send("06",
                (character.GetCharacterX() * 10).ConvertToDword(2),
                (character.GetCharacterY() * 10).ConvertToDword(2),
                (character.GetCharacterZ() * 10).ConvertToDword(2),
                "2D0000",
                (character.GetCharacterX() * 10).ConvertToDword(2),
                (character.GetCharacterY() * 10).ConvertToDword(2),
                (character.GetCharacterZ() * 10).ConvertToDword(2));

            if (afterNormalWalk)
                await character.Walk(goX, goY);
        }

        private static async Task Teleport(this Character character, int goX, int goY, bool afterNormalWalk = true)
        {
            if (DistanceHelper.GetDistance(character.GetCharacterX(), character.GetCharacterY(), goX, goY) < 1 || goX == 0 || goY == 0)
                return;

            int jump = 4;
            double cX, cY, isrtx, bykx, kckx, isrty, byky, kcky, A, b, D, e;
            int uzak, tx, ty, CoorX, CoorY;
            tx = character.GetCharacterX();
            ty = character.GetCharacterY();
            cX = Math.Abs(goX - tx);
            cY = Math.Abs(goY - ty);
            if (tx > goX) { isrtx = -1; bykx = tx; kckx = goX; } else { isrtx = 1; bykx = goX; kckx = tx; }
            if (ty > goY) { isrty = -1; byky = ty; kcky = goY; } else { isrty = 1; byky = goY; kcky = ty; }
            uzak = (int)Math.Sqrt(Math.Pow(cX, 2) + Math.Pow(cY, 2));
            for (int i = jump; i <= uzak; i += jump)
            {
                A = Math.Pow(i, 2) * Math.Pow(cX, 2);
                b = Math.Pow(cX, 2) + Math.Pow(cY, 2);
                D = Math.Sqrt(A / b);
                e = Math.Sqrt(Math.Pow(i, 2) - Math.Pow(D, 2));
                CoorX = Convert.ToInt32(tx + isrtx * D);
                CoorY = Convert.ToInt32(ty + isrty * e);
                if (kckx <= CoorX && CoorX <= bykx && kcky <= CoorY && CoorY <= byky)
                {
                    await character.WriteFloat(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_X, CoorX);
                    await character.WriteFloat(character.ReadLong(Settings.KO_PTR_CHR) + Settings.KO_OFF_Y, CoorY);

                    await character.Send("06",
                        (character.GetCharacterX() * 10).ConvertToDword(2),
                        (character.GetCharacterY() * 10).ConvertToDword(2),
                        (character.GetCharacterZ() * 10).ConvertToDword(2),
                        "2D0000",
                        (character.GetCharacterX() * 10).ConvertToDword(2),
                        (character.GetCharacterY() * 10).ConvertToDword(2),
                        (character.GetCharacterZ() * 10).ConvertToDword(2));
                }
            }

            await character.Send("06",
                (character.GetCharacterX() * 10).ConvertToDword(2),
                (character.GetCharacterY() * 10).ConvertToDword(2),
                (character.GetCharacterZ() * 10).ConvertToDword(2),
                "2D0000",
                (character.GetCharacterX() * 10).ConvertToDword(2),
                (character.GetCharacterY() * 10).ConvertToDword(2),
                (character.GetCharacterZ() * 10).ConvertToDword(2));

            if (afterNormalWalk)
                await character.Walk(goX, goY);
        }

        public static async Task SendStatPoint(this Character character, CharacterStatType statType)
        {
            if (statType == CharacterStatType.Point || character.GetCharacterStatPoint(CharacterStatType.Point) <= 0) return;
            await Client.Main.Send("280", ((int)statType).ToString(), "0100");
        }
    }
}