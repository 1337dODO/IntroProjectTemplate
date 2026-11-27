using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Components;

public class SpriteComponent : Component
{
    private float _opacity = 1.0f;
    private float _rotation = 0.0f;

    public Texture2D? Texture { get; set; }

    public SpriteEffects Effects { get; set; } = SpriteEffects.None;

    public Color Color { get; set; } = Color.White;

    public Vector2 Scale { get; set; } = Vector2.One;

    public OriginType OriginType { get; set; } = OriginType.TopLeft;

    public Vector2? Origin => SourceRectangle?.GetOrigin(OriginType);

    public float Rotation
    {
        get => _rotation;
        set => _rotation = MathUtils.Modulo(value, MathF.Tau);
    }

    public Rectangle? SourceRectangle
    {
        get => field ?? Texture?.Bounds;
        set;
    }

    public float Opacity
    {
        get => _opacity;
        set => _opacity = Math.Clamp(value, 0.0f, 1.0f);
    }

    public Vector2 GlobalDrawPosition => Owner.GlobalPosition; // TODO: Implement camera offset

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (Texture == null) return;

        Debug.Assert(Origin != null, nameof(Origin) + " != null");
        spriteBatch.Draw(
            texture: Texture,
            position: GlobalDrawPosition,
            sourceRectangle: SourceRectangle,
            color: Color * _opacity,
            rotation: _rotation,
            origin: Origin.Value,
            scale: Scale,
            effects: Effects,
            layerDepth: Owner.DrawLayer
        );
    }
}
