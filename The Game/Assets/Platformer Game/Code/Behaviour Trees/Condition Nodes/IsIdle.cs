using PlatformerGame.Characters;
using System;

namespace PlatformerGame.BehaviourTrees
{
    [Serializable]
    public sealed class IsIdle : ConditionNode
    {
        public override bool Condition
        {
            get
            {
                var character = Context<Character>.Current;
                return character.StateMachine.CurrentState == character.Idle;
            }
        }

    }
}
