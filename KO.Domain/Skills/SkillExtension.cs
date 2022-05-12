using SQLite.CodeFirst;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KO.Domain.Skills
{
    [Table("SkillExtensions")]
    public class SkillExtension
    {
        [Autoincrement, Key]
        public int Id { get; protected set; }

        public int Number { get; protected set; }
        public int BaseSkillId { get; protected set; } // 0
        public int ArrowCount { get; protected set; } // 2
        public int PotionType { get; protected set; } // 3
        public int PotionValue { get; protected set; } // 3
        public int BuffType { get; protected set; } // 4
        public int AreaRadius { get; protected set; } // 4
        public int BuffDurationBase { get; protected set; } // 4

        public int BuffDuration { get; protected set; }
        public bool IsPotion { get; protected set; }
        public bool IsHealthPotion { get; protected set; }
        public bool IsManaPotion { get; protected set; }

        public SkillExtension() { }

        public SkillExtension(int number, int baseId)
        {
            Number = number;
            BaseSkillId = baseId;
        }

        public SkillExtension(int number, int baseId, int arrowCount)
        {
            Number = number;
            BaseSkillId = baseId;
            ArrowCount = arrowCount;
        }

        public SkillExtension(int number, int baseId, int potionType, int potionValue)
        {
            Number = number;
            BaseSkillId = baseId;
            PotionType = potionType;
            PotionValue = potionValue;

            IsHealthPotion = PotionType == 1;
            IsManaPotion = PotionType == 2;

            IsPotion = IsHealthPotion || IsManaPotion;
        }

        public SkillExtension(int number, int baseId, int buffType, int areaRadius, int buffDurationBase)
        {
            Number = number;
            BaseSkillId = baseId;
            BuffType = buffType;
            AreaRadius = areaRadius;
            BuffDurationBase = buffDurationBase;

            BuffDuration = BuffDurationBase * 100;
        }
    }
}
