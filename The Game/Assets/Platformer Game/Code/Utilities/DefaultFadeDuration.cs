namespace Animancer
{
    internal static class DefaultFadeDuration
    {

        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize() => AnimancerPlayable.DefaultFadeDuration = 0;

    }
}
