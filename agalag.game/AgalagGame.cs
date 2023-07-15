﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using agalag.engine;
using agalag.game.input;
using System.Diagnostics;
using agalag.engine.content;
using Microsoft.Xna.Framework.Content;

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

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Prefabs.AddPrefab<Bullet>(Content.Load<Texture2D>("Sprites/bullet_player"));
        
        Texture2D playerSprite = Content.Load<Texture2D>("Sprites/player");
        Prefabs.AddPrefab<Player>(new Player(playerSprite, Vector2.Zero), playerSprite);

        Texture2D kamikazeSprite = Content.Load<Texture2D>("Sprites/enemy_kamikaze");
        Prefabs.AddPrefab<EnemyKamikaze>(new EnemyKamikaze(kamikazeSprite, Vector2.Zero, Vector2.One), kamikazeSprite);
        
        Prefabs.AddPrefab<EnemyGemini>(new EnemyGemini(null, Vector2.Zero, Vector2.One));

        Texture2D geminiBullet = Content.Load<Texture2D>("Sprites/bullet_gemini");
        Texture2D geminiChildSprite = Content.Load<Texture2D>("Sprites/enemy_gemini");
        Prefabs.AddPrefab<EnemyGeminiChild>(new EnemyGeminiChild(geminiChildSprite, Vector2.Zero, Vector2.One, bulletTexture: geminiBullet), geminiChildSprite);

        Texture2D bumblebeeBullet = Content.Load<Texture2D>("Sprites/bullet_bumblebee");
        Texture2D bumblebeeSprite = Content.Load<Texture2D>("Sprites/enemy_bumblebee");
        Prefabs.AddPrefab<EnemyBumblebee>(new EnemyBumblebee(bumblebeeSprite, Vector2.Zero, Vector2.One, bulletTexture: bumblebeeBullet), bumblebeeSprite);

        _sceneManager.SetDefaultScene(new test.TestScene());
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

        // TODO: Add your drawing code here
        base.Draw(gameTime);
    }
}
