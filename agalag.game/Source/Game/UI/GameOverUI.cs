using agalag.engine;
using agalag.engine.content;
using agalag.engine.utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agalag.game
{
    class GameOverUI : Singleton<GameOverUI>
    {
        private UIButton _retry, _quit;
        private UISprite _background;
        private UIText _title;
        private UIText _scoreLabel, _scoreValue;

        public void Initialize()
        {
            this._background = new UISprite(null, Vector2.Zero, Color.Black, 0.6f, ResolutionScaler.InternalResolution.Rectangle);
            this._title = new UIText("Game Over", new Vector2(Utils.ScreenWidth / 2, 50f), Prefabs.GetFont("Title"));
            _title.SetColor(Color.MediumVioletRed);
            _title.SetAlign(TextAlign.Center);

            this._retry = new UIButton("Retry", new Vector2(Utils.ScreenWidth / 2, Utils.ScreenHeight / 2 + 180), new Vector2(400, 120),
                action: () => {
                    SceneManager.Instance.SwitchScene(AgalagGame.GetNewScene(AgalagGame.SceneID.EndlessLevel));
                    SceneManager.Instance.SwitchPause(false);
                    Show(false);
                });
            this._quit = new UIButton("Quit", new Vector2(Utils.ScreenWidth / 2, Utils.ScreenHeight / 2 + (180 * 2)), new Vector2(400, 120), hoverColor: Color.DarkRed, 
                action: () => {
                    SceneManager.Instance.SwitchToDefaultScene();
                    SceneManager.Instance.SwitchPause(false);
                });

            _scoreLabel = new UIText("Score:", new Vector2(Utils.ScreenWidth / 2, Utils.ScreenHeight / 2 - 80), Prefabs.GetFont("Button"));
            _scoreLabel.SetAlign(TextAlign.Center);
            _scoreValue = new UIText("0", new Vector2(Utils.ScreenWidth / 2, Utils.ScreenHeight / 2), Prefabs.GetFont("Button"));
            _scoreValue.SetAlign(TextAlign.Center);
            _scoreValue.SetColor(Color.OrangeRed);

            Show(false);
            LevelController.Instance.onGameOver = () => Show(true);
        }

        public void UpdateScore(int score)
        {
            _scoreValue.SetText(score.ToString());
        }

        private void Show(bool show)
        {
            this._title.SetActive(show);
            this._retry.SetActive(show);
            this._scoreLabel.SetActive(show);
            this._scoreValue.SetActive(show);
            this._quit.SetActive(show);
            this._background.SetActive(show);
        }
    }
}
