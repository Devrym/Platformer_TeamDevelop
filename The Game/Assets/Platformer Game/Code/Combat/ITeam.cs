using UnityEngine;

namespace PlatformerGame
{
    public interface ITeam
    {
        Team Team { get; }
    }

    public static partial class PlatformerUtilities
    {
        public static bool IsEnemy(this Team team, GameObject other)
            => team.IsEnemy(other.GetTeam());

        public static bool IsEnemy(GameObject gameObject, GameObject other)
            => gameObject.GetTeam().IsEnemy(other);

        public static Team GetTeam(this GameObject gameObject)
        {
            if (gameObject == null)
                return default;

            var team = gameObject.GetComponentInParent<ITeam>();
            if (team == null)
                return default;

            return team.Team;
        }

    }
}
