using DIKUArcade.Entities;
using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.Timers;

namespace BreakOut.EntityObjects
{
    using Modifiers;

    /// <summary>
    /// Represents a large collection of blocks, which is useful for instantiating and keeping
    /// track of all the blocks belonging to a particular level.
    /// </summary>
    public class BlockFormation : IFlagHandler
    {
        private readonly EntityContainer<Block> container;
        private readonly char unbreakableChar;
        private readonly char hardenedChar;
        private readonly char PowerUpChar;
        public bool shown;
        private int unbreakableBlocksCount = 0;

        private const float block_width = 12.0f;
        private const float block_height = 40.0f;

        /// <summary>
        /// Instantiates a container with all the blocks belonging to a given level with their corresponding
        /// types, positions and images.
        /// </summary>
        public BlockFormation(Level level)
        {
            container = new EntityContainer<Block>();
            shown = true;
            Modifiers.Subscribe(this);

            string[] mapArr = level.Map.Split("\r\n");

            for (int i = 0; i < mapArr.Length; i++)
            {
                for (int j = 0; j < mapArr[i].Length; j++)
                {
                    bool success = level.Legends.TryGetValue(string.Format("{0}", mapArr[i][j]),
                                out string? legend);

                    if (success && legend != null)
                    {
                        Image blockImage = new Image(Path.Combine("Assets", "Images", legend));
                        float x = j * 1.0f / block_width;
                        float y = i * 1.0f / block_height;
                        StationaryShape blockShape = new(x, 1.0f - y, 1.0f / block_width, 1.0f / block_height);


                        if (level.Metas.ContainsValue(mapArr[i][j].ToString()))
                        {
                            if (level.Metas.ContainsKey("Unbreakable"))
                                unbreakableChar = level.Metas["Unbreakable"][0];
                            if (level.Metas.ContainsKey("Hardened"))
                                hardenedChar = level.Metas["Hardened"][0];
                            if (level.Metas.ContainsKey("PowerUp"))
                                PowerUpChar = level.Metas["PowerUp"][0];

                            switch (mapArr[i][j])
                            {
                                case char value when value == unbreakableChar:
                                    container.AddEntity(Unbreakable.UnbreakFactory(blockShape, blockImage));
                                    break;
                                case char value when value == hardenedChar:
                                    Image damageImg = new Image(Path.Combine("Assets", "Images",
                                        legend[..(legend.Length - 4)] + "-damaged.png"));
                                    container.AddEntity(Hardened.HardenedFactory(blockShape, blockImage, damageImg));
                                    break;
                                case char value when value == PowerUpChar:
                                    container.AddEntity(PowerUpBlock.PowerUpBlockFactory(blockShape, blockImage));
                                    break;
                                default:
                                    container.AddEntity(HazardBlock.HazardBlockFactory(blockShape, blockImage));
                                    break;
                            }
                        }
                        else container.AddEntity(HazardBlock.HazardBlockFactory(blockShape, blockImage));
                    }
                }
            }
            container.Iterate(block =>
            {
                if (block is Unbreakable) unbreakableBlocksCount++;
            });
        }
        public EntityContainer<Block> Container { get => container; }

        /// <summary>
        /// Applies the modifiers for a collected event relevant to all blocks on the screen.
        /// </summary>
        public void ApplyModifiers(GameFlag flag)
        {
            GameFlag added = Modifiers.Added(flag);
            GameFlag removed = Modifiers.Removed(flag);

            if (added.HasFlag(GameFlag.FogOfWar))
            {
                shown = false;

                BreakOutBus.GetBus().AddOrResetTimedEvent(
                    new GameEvent
                    {
                        EventType = GameEventType.StatusEvent,
                        Message = "DisableFogOfWar",
                    },
                    TimePeriod.NewMilliseconds(2000));
            }

            if (removed.HasFlag(GameFlag.FogOfWar))
            {
                shown = true;
            }
        }

        public void Render()
        {
            if (!shown) return;

            foreach (Block block in container)
            {
                block.RenderEntity();
            }
        }

        /// <summary>
        /// Returns the amount of unbreakable blocks in the formation. This is useful for
        /// figuring out when a level has been beaten.
        /// </summary>
        public int UnbreakableBlocksCount()
        {
            return unbreakableBlocksCount;
        }
    }
}
