using UnityEngine;

namespace PlatformerGame
{
    public static class Strings
    {
        public const string ProductName = "Platformer Game";

        public const string MenuPrefix = ProductName + "/";


        public static class Tooltips
        {
            public const string HitAngle = "The direction in which the knockback force is applied (in degrees)" +
                "\n• 0 = Forward" +
                "\n• 90 = Up" +
                "\n• 180 = Backward" +
                "\n• -90 = Down";
        }

    }
}
