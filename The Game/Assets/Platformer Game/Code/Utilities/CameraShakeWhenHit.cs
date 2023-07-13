using Animancer;
using Animancer.Units;
using UnityEngine;

namespace PlatformerGame
{
    public sealed class CameraShakeWhenHit : MonoBehaviour
    {
        [SerializeField, Multiplier]
        [Tooltip("The shake magnitude to apply regardless of damage taken")]
        private float _BaseMagnitude = 0.3f;
        public ref float BaseMagnitude => ref _BaseMagnitude;

        [SerializeField, Multiplier]
        [Tooltip("The additional shake magnitude which is multiplied by the damage taken")]
        private float _ScalingMagnitude = 0.02f;
        public ref float ScalingMagnitude => ref _ScalingMagnitude;

#if UNITY_EDITOR
        private void OnValidate()
        {
            PlatformerUtilities.NotNegative(ref _BaseMagnitude);
            PlatformerUtilities.NotNegative(ref _ScalingMagnitude);
        }
#endif

        private void Awake()
        {
        }

    }
}
