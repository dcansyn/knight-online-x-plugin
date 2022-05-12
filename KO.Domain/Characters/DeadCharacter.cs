using System;

namespace KO.Domain.Characters
{
    public class DeadCharacter
    {
        public int Id { get; set; }
        public DateTime DeadExpireTime { get; set; }

        public DeadCharacter(int id)
        {
            Id = id;
            DeadExpireTime = DateTime.Now.AddSeconds(2);
        }
    }
}
