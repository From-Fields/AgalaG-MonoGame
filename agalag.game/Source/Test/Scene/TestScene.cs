using agalag.engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using agalag.game;
using agalag.game.input;
using System.Diagnostics;
using agalag.engine.content;

namespace agalag.test 
{
    public class TestScene : Scene
    {
        bool paused = false;
        List<MonoEntity> entities;
        List<WaveController> _waves;

        public TestScene(List<SceneLayer> layers = null) : base(layers)
        {
            entities = new List<MonoEntity>();
            _waves = new List<WaveController>();
            AddLayer(new SceneLayer((int)Layer.Default, null));
            AddLayer(new SceneLayer((int)Layer.Objects, null));
            AddLayer(new SceneLayer((int)Layer.Entities, null));
        }

        public override void Clear()
        {
            foreach (WaveController wave in _waves)
                wave.Clear();
                
            foreach (SceneLayer layer in _layers.Values)
                layer.Clear();

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
            // Create Boundaries
            Wall[] bounds = new Wall[] {
                new Wall(new Vector2(100, 1080), new Vector2(-99, 0)), // Left
                new Wall(new Vector2(1920, 100), new Vector2(0, -99)), // Top
                new Wall(new Vector2(100, 1080), new Vector2(1919, 0)), // Right
                new Wall(new Vector2(1920, 100), new Vector2(0, 1079)), // Bottom
            };

            Background background = new Background(Prefabs.GetTextureOfType<Background>(), new Rectangle(0, 0, 1920, 1080));


            Player player = new Player(GetPrefab<Player>(), new Vector2(960, 540), active: true);

            // PickUp pickUp = EntityPool<PickUp>.Instance.Pool.Get();

            // pickUp.Initialize(new RepairPowerUp(), new Vector2(60, 60), new Vector2(0.5f, 0.5f));

            CreateWave();

            // EnemyKamikaze enemyK = EntityPool<EnemyKamikaze>.Instance.Pool.Get();
            // EnemyGemini enemyK = EntityPool<EnemyGemini>.Instance.Pool.Get();
            // Bullet bullet = new Bullet(new Vector2(900, 100), 0, new Vector2(0, 1), 0f);

            // Queue<iEnemyAction> queue = new Queue<iEnemyAction>();
            // queue.Enqueue(new MoveTowards(new Vector2(350, 180)));
            // queue.Enqueue(new Shoot(2));
            // queue.Enqueue(new MoveTowards(player));
            // enemyK.Initialize(queue, new WaitSeconds(2), new MoveTowards(player),  new Vector2(960, 120), new Rectangle(Point.Zero, new Point(1920, 1080)));

            
            // enemyK = new EnemyKamikaze(kamikazeSprite, new Vector2(1260, 120), Vector2.One, player);
            // enemyK.Initialize(queue, new WaitSeconds(4), new WaitSeconds(1), enemyK.Transform.position);
            // enemyK = new EnemyKamikaze(kamikazeSprite, new Vector2(660, 120), Vector2.One, player);
            // enemyK.Initialize(queue, new WaitSeconds(4), new WaitSeconds(1), enemyK.Transform.position);

            // System.Diagnostics.Debug.WriteLine(this._layers[(int)Layer.Entities].Entities.Count);

            PauseMenu.Instance.Initialize();

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
            if(InputHandler.Instance.GetPause()) {
                SceneManager.Instance.SwitchPause(!paused);
                paused = !paused;
            }
        }

        private void CreateWave()
        {
            WaveController wave = new WaveController(
                10  ,  
                new Rectangle(Point.Zero, new Point(1920, 1080)), 
                new List<iWaveUnit>(new iWaveUnit[] {
                    new WaveUnit<EnemyBumblebee>(
                        new Vector2(700, -64),
                        new MoveAndShoot(new Vector2(700, 180), 0.7f, 1, 0.9f),
                        new MoveTowards(new Vector2(-450, 500), 1.5f),
                        new Queue<iEnemyAction>(new [] {
                            new MoveTowards(new Vector2(450, 500), 1.5f, 1, 0.9f)
                        })
                    ),
                    //     new WaveUnit<EnemyBumblebee>(
                    //         new Vector2(1920 - 700, -64),
                    //         new MoveAndShoot(new Vector2(1920 - 700, 180), 1, 1, 0.9f),
                    //         new MoveTowards(new Vector2(1920 + 450, 500)),
                    //         new Queue<iEnemyAction>(new[] {
                    //             new MoveTowards(new Vector2(1920 - 450, 500), 1, 1, 0.9f)
                    //         })
                    //     )
                    // }));

                    // WaveController wave = new WaveController(10, new List<iWaveUnit>(new iWaveUnit[] {
                    new WaveUnit<EnemyGemini>(
                        new Vector2(1920 / 2, 100),
                        new WaitSeconds(1),
                        new MoveTowards(new Vector2(-450, 500), 1.5f),
                        new Queue<iEnemyAction>(new iEnemyAction[] {
                            new MoveTowards(new Vector2(1920 - 450, 500), 1, 1, 0.9f),
                            new WaitSeconds(1)
                        })
                    ),
                    new WaveHazard(
                        EntityPool<Hazard>.Instance.Pool.Get(),
                        new Vector2(20, -20), 
                        new Vector2(0.2f, 1), 
                        maxBounces: 3
                    )
                })
            );

            wave.onWaveDone += CreateWave;

            wave.Initialize();
            _waves.Add(wave);
        }
    }
}