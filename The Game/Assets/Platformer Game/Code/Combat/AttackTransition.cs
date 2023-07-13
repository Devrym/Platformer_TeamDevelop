using Animancer;
using System;
using UnityEngine;

namespace PlatformerGame
{
    [Serializable]
    public partial class AttackTransition : ClipTransition, ICopyable<AttackTransition>
    {
        [SerializeField]
        private HitData[] _Hits;

        public HitData[] Hits
        {
            get => _Hits;
            set
            {
                if (_HasInitializedEvents)
                    throw new InvalidOperationException(
                        $"Modifying the {nameof(AttackTransition)}.{nameof(Hits)} after the transition has already been used" +
                        $" is not supported because its initialisation modifies the underlying" +
                        $" {nameof(AnimancerEvent)}.{nameof(AnimancerEvent.Sequence)} in a way that can't be easily undone.");

                _Hits = value;
            }
        }

        private bool _HasInitializedEvents;

        public override void Apply(AnimancerState state)
        {
            if (!_HasInitializedEvents)
            {
                _HasInitializedEvents = true;
                HitData.InitializeEvents(Hits, SerializedEvents.Events, Clip.length);
            }

            base.Apply(state);
        }

        public virtual void CopyFrom(AttackTransition copyFrom)
        {
            CopyFrom((ClipTransition)copyFrom);

            if (copyFrom == null)
            {
                _Hits = default;
                return;
            }

            AnimancerUtilities.CopyExactArray(copyFrom._Hits, ref _Hits);
        }

    }
}
