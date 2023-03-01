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
        Texture2D playerSprite;
        Texture2D kamikazeSprite;

        List<MonoEntity> entities;

        public TestScene(List<SceneLayer> layers = null) : base(layers)
        {
            entities = new List<MonoEntity>();
            AddLayer(new SceneLayer((int)Layer.Default, entities));
            AddLayer(new SceneLayer((int)Layer.Objects, entities));
            AddLayer(new SceneLayer((int)Layer.Entities, entities));
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
            Player player = new Player(playerSprite, new Vector2(960, 540));
            
            EnemyKamikaze enemyK = new EnemyKamikaze(kamikazeSprite, new Vector2(960, 120), Vector2.One, player);
            //Bullet bullet = new Bullet(new Vector2(900, 100), 0, new Vector2(0, 1), 0f);

            Queue<iEnemyAction> queue = new Queue<iEnemyAction>();
            queue.Enqueue(new MoveTowards(1, 1, 1f, 180, 10f, new Vector2(350, 180)));
            queue.Enqueue(new Shoot(2));
            queue.Enqueue(new MoveTowards(5, 1, 0.5f, 40, 1f, player));
            enemyK.Initialize(queue, new WaitSeconds(4), new WaitSeconds(1), enemyK.Transform.position);
            

            this.isInitialized = true;
        }

        public override bool LoadContent(ContentManager content)
        {
            playerSprite = content.Load<Texture2D>("Sprites/player");
            kamikazeSprite = content.Load<Texture2D>("Sprites/kamikaze");

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
    }
}