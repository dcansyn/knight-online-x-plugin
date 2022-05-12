using System.Linq;

namespace KO.Domain.Characters
{
    public class CharacterWalk
    {
        public bool IsWalk { get; protected set; }
        public string[] CoordinateList { get; protected set; }
        public CharacterWalkType CoordinateWalkType { get; protected set; }
        public bool IsWalkCoordinateWhenTargetEmpty { get; protected set; }
        public int CoordinateRow { get; set; }
        public int ChangeCoordinateSeconds { get; set; }

        public CharacterWalk(bool isWalk, string[] coordinateList, CharacterWalkType coordinateWalkType, bool isWalkCoordinateWhenTargetEmpty, int changeCoordinateSeconds)
        {
            IsWalk = isWalk;
            CoordinateList = coordinateList;
            CoordinateWalkType = coordinateWalkType;
            IsWalkCoordinateWhenTargetEmpty = isWalkCoordinateWhenTargetEmpty;
            ChangeCoordinateSeconds = changeCoordinateSeconds;
            CoordinateRow = -1;
        }

        public void UpdateCoordinateRow(int coordinateRow)
        {
            CoordinateRow = coordinateRow;
        }

        public string GetCurrentCoodinate()
        {
            if (CoordinateRow < 0 || CoordinateRow >= CoordinateList.Count()) CoordinateRow = 0;

            return CoordinateList.Count() > 0 ? CoordinateList.ElementAt(CoordinateRow) : null;
        }

        public void NextCoordinate()
        {
            if (CoordinateRow < CoordinateList.Count())
                CoordinateRow++;
            else
                CoordinateRow = 0;
        }
    }
}
