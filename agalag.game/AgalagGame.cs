using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using agalag.engine;
using agalag.game.input;
using System.Diagnostics;
using agalag.engine.content;
using Microsoft.Xna.Framework.Content;
using agalag.game.Source.Game.Scenes;
using Microsoft.Xna.Framework.Audio;
using agalag.game.scenes;

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

    private Effect _squareOutlineShader;
    public static AgalagGame Instance { get; private set; }

    public enum SceneID { MainMenu, EndlessLevel };

    public AgalagGame()
    {
        _graphics = new GraphicsDeviceManager(this);

        SetResolution();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _inputHandler = InputHandler.Instance;
        Prefabs.DefineContent(Content);

        Instance = this;
    }

    public void SetFullscreen(bool fullscreen)
    {
        _isFullscreen = fullscreen;
        SetResolution();
    }

    public static Scene GetNewScene(SceneID id)
    {
        return (id) switch {
            SceneID.MainMenu => new MainMenuScene(),
            SceneID.EndlessLevel => new LevelScene(GameWaves.GetLevel(new Rectangle(0, 0, 1920, 1080), null)),
            _ => throw new System.Exception("No Scene found")
        };;
    }
    
    private void SetResolution()
    {
        ResolutionScaler.SetResolution(
            _graphics,
            _internalResolutionWidth, _internalResolutionHeight,
            _finalResolutionWidth, _finalResolutionHeight,
            _isFullscreen
        );
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

        LoadPlayer();
        LoadEnemies();
        LoadPickUps();
        LoadUI();

        Prefabs.AddShape(new Texture2D(GraphicsDevice, 80, 30), Shapes.Rectangle);
		
        Texture2D background = Content.Load<Texture2D>("Sprites/bg");
        Prefabs.AddTexture<Background>(background);

        _squareOutlineShader = Content.Load<Effect>("Shaders/Outline");

        Matrix projection = Matrix.CreateOrthographicOffCenter(0, _internalResolutionWidth, _internalResolutionHeight, 0, 0, 1);
        _squareOutlineShader.Parameters["WorldViewProjection"].SetValue(Matrix.Identity * projection);

        _sceneManager.SetDefaultScene(GetNewScene(SceneID.MainMenu));
        _sceneManager.SwitchToDefaultScene(Content);
    }
    private void LoadPlayer() 
    {

        //Weapons
        Prefabs.AddTexture<Bullet>(Content.Load<Texture2D>("Sprites/bullet_player"));
        Prefabs.AddSprite("missile", Content.Load<Texture2D>("Sprites/missilePlayer"));

        
        SoundEffect explosionSound = Content.Load<SoundEffect>("Sounds/Shot/sfx_explosion"); 
        Prefabs.AddTexture<Explosion>(Content.Load<Texture2D>("Sprites/explosion"));
        Prefabs.AddSound<Explosion>(explosionSound);
        
        //Player
        SoundEffect playerDmgSound = Content.Load<SoundEffect>("Sounds/Damage/sfx_dmg_player"); 
        SoundEffect playerDeathSound = Content.Load<SoundEffect>("Sounds/Death/sfx_death_player"); 
        SoundEffect playerMoveSound = Content.Load<SoundEffect>("Sounds/Movement/sfx_move_player"); 
        SoundEffect playerShotSound = Content.Load<SoundEffect>("Sounds/Shot/sfx_shot_player"); 
        SoundEffect powerUpSound = Content.Load<SoundEffect>("Sounds/PickUp/sfx_powerup");
        Texture2D playerSprite = Content.Load<Texture2D>("Sprites/player");
        Prefabs.AddSprite("playerShip", playerSprite);

        Prefabs.AddPrefab(
            new Player(
                playerSprite, Vector2.Zero,
                audioManager: new EntityAudioManager(
                    dmgSound: playerDmgSound,
                    deathSound: playerDeathSound,
                    moveSound: playerMoveSound, 
                    shotSound: playerShotSound,
                    powerUpSound: powerUpSound
                )
            ),
            playerSprite
        );

    }
    private void LoadEnemies() 
    {
        SoundEffect deathSound = Content.Load<SoundEffect>("Sounds/Death/sfx_death_enemy");

        // Kamikaze
        Texture2D kamikazeSprite = Content.Load<Texture2D>("Sprites/enemy_kamikaze");
        SoundEffect kamiMoveSound = Content.Load<SoundEffect>("Sounds/Movement/sfx_move_kamikaze"); 
        Prefabs.AddPrefab<EnemyKamikaze>(
            new EnemyKamikaze(
                kamikazeSprite, Vector2.Zero, Vector2.One, 
                audioManager: new EntityAudioManager(deathSound: deathSound, moveSound: kamiMoveSound)
            ),
            kamikazeSprite
        );
        // Gemini
        SoundEffect geminiMoveSound = Content.Load<SoundEffect>("Sounds/Movement/sfx_move_gemini"); 
        Prefabs.AddPrefab<EnemyGemini>(
            new EnemyGemini(
                null, Vector2.Zero, Vector2.One,
                audioManager: new EntityAudioManager(moveSound: geminiMoveSound)
            )
        );
        // GeminiChild
        Texture2D geminiBullet = Content.Load<Texture2D>("Sprites/bullet_gemini");
        Texture2D geminiChildSprite = Content.Load<Texture2D>("Sprites/enemy_gemini");
        SoundEffect geminiShotSound = Content.Load<SoundEffect>("Sounds/Shot/sfx_shot_gemini"); 
        Prefabs.AddPrefab<EnemyGeminiChild>(
            new EnemyGeminiChild(
                geminiChildSprite, Vector2.Zero, Vector2.One, bulletTexture: geminiBullet,
                audioManager: new EntityAudioManager(deathSound: deathSound, shotSound: geminiShotSound)
            ), 
            geminiChildSprite
        );
        // Bumblebee
        Texture2D bumblebeeBullet = Content.Load<Texture2D>("Sprites/bullet_bumblebee");
        Texture2D bumblebeeSprite = Content.Load<Texture2D>("Sprites/enemy_bumblebee");
        SoundEffect bumbleMoveSound = Content.Load<SoundEffect>("Sounds/Movement/sfx_move_bumble"); 
        SoundEffect bumbleShotSound = Content.Load<SoundEffect>("Sounds/Shot/sfx_shot_bumble"); 
        Prefabs.AddPrefab<EnemyBumblebee>(
            new EnemyBumblebee(
                bumblebeeSprite, Vector2.Zero, Vector2.One, bulletTexture: bumblebeeBullet,
                audioManager: new EntityAudioManager(deathSound: deathSound, moveSound: bumbleMoveSound, shotSound: bumbleShotSound)
            ), 
            bumblebeeSprite
        );

        //Hazard
        Texture2D hazardSprite = Content.Load<Texture2D>("Sprites/hazard_a");
        SoundEffect hazardBounceSound = Content.Load<SoundEffect>("Sounds/PickUp/sfx_bounce"); 
        Prefabs.AddPrefab<Hazard>(
            new Hazard(
                hazardSprite,
                audioManager: new EntityAudioManager(deathSound: deathSound, bounceSound: hazardBounceSound)
            ), 
            hazardSprite
        );
    }
    private void LoadPickUps() 
    {
        // Sounds
        SoundEffect pickupSound = Content.Load<SoundEffect>("Sounds/PickUp/sfx_bounce"); 
        Prefabs.AddSound<PickUp>(pickupSound);
        SoundEffect shieldSound = Content.Load<SoundEffect>("Sounds/Damage/sfx_dmg_shield");
        Prefabs.AddSound<ShieldPowerUp>(shieldSound);

        // PickUp
        Prefabs.AddPrefab<PickUp>(new PickUp(audioManager: new EntityAudioManager(bounceSound: pickupSound)));
        // Shield
        Texture2D shieldSprite = Content.Load<Texture2D>("Sprites/pu_shield");
        Prefabs.AddTexture<ShieldPowerUp>(shieldSprite);
        // Repair
        Texture2D repairSprite = Content.Load<Texture2D>("Sprites/pu_repair");
        Prefabs.AddTexture<RepairPowerUp>(repairSprite);
        // Repair
        Texture2D multishotSprite = Content.Load<Texture2D>("Sprites/pu_multishot");
        Prefabs.AddTexture<TripleMachineGunPowerUp>(multishotSprite);
    }
    private void LoadUI()
    {
		// UI
		Prefabs.DefineStandardFont(Content.Load<SpriteFont>("Fonts/Standard"));
		Prefabs.AddFont("Title", Content.Load<SpriteFont>("Fonts/Title"));
        Prefabs.AddFont("Button", Content.Load<SpriteFont>("Fonts/ButtonText"));
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
            samplerState: SamplerState.LinearWrap,
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

        _spriteBatch.Begin(
            SpriteSortMode.Immediate,
            transformMatrix: ResolutionScaler.ResolutionMatrix,
            effect: _squareOutlineShader,
            blendState: BlendState.NonPremultiplied
        );

        UIHandler.Instance.DrawWithEffect(_spriteBatch);

        _spriteBatch.End();

        // TODO: Add your drawing code here
        base.Draw(gameTime);
    }
}
