using agalag.engine;
using agalag.engine.content;
using agalag.game.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agalag.game.Source.Game.Scenes
{
    public class MainMenuScene : Scene
    {
        public MainMenuScene(List<SceneLayer> layers = null) : base(layers)
        {
            AddLayer(new SceneLayer((int)Layer.Default, null));
        }

        public override void Clear()
        {
            isInitialized = false;
        }

        public override void Draw(SpriteBatch spriteBatch) { }

        public override void FixedUpdate(GameTime gameTime, FixedFrameTime fixedFrameTime) { }

        public override void Initialize()
        {
            UIText title = new UIText("AgalaG", new Vector2(Utils.ScreenWidth/2, Utils.ScreenHeight/4), Prefabs.GetFont("Title"));
            title.SetColor(Color.OrangeRed);
            title.SetAlign(TextAlign.Center);

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
            
        }

    }
}
