using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace BreakOut.EntityObjects
{
    /// <summary>
    /// Represents the Hardened block type with twice the normal amount of health.
    /// </summary>
    public class Hardened : Block
    {
        private readonly int maxHealth;
        private readonly IBaseImage damagedImage;
        private Hardened(int value, int health, StationaryShape shape, IBaseImage image, IBaseImage dmgimg) :
            base(value, health, shape, image)
        {
            maxHealth = this.health;
            damagedImage = dmgimg;
        }

        public static Hardened HardenedFactory(StationaryShape shape, IBaseImage image, IBaseImage dmgimg)
        {
            return new Hardened(2, 2, shape, image, dmgimg);
        }

        protected override void UpdateState()
        {
            base.UpdateState();
            if (this.health <= maxHealth) this.Image = damagedImage;
        }
    }
}
