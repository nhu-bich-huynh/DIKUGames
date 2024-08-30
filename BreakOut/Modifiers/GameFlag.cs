namespace BreakOut.Modifiers
{

    [Flags]
    public enum GameFlag : byte
    {
        None = 0,
        Rocket = 1,
        FogOfWar = 2,
        ExtraLife = 4,
        LoseLife = 8,
        PlayerSpeed = 16,
        Wide = 32,
        DoubleSize = 64,
    }
}
