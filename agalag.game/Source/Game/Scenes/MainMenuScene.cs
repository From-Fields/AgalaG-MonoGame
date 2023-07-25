using agalag.engine;
using agalag.engine.content;
using agalag.game.input;
using agalag.test;
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
            UIHandler.Instance.Clean();
            isInitialized = false;
        }

        public override void Draw(SpriteBatch spriteBatch) { }

        public override void FixedUpdate(GameTime gameTime, FixedFrameTime fixedFrameTime) { }

        public override void Initialize()
        {
            UIText title = new UIText("AgalaG", new Vector2(Utils.ScreenWidth/2, 50f), Prefabs.GetFont("Title"));
            title.SetColor(Color.OrangeRed);
            title.SetAlign(TextAlign.Center);

            _ = new UIButton("Start", new Vector2(Utils.ScreenWidth / 2, Utils.ScreenHeight / 2), new Vector2(400, 120), action: () => {
                SceneManager.Instance.SwitchScene(new TestScene());
            });
            _ = new UIButton("Settings", new Vector2(Utils.ScreenWidth / 2, Utils.ScreenHeight / 2 + 180), new Vector2(400, 120));
            _ = new UIButton("Exit", new Vector2(Utils.ScreenWidth / 2, Utils.ScreenHeight / 2 + (180*2)), new Vector2(400, 120), hoverColor: Color.DarkRed);

            this.isInitialized = true;
        }

        public override bool LoadContent(ContentManager content)
        {
            this.isLoaded = true;
            return true;
        }

        public override bool UnloadContent(ContentManager content)
        {
            UIHandler.Instance.Clean();

            this.isLoaded = false;
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            
        }

    }
}
