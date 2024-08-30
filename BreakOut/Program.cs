using DIKUArcade.GUI;

namespace BreakOut
{
    class Program
    {
        static void Main(string[] args)
        {
            WindowArgs windowArgs = new() { Title = "BreakOut" };
            Game game = new(windowArgs);
            game.Run();
        }
    }
}
