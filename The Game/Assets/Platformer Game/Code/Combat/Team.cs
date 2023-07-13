using System;
using UnityEngine;

namespace PlatformerGame
{
    public sealed class Team : ScriptableObject
    {
        [SerializeField]
        [Tooltip("Other teams are enemies by default unless they are in this list")]
        private Team[] _Allies;
        public ref Team[] Allies => ref _Allies;

#if UNITY_EDITOR
        [SerializeField, TextArea]
        private string _EditorDescription;
#endif

    }


    public static partial class PlatformerUtilities
    {

        public static bool IsAlly(this Team team, Team other)
            => team != null && (team == other || Array.IndexOf(team.Allies, other) >= 0);

        public static bool IsEnemy(this Team team, Team other)
            => !team.IsAlly(other);

    }
}
