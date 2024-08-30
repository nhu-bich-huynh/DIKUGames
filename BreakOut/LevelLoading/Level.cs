namespace BreakOut
{
    /// <summary>
    /// Represents all the information extracted from a level txt-file.
    /// </summary>
    public class Level
    {
        private readonly string map;
        private readonly Dictionary<string, string> metas;
        private readonly Dictionary<string, string> legends;

        public Level(string map, Dictionary<string, string> metas, Dictionary<string, string> legends)
        {
            this.map = map;
            this.metas = metas;
            this.legends = legends;
        }

        public string Map { get => map; }
        public Dictionary<string, string> Metas { get => metas; }
        public Dictionary<string, string> Legends { get => legends; }

    }
}
