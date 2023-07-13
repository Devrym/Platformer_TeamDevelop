using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PlatformerGame
{
    public partial struct Hit
    {
        public Transform source;

        public Team team;

        public int damage;

        public float force;

        public Vector2 direction;

        public Hit(
            Transform source,
            Team team,
            int damage,
            float force,
            Vector2 direction,
            ICollection<ITarget> ignore = null)
        {
            target = null;
            this.source = source;
            this.team = team;
            this.damage = damage;
            this.force = force;
            this.direction = direction;
            this.ignore = ignore;
        }


        partial void AppendDetails(StringBuilder text)
        {
            text.Append($", {nameof(source)}='").Append(source != null ? source.name : "null")
                .Append($"', {nameof(team)}=").Append(team)
                .Append($", {nameof(damage)}=").Append(damage)
                .Append($", {nameof(force)}=").Append(force);
        }

    }
}
