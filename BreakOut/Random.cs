namespace BreakOut
{

    /// <summary>
    /// Used for generating random numbers in the game without having to instantiate more than one Random object.
    /// </summary>
    public static class BreakoutRandom
    {
        private static Random? rand;
        public static Random GetRand()
        {
            return rand ??= new Random();
        }
    }
}
