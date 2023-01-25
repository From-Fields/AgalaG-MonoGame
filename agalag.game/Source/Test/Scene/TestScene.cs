using agalag.engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using agalag.game;

namespace agalag.test 
{
    public class TestScene : Scene
    {
        Texture2D playerSprite;

        public override void Clear()
        {
            isInitialized = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public override void FixedUpdate(GameTime gameTime)
        {
            
        }

        public override void Initialize()
        {
            List<MonoEntity> entities = new List<MonoEntity>();
            entities.Add(new Player(playerSprite, new Vector2(20, 20), Vector2.One));
            SceneLayer layer = new SceneLayer(0, entities);

            this.AddLayer(layer);

            this.isInitialized = true;
        }

        public override bool LoadContent(ContentManager content)
        {
            playerSprite = content.Load<Texture2D>("Sprites/player");

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
        { }
    }
}