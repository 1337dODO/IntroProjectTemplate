using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine;

public abstract class Component
{
    public GameObject Owner { get; init; } = null!;

    public virtual void Initialize() { }

    public virtual void Update(GameTime gameTime) { }

    public virtual void Draw(SpriteBatch spriteBatch) { }
}
