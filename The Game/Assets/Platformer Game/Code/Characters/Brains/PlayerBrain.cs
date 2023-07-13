
using Animancer.FSM;
using Animancer.Units;
using PlatformerGame.Characters.States;
using UnityEngine;
using static Animancer.Validate.Value;

namespace PlatformerGame.Characters.Brains
{
    
    public class PlayerBrain : CharacterBrain
    {
        [SerializeField, Seconds(Rule = IsNotNegative)] private float _InputBufferTimeOut = 0.5f;

        [SerializeField] private CharacterState _Jump;
        [SerializeField] private CharacterState _PrimaryAttack;
        [SerializeField] private CharacterState _SecondaryAttack;

        private StateMachine<CharacterState>.InputBuffer _InputBuffer;

        protected virtual void Awake()
        {
            _InputBuffer = new StateMachine<CharacterState>.InputBuffer(Character.StateMachine);
        }

        protected virtual void Update()
        {
            _InputBuffer.Update();
        }

        public void TryIdle()
            => Character.StateMachine.TrySetDefaultState();

        public void Buffer(CharacterState state)
        {
            if (state == null)
                return;

            _InputBuffer.Buffer(state, _InputBufferTimeOut);
        }

        public void TryJump()
            => Buffer(_Jump);

        public void TryPrimaryAttack()
            => Buffer(_PrimaryAttack);

        public void TrySecondaryAttack()
            => Buffer(_SecondaryAttack);

   }
}
