using Microsoft.Xna.Framework;

namespace Engine;

public static class ExtensionMethods
{
    extension(GameTime gameTime)
    {
        public float DeltaTime => (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    /// <summary>
    /// Calculates how much a sprite should be offset compared to its position (Dutch: "aangrijpspunt").
    /// </summary>
    /// <param name="rectangle">Typically the source rectangle of a Sprite instance.</param>
    /// <param name="offsetType">The origin type.</param>
    /// <returns></returns>
    public static Vector2 GetOrigin(this Rectangle rectangle, OriginType offsetType)
    {
        switch (offsetType)
        {
            // Top.
            default:
            case OriginType.TopLeft:
                return Vector2.Zero;
            case OriginType.TopCenter:
                return Vector2.UnitX * (rectangle.Width / 2f);
            case OriginType.TopRight:
                return Vector2.UnitX * rectangle.Width;
            // Center.
            case OriginType.CenterLeft:
                return Vector2.UnitY * (rectangle.Height / 2f);
            case OriginType.Center:
                return new Vector2(rectangle.Width / 2f, rectangle.Height / 2f);
            case OriginType.CenterRight:
                return new Vector2(rectangle.Width, rectangle.Height / 2f);
            // Bottom.
            case OriginType.BottomLeft:
                return Vector2.UnitY * rectangle.Height;
            case OriginType.BottomCenter:
                return new Vector2(rectangle.Width / 2f, rectangle.Height);
            case OriginType.BottomRight:
                return new Vector2(rectangle.Width, rectangle.Height);
        }
    }
}
