﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using agalag.engine;
using agalag.game.input;
using System.Diagnostics;
using agalag.game.prefabs;

namespace agalag.game;

public class AgalagGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SceneManager _sceneManager = SceneManager.Instance;
    private InputHandler _inputHandler;

    public static GameTime _globalGameTime;
    public static GameTime GlobalGameTime => _globalGameTime;

    public AgalagGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _inputHandler = InputHandler.Instance;
        Prefabs.DefineContent(Content);
    }

    protected override void Initialize()
    {
        _sceneManager.SetDefaultScene(new test.TestScene());
        _sceneManager.SwitchToDefaultScene(Content);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
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
            SpriteSortMode.BackToFront
        );

        _sceneManager.DrawChildren(_spriteBatch);

        _spriteBatch.End();

        // TODO: Add your drawing code here
        base.Draw(gameTime);
    }
}