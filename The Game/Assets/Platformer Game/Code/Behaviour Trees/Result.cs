namespace PlatformerGame.BehaviourTrees
{
    
    public enum Result
    {
        
        Pending = 0,

        Pass = 1,

        Fail = -1,
    }

    public static partial class BehaviourTreeUtilities
    {
        public static Result ToResult(this bool value) => value ? Result.Pass : Result.Fail;


        public static Result Invert(this Result result) => (Result)(-(int)result);

    }
}
