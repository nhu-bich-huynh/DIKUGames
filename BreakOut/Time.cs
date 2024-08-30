using DIKUArcade.Timers;


namespace BreakOut
{
    /// <summary>
    /// Used for keeping track of the passage of real-time while the game is running.
    /// </summary>
    static public class Time
    {
        static long _time = StaticTimer.GetElapsedMilliseconds();
        static long _prevTime = StaticTimer.GetElapsedMilliseconds();
        static public long DeltaTime()
        {
            return _time - _prevTime;
        }
        static public void UpdateDeltaTime()
        {
            _prevTime = _time;
            _time = StaticTimer.GetElapsedMilliseconds();
        }
    }
}
