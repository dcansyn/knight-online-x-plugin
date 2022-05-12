namespace KO.Domain.Supplies
{
    public class SupplyItem
    {
        public int ItemId { get; protected set; }
        public int Count { get; protected set; }
        public int Page { get; protected set; }
        public int Row { get; protected set; }
        public bool IsComplete { get; protected set; }

        public SupplyItem(int itemId, int count, int page, int row, bool isComplete = true)
        {
            ItemId = itemId;
            Count = count;
            Page = page;
            Row = row;
            IsComplete = isComplete;
        }
    }
}
