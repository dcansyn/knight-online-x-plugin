namespace KO.Domain.Boxes
{
    public class Box
    {
        public int Id { get; protected set; }
        public int TargetId { get; protected set; }
        public int X { get; protected set; }
        public int Y { get; protected set; }

        public Box(int id, int targetId, int x, int y)
        {
            Id = id;
            TargetId = targetId;
            X = x;
            Y = y;
        }
    }
}
