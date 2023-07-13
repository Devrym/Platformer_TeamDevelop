using Animancer;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PlatformerGame
{
    public partial struct Hit
    {
        public interface ITarget
        {
            bool CanBeHit(ref Hit hit);

            void ReceiveHit(ref Hit hit);
        }

        public ITarget target;

        public ICollection<ITarget> ignore;

        public override string ToString()
        {
            var text = ObjectPool.AcquireStringBuilder()
                .Append($"{nameof(Hit)}({nameof(target)}='").Append(target).Append('\'');

            AppendDetails(text);

            text.Append($", {nameof(ignore)}=");
            if (ignore == null)
            {
                text.Append("null");
            }
            else
            {
                text.Append('[')
                    .Append(ignore.Count)
                    .Append("] {");

                var first = true;
                foreach (var ignore in ignore)
                {
                    if (first)
                        first = false;
                    else
                        text.Append(", ");

                    text.Append(ignore);
                }
                text.Append('}');
            }
            text.Append(')');
            return text.ReleaseToString();
        }

        partial void AppendDetails(StringBuilder text);

        public static ITarget GetTarget(Component component)
            => component.GetComponentInParent<ITarget>();

        public static ITarget GetTarget(GameObject gameObject)
            => gameObject.GetComponentInParent<ITarget>();

        public bool CanHit(Component component, bool resultIfNoTarget = false)
        {
            if (component != null)
            {
                var target = GetTarget(component);
                if (target != null)
                    return target.CanBeHit(ref this);
            }

            return resultIfNoTarget;
        }

        public bool CanHit(GameObject gameObject, bool resultIfNoTarget = false)
        {
            if (gameObject != null)
            {
                var target = GetTarget(gameObject);
                if (target != null)
                    return target.CanBeHit(ref this);
            }

            return resultIfNoTarget;
        }

        public bool TryHit(ITarget target, bool dontHitAgain = true)
        {
            if (target == null)
                return false;

            this.target = target;

            if ((ignore != null && ignore.Contains(target)) ||
                !target.CanBeHit(ref this) ||
                target == null)
                return false;

            if (dontHitAgain)
                ignore?.Add(target);

            target.ReceiveHit(ref this);

            return true;
        }

        public bool TryHitComponent(Component target, bool dontHitAgain = true)
            => TryHit(GetTarget(target), dontHitAgain);

        public void TryHitComponents(Component[] targets, bool dontHitAgain = true)
            => TryHitComponents(targets, targets.Length, dontHitAgain);

        public void TryHitComponents(Component[] targets, int count, bool dontHitAgain = true)
        {
            for (int i = 0; i < count; i++)
            {
                var copy = this;
                copy.TryHitComponent(targets[i], dontHitAgain);
            }
        }

    }
}
