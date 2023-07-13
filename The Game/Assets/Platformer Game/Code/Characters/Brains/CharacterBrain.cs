using Animancer;
using UnityEngine;

namespace PlatformerGame.Characters.Brains
{
    public abstract class CharacterBrain : MonoBehaviour
    {
        public const string MenuPrefix = Character.MenuPrefix + "Brains/";


        public const int DefaultExecutionOrder = -10000;


        [SerializeField]
        private Character _Character;

        public ref Character Character => ref _Character;


#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Character);
        }
#endif

    }
}
