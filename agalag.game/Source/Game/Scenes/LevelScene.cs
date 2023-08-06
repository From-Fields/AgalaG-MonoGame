using agalag.engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using agalag.game;
using agalag.game.input;
using System.Diagnostics;
using agalag.engine.content;

namespace agalag.game 
{
    public class LevelScene : Scene
    {
        bool paused = false;
        List<MonoEntity> entities;
        LevelController _controller;

        public LevelScene(iLevel level, List<SceneLayer> layers = null) : base(layers)
        {
            entities = new List<MonoEntity>();
            _controller = new LevelController(level);
            AddLayer(new SceneLayer((int)Layer.Default, null));
            AddLayer(new SceneLayer((int)Layer.Objects, null));
            AddLayer(new SceneLayer((int)Layer.Entities, null));
        }

        public override void Clear()
        {
            _controller.Clear();
                
            foreach (SceneLayer layer in _layers.Values)
                layer.Clear();

            isInitialized = false;
        }

        public override void Draw(SpriteBatch spriteBatch) { }

        public override void FixedUpdate(GameTime gameTime, FixedFrameTime fixedFrameTime) { }

        public override void Initialize()
        {
            // Create Boundaries
            Wall[] bounds = new Wall[] {
                new Wall(new Vector2(100, 1080), new Vector2(-99, 0)), // Left
                new Wall(new Vector2(1920, 100), new Vector2(0, -99)), // Top
                new Wall(new Vector2(100, 1080), new Vector2(1919, 0)), // Right
                new Wall(new Vector2(1920, 100), new Vector2(0, 1079)), // Bottom
            };

            Background background = new Background(Prefabs.GetTextureOfType<Background>(), new Rectangle(0, 0, 1920, 1080));

            Player player = new Player(GetPrefab<Player>(), new Vector2(960, 540), active: true);
            _controller.SetPlayer(player);

            PauseMenu.Instance.Initialize();
            GameOverUI.Instance.Initialize();
            GameplayUI.Instance.Initialize();
            _controller.Initialize();

            this.isInitialized = true;
        }

        public override bool LoadContent(ContentManager content)
        {
            this.isLoaded = true;
            return true;
        }

        public override bool UnloadContent(ContentManager content)
        {
            this.isLoaded = false;
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            if(InputHandler.Instance.GetPause()) {
                SceneManager.Instance.SwitchPause(!paused);
                paused = !paused;
            }
        }
    }
}