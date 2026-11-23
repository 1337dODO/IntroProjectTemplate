using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IntroGame;

public class Game1 : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private InputManager _inputManager = new InputManager();

    private GameTree _gameTree = new GameTree();

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        var playerTexture = Content.Load<Texture2D>("Pixel Art Top Down - Basic/TX Plant");
        var player = new GameObject();

        var velocityComponent = player.AddComponent<VelocityComponent>();

        var spriteComponent = player.AddComponent<SpriteComponent>();
        spriteComponent.Texture = playerTexture;
        spriteComponent.SourceRectangle = new Rectangle(location: new Point(24, 14), size: new Point(113, 139));
        spriteComponent.OriginType = OriginType.Center;

        var playerInputComponent = player.AddComponent<PlayerInputComponent>();
        playerInputComponent.InputManager = _inputManager;
        playerInputComponent.VelocityComponent = velocityComponent;

        player.GlobalPosition = new Vector2(400f, 200f);
        _gameTree.AddGameObject(player);
    }

    protected override void Update(GameTime gameTime)
    {
#if !__IOS__
        // Exit the game if the Back button (GamePad) or Escape key (Keyboard) is pressed.
        // (iOS does not allow programmatically closing an app.)
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
#endif

        // TODO: Add your update logic here
        _inputManager.Update();
        _gameTree.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        _gameTree.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
