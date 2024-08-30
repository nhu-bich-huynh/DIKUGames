using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace BreakOut.EntityObjects
{
    /// <summary>
    /// The Rocket class represents the Rocket fired after catching the Rocket PowerUp
    /// </summary>
    public class Rocket : Entity
    {
        public Vec2F explosionExtent;

        public Rocket(Shape shape, IBaseImage image) : base(shape, image)
        {
            explosionExtent = new Vec2F(0.2f, 0.2f);
        }

        /// <summary>
        /// Creates a new Rocket object with the specified position.
        /// </summary>
        /// <param name="x">The x-coordinate of the rocket's position.</param>
        /// <param name="y">The y-coordinate of the rocket's position.</param>
        /// <returns>A new Rocket object.</returns>
        public static Rocket Factory(float x, float y)
        {
            Vec2F pos = new(x, y);
            Vec2F dir = new(0.0f, 0.01f);
            Vec2F ext = new(0.05f, 0.05f);
            DynamicShape shape = new(pos - ext / 2, ext, dir);

            List<Image> imgStride = ImageStride.CreateStrides(5,
            Path.Combine("Assets", "Images", "RocketLaunched.png"));
            ImageStride img = new(200, imgStride);

            return new Rocket(shape, img);
        }
    }
}
