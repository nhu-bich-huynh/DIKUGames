using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace BreakOut.EntityObjects
{
    /// <summary>
    /// Represents the unbreakable block type which cannot be destroyed.
    /// </summary>
    public class Unbreakable : Block
    {
        private Unbreakable(int value, int health, StationaryShape shape, IBaseImage image) :
            base(value, health, shape, image)
        {
        }

        public static Unbreakable UnbreakFactory(StationaryShape shape, IBaseImage image)
        {
            return new Unbreakable(1, 1, shape, image);
        }

        /// <summary>
        /// Since the unbreakable block cannot be broken, its hit method is overwritten to do nothing.
        /// </summary>
        public override void Hit()
        {
            UpdateState();
        }
    }
}