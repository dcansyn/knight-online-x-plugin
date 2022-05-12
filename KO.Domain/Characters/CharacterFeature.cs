namespace KO.Domain.Characters
{
    public class CharacterFeature
    {
        public bool Speed { get; protected set; }
        public bool Wall { get; protected set; }

        public CharacterFeature(bool speed, bool wall)
        {
            Speed = speed;
            Wall = wall;
        }
    }
}
