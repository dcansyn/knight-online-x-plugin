using System.ComponentModel.DataAnnotations;

namespace KO.Domain.Supplies
{
    public class SupplyAction
    {
        public int X { get; set; }
        public int Y { get; set; }
        public SupplyActionType Type { get; set; }

        public SupplyAction(int x, int y, SupplyActionType type)
        {
            X = x;
            Y = y;
            Type = type;
        }
    }

    public enum SupplyActionType
    {
        [Display(Name = "Coordinate")]
        Coordinate,
        [Display(Name = "Town")]
        Town,
        [Display(Name = "Sundires")]
        Sundires,
        [Display(Name = "Potion")]
        Potion,
        [Display(Name = "Completed")]
        Completed,
    }
}
