using Engine;
using Engine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace IntroGame;

public class PlayerInputComponent : Component
{
    private const float Speed = 300f;

    public InputManager InputManager { get; set; }

    public VelocityComponent VelocityComponent { get; set; }

    public override void Update(GameTime gameTime)
    {
        var direction = Vector2.Zero;

        if (InputManager.CheckIsKeyPressed(Keys.W)) direction -= Vector2.UnitY;

        if (InputManager.CheckIsKeyPressed(Keys.A)) direction -= Vector2.UnitX;

        if (InputManager.CheckIsKeyPressed(Keys.S)) direction += Vector2.UnitY;

        if (InputManager.CheckIsKeyPressed(Keys.D)) direction += Vector2.UnitX;

        VelocityComponent.Velocity = direction == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(direction) * Speed;
    }
}
