using System;

namespace PlatformerGame
{
    public readonly struct Context<T> : IDisposable
    {
        [ThreadStatic]
        private static T _Current;
        public static ref T Current
        {
            get
            {
#if UNITY_EDITOR
                if (enableStackTrace &&
                    (_CurrentStackTrace == null || Equals(_Current, BoxedDefault)))
                    _CurrentStackTrace = new System.Diagnostics.StackTrace(1, true);
#endif
                return ref _Current;
            }
        }


        private readonly T Previous;

        public Context(T value)
        {
            Previous = Current;
            _Current = value;
#if UNITY_EDITOR
            PreviousStackTrace = _CurrentStackTrace;
            if (enableStackTrace)
                _CurrentStackTrace = new System.Diagnostics.StackTrace(1, true);
#endif
        }

        public static implicit operator Context<T>(T value) => new Context<T>(value);

        public void Dispose()
        {
            _Current = Previous;
#if UNITY_EDITOR
            _CurrentStackTrace = PreviousStackTrace;
#endif
        }

#if UNITY_EDITOR

        public static bool enableStackTrace;

        private static readonly object BoxedDefault = default(T);

        private static System.Diagnostics.StackTrace _CurrentStackTrace;

        private readonly System.Diagnostics.StackTrace PreviousStackTrace;


        static Context()
        {
            UnityEditor.EditorApplication.update += () =>
            {
                if (!Equals(Current, BoxedDefault))
                {
                    enableStackTrace = true;

                    var stackTrace = _CurrentStackTrace != null ?
                        $"\n{_CurrentStackTrace}" :
                        $"Null because {nameof(Context<T>)}<{typeof(T).FullName}>.{nameof(enableStackTrace)} was false" +
                        $" when the {nameof(Context<T>)} was created. It is now set to true for future usage, but may" +
                        $" need to be set manually on startup to identify where the first issue occurs.";

                    UnityEngine.Debug.LogError(
                        $"{nameof(Context<T>)}<{typeof(T).FullName}> hasn't been disposed." +
                        $"\n- {nameof(Current)}: {Current}" +
                        $"\n- {nameof(System.Diagnostics.StackTrace)}: {stackTrace}",
                        Current as UnityEngine.Object);

                    _Current = default;
                    _CurrentStackTrace = null;
                }
            };
        }

#endif
    }
}
