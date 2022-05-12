using System.ComponentModel.DataAnnotations;

namespace KO.Domain.Characters
{
    public class CharacterFollow
    {
        public bool IsFollow { get; protected set; }
        public FollowType FollowType { get; protected set; }

        public CharacterFollow(bool isFollow, FollowType followType)
        {
            IsFollow = isFollow;
            FollowType = followType;
        }
    }

    public enum FollowType
    {
        [Display(Name = "Main Character", Order = 10)]
        MainCharacter,
        [Display(Name = "Main Target", Order = 20)]
        MainTarget
    }
}
