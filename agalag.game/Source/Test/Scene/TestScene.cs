using agalag.engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using agalag.game;
using agalag.game.input;
using System.Diagnostics;

namespace agalag.test 
{
    public class TestScene : Scene
    {
        private readonly List<MonoEntity> entities;

        public TestScene(List<SceneLayer> layers = null) : base(layers)
        {
            entities = new List<MonoEntity>();
            AddLayer(new SceneLayer((int)Layer.Default, null));
            AddLayer(new SceneLayer((int)Layer.Objects, null));
            AddLayer(new SceneLayer((int)Layer.Entities, null));
        }

        public override void Clear()
        {
            isInitialized = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public override void FixedUpdate(GameTime gameTime, FixedFrameTime fixedFrameTime)
        {
            
        }

        public override void Initialize()
        {
            Player player = new Player(GetPrefab<Player>(), new Vector2(960, 540));

            CreateWave();

            //new UIText("This is not a test", new Vector2(0, 0));

            // EnemyKamikaze enemyK = new EnemyKamikaze(GetPrefab<EnemyKamikaze>(), true);
            //Bullet bullet = new Bullet(new Vector2(900, 100), 0, new Vector2(0, 1), 0f);

            // Queue<iEnemyAction> queue = new Queue<iEnemyAction>();
            // queue.Enqueue(new MoveTowards(1, 1, 1f, 180, 10f, new Vector2(350, 180)));
            // queue.Enqueue(new Shoot(2));
            // queue.Enqueue(new MoveTowards(5, 1, 0.5f, 40, 1f, player));
            // enemyK.Initialize(queue, new WaitSeconds(2), new MoveTowards(5, 1, 0.5f, 40, 1f, player),  new Vector2(960, 120));

            
            // enemyK = new EnemyKamikaze(kamikazeSprite, new Vector2(1260, 120), Vector2.One, player);
            // enemyK.Initialize(queue, new WaitSeconds(4), new WaitSeconds(1), enemyK.Transform.position);
            // enemyK = new EnemyKamikaze(kamikazeSprite, new Vector2(660, 120), Vector2.One, player);
            // enemyK.Initialize(queue, new WaitSeconds(4), new WaitSeconds(1), enemyK.Transform.position);

            // System.Diagnostics.Debug.WriteLine(this._layers[(int)Layer.Entities].Entities.Count);

            this.isInitialized = true;
        }

        public override bool LoadContent(ContentManager content)
        {
            this.isLoaded = true;
            return true;
        }

        public override bool UnloadContent(ContentManager content)
        {
            content.UnloadAsset("Sprites/player");
            this.isLoaded = false;
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            if(InputHandler.Instance.GetPause())
                Debug.WriteLine("pause");
        }

        private void CreateWave()
        {
            WaveController wave = new WaveController(10, new List<iWaveUnit>(new[] {
                new WaveUnit<EnemyKamikaze>(
                    new Vector2(700, -64),
                    new MoveTowards(new Vector2(700, 180), 0.7f, 1, 0.9f),
                    new MoveTowards(new Vector2(-450, 500), 1.5f),
                    new Queue<iEnemyAction>(new [] {
                        new MoveTowards(new Vector2(450, 500), 1.5f, 1, 0.9f)
                    })
                ),
                new WaveUnit<EnemyKamikaze>(
                    new Vector2(1920 - 700, -64),
                    new MoveTowards(new Vector2(1920 - 700, 180), 1, 1, 0.9f),
                    new MoveTowards(new Vector2(1920 + 450, 500)),
                    new Queue<iEnemyAction>(new[] {
                        new MoveTowards(new Vector2(1920 - 450, 500), 1, 1, 0.9f)
                    })
                )
            }));

            wave.onWaveDone += CreateWave;

            wave.Initialize();
        }
    }
}