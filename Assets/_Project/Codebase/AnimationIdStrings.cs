using System;

namespace BulletHell
{
    public static class AnimationIdStrings
    {
        public const string CHARACTER_TEMPLATE_IDLE = "Character_Template_Idle";
        public const string CHARACTER_TEMPLATE_RUN = "Character_Template_Run";

        public static AnimationId ToId(in string stringId)
        {
            return Enum.Parse<AnimationId>(stringId);
        }
    }
}