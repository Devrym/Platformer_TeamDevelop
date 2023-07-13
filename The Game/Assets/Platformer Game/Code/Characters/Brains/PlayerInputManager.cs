using UnityEngine;

namespace PlatformerGame.Characters.Brains
{
    public class PlayerInputManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerBrain _States;

        [Header("Input Names (Edit/Preferences/Input Manager)")]

        [SerializeField]
        [Tooltip("Space by default")]
        private string _JumpButton = "Jump";

        [SerializeField]
        [Tooltip("Left Click by default")]
        private string _PrimaryAttackButton = "Fire1";

        [SerializeField]
        [Tooltip("Right Click by default")]
        private string _SecondaryAttackButton = "Fire2";

        [SerializeField]
        [Tooltip("Left Shift by default")]
        private string _RunButton = "Fire3";

        [SerializeField]
        [Tooltip("A/D and Left/Right Arrows by default")]
        private string _XAxis = "Horizontal";

        [SerializeField]
        [Tooltip("W/S and Up/Down Arrows by default")]
        private string _YAxis = "Vertical";

        protected virtual void OnValidate()
        {
            Animancer.AnimancerUtilities.GetComponentInParentOrChildren(gameObject, ref _States);
        }

        protected virtual void Update()
        {

            _States.Character.Run = Input.GetButton(_RunButton);
            _States.Character.MovementDirection = new Vector2(
                Input.GetAxisRaw(_XAxis),
                Input.GetAxisRaw(_YAxis));
        }

     }
}
