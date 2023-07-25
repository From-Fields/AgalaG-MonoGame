using System;
using agalag.engine;
using agalag.engine.content;
using agalag.engine.utils;
using Microsoft.Xna.Framework;

namespace agalag.game
{
    public class PauseMenu: Singleton<PauseMenu>
    {
        private UIButton _resume, _retry, _quit;
        private UISprite _background;
        private UIText _title;

        public void Initialize()
        {
            this._background = new UISprite(new Sprite(Prefabs.GetShape(Shapes.Rectangle)), Vector2.Zero, Color.Black, 0.6f, ResolutionScaler.InternalResolution.Rectangle);
            this._title = new UIText("Paused", new Vector2(Utils.ScreenWidth/2, 50f), Prefabs.GetFont("Title"));
            _title.SetColor(Color.OrangeRed);
            _title.SetAlign(TextAlign.Center);

            this._resume = new UIButton("Resume", new Vector2(Utils.ScreenWidth / 2, Utils.ScreenHeight / 2), new Vector2(400, 120), action: () => {
                SceneManager.Instance.SwitchPause(false);
            });
            this._retry = new UIButton("Retry", new Vector2(Utils.ScreenWidth / 2, Utils.ScreenHeight / 2 + 180), new Vector2(400, 120));
            this._quit = new UIButton("Quit", new Vector2(Utils.ScreenWidth / 2, Utils.ScreenHeight / 2 + (180*2)), new Vector2(400, 120), hoverColor: Color.DarkRed, action: () => {
                SceneManager.Instance.SwitchToDefaultScene();
                SceneManager.Instance.SwitchPause(false);
            });

            SwitchPauseMenu(false);
            SceneManager.Instance.onPause += SwitchPauseMenu;
        }

        private void SwitchPauseMenu(bool paused) {
            this._title.SetActive(paused);
            this._resume.SetActive(paused);
            this._retry.SetActive(paused);
            this._quit.SetActive(paused);
            this._background.SetActive(paused);
        }
    }
}