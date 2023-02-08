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
            List<MonoEntity> entities = new List<MonoEntity>();
            Player player = new Player(playerSprite, new Vector2(960, 540));
            entities.Add(player);
            entities.Add(new TestEnemy(kamikazeSprite, new Vector2(960, 120), Vector2.One, player));
            //entities.Add(new SpriteTest(playerSprite, new Vector2(150, 150)));
            SceneLayer layer = new SceneLayer(0, entities);

            this.AddLayer(layer);

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