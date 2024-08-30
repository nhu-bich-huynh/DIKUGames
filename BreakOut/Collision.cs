using DIKUArcade.Entities;


namespace BreakOut
{
    /// <summary>
    /// Used to assert whether a given Shape is within a given distance of another shape.
    /// This is useful for determining collision for shapes moving without the use of
    /// vectors, etc.
    /// </summary>
    static public class Collision
    {
        static public bool AABB(Shape shape, Shape other)
        {
            return shape.Position.X < other.Position.X + other.Extent.X &&
                other.Position.X < shape.Position.X + shape.Extent.X &&
                shape.Position.Y < other.Position.Y + other.Extent.Y &&
                other.Position.Y < shape.Position.Y + shape.Extent.Y;
        }
    }
}
