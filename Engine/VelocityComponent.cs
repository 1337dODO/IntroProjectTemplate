using Microsoft.Xna.Framework;

namespace Engine;

public class VelocityComponent : Component
{
    public Vector2 Velocity { get; set; } = Vector2.Zero;

    public float VelocityScalar
    {
        get => Velocity.Length();
        set
        {
            var direction = Vector2.Normalize(Velocity);
            Velocity = direction * value;
        }
    }

    public override void Update(GameTime gameTime)
    {
        Owner.LocalPosition += Velocity * gameTime.DeltaTime;
    }
}
