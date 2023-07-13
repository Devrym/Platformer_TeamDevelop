using UnityEngine;

namespace PlatformerGame
{
    public sealed class Destructible : MonoBehaviour, Hit.ITarget
    {
        public bool CanBeHit(ref Hit hit) => true;

        public void ReceiveHit(ref Hit hit) => Destroy(gameObject);

    }
}
