
using Animancer.FSM;
using Animancer.Units;
using UnityEngine;
using static Animancer.Validate.Value;

namespace PlatformerGame.Characters.Brains
{
    
    public class PlayerBrain : CharacterBrain
    {
        [SerializeField, Seconds(Rule = IsNotNegative)] private float _InputBufferTimeOut = 0.5f;


        protected virtual void Awake()
        {
        }

        protected virtual void Update()
        {
        }



   }
}
