using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using agalag.engine;
using agalag.game.input;
using System.Diagnostics;
using agalag.engine.content;
using Microsoft.Xna.Framework.Content;
using agalag.game.Source.Game.Scenes;

namespace agalag.game;

public class AgalagGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SceneManager _sceneManager = SceneManager.Instance;
    private InputHandler _inputHandler;

    public static GameTime _globalGameTime;
    public static GameTime GlobalGameTime => _globalGameTime;

    private int _internalResolutionHeight = 1080, _internalResolutionWidth = 1920;
    private int _finalResolutionHeight = 720, _finalResolutionWidth = 1280;
    private bool _isFullscreen = false;

    public AgalagGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        
        ResolutionScaler.SetResolution(
            _graphics,
            _internalResolutionWidth, _internalResolutionHeight, 
            _finalResolutionWidth, _finalResolutionHeight,
            _isFullscreen
        );
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _inputHandler = InputHandler.Instance;
        Prefabs.DefineContent(Content);
    }

    protected override void Initialize()
    {
        TagUtils.SetMask(EntityTag.Player, EntityTag.Player, false);
        TagUtils.SetMask(EntityTag.Player, EntityTag.PlayerBullet, false);
        TagUtils.SetMask(EntityTag.Player, EntityTag.Enemy, true);
        TagUtils.SetMask(EntityTag.Player, EntityTag.Wall, true);
        TagUtils.SetMask(EntityTag.PlayerBullet, EntityTag.Player, false);
        TagUtils.SetMask(EntityTag.PlayerBullet, EntityTag.Enemy, true);
        TagUtils.SetMask(EntityTag.PlayerBullet, EntityTag.Wall, false);
        TagUtils.SetMask(EntityTag.Enemy, EntityTag.Enemy, false);
        TagUtils.SetMask(EntityTag.Enemy, EntityTag.Wall, false);
        TagUtils.SetMask(EntityTag.Enemy, EntityTag.PickUp, false);
        TagUtils.SetMask(EntityTag.Enemy, EntityTag.Hazard, false);
        TagUtils.SetMask(EntityTag.Wall, EntityTag.Wall, false);

        // System.Diagnostics.Debug.WriteLine(TagUtils.GetInteraction(EntityTag.Player, EntityTag.Enemy));

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Utils.ScreenManager = _graphics;

        Prefabs.AddShape(new Texture2D(GraphicsDevice, 80, 30), Shapes.Rectangle);
		
        //Weapons
        Prefabs.AddPrefab<Bullet>(Content.Load<Texture2D>("Sprites/bullet_player"));
        Prefabs.AddSprite("missile", Content.Load<Texture2D>("Sprites/missilePlayer"));
        Prefabs.AddPrefab<Explosion>(Content.Load<Texture2D>("Sprites/explosion"));
        
        //Player
        Texture2D playerSprite = Content.Load<Texture2D>("Sprites/player");
        Prefabs.AddPrefab<Player>(new Player(playerSprite, Vector2.Zero), playerSprite);

        // Kamikaze
        Texture2D kamikazeSprite = Content.Load<Texture2D>("Sprites/enemy_kamikaze");
        Prefabs.AddPrefab<EnemyKamikaze>(new EnemyKamikaze(kamikazeSprite, Vector2.Zero, Vector2.One), kamikazeSprite);
        // Gemini
        Prefabs.AddPrefab<EnemyGemini>(new EnemyGemini(null, Vector2.Zero, Vector2.One));
        // GeminiChild
        Texture2D geminiBullet = Content.Load<Texture2D>("Sprites/bullet_gemini");
        Texture2D geminiChildSprite = Content.Load<Texture2D>("Sprites/enemy_gemini");
        Prefabs.AddPrefab<EnemyGeminiChild>(new EnemyGeminiChild(geminiChildSprite, Vector2.Zero, Vector2.One, bulletTexture: geminiBullet), geminiChildSprite);
        // Bumblebee
        Texture2D bumblebeeBullet = Content.Load<Texture2D>("Sprites/bullet_bumblebee");
        Texture2D bumblebeeSprite = Content.Load<Texture2D>("Sprites/enemy_bumblebee");
        Prefabs.AddPrefab<EnemyBumblebee>(new EnemyBumblebee(bumblebeeSprite, Vector2.Zero, Vector2.One, bulletTexture: bumblebeeBullet), bumblebeeSprite);

        //Hazard
        Texture2D hazardSprite = Content.Load<Texture2D>("Sprites/hazard_a");
        Prefabs.AddPrefab<Hazard>(new Hazard(hazardSprite), hazardSprite);

        // PickUp
        Prefabs.AddPrefab<PickUp>(new PickUp());
        // Shield
        Texture2D shieldSprite = Content.Load<Texture2D>("Sprites/pu_shield");
        Prefabs.AddPrefab<ShieldPowerUp>(shieldSprite);
        // Repair
        Texture2D repairSprite = Content.Load<Texture2D>("Sprites/pu_repair");
        Prefabs.AddPrefab<RepairPowerUp>(repairSprite);

		// UI
		Prefabs.DefineStandardFont(Content.Load<SpriteFont>("Fonts/Standard"));
		Prefabs.AddFont("Title", Content.Load<SpriteFont>("Fonts/Title"));
        Prefabs.AddFont("Button", Content.Load<SpriteFont>("Fonts/ButtonText"));

        _sceneManager.SetDefaultScene(new MainMenuScene());
        _sceneManager.SwitchToDefaultScene(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        _inputHandler.Update();

        // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        //     Exit();

        // TODO: Add your update logic here

        _globalGameTime = gameTime;
        _sceneManager.UpdateChildren(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin(
            SpriteSortMode.BackToFront,
            transformMatrix: ResolutionScaler.ResolutionMatrix
        );

        _sceneManager.DrawChildren(_spriteBatch);

        _spriteBatch.End();
		
		_spriteBatch.Begin(
            SpriteSortMode.Immediate,
            transformMatrix: ResolutionScaler.ResolutionMatrix
        );

        UIHandler.Instance.Draw(_spriteBatch);

        _spriteBatch.End();

        // TODO: Add your drawing code here
        base.Draw(gameTime);
    }
}
