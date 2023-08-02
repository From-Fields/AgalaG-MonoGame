using agalag.engine;
using agalag.engine.content;
using agalag.game.input;
using agalag.game.scenes;
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
        private enum States { MainOptions, Settings }
        private States state = States.MainOptions;

        private UIText title;
        private UIButton startButton, settingsButton, exitButton;

        private UIText settingsTitle;
        private UIText fullscreenText, masterText, musicText, sfxText;

        private UIToggleButton toggleFullscreen;
        private UISlider masterSlider, musicSlider, sfxSlider;

        private UIButton backButton;


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
            title = new UIText("AgalaG", new Vector2(Utils.ScreenWidth/2, 50f), Prefabs.GetFont("Title"));
            title.SetColor(Color.OrangeRed);
            title.SetAlign(TextAlign.Center);

            startButton = new UIButton("Start", new Vector2(Utils.ScreenWidth / 2, Utils.ScreenHeight / 2), new Vector2(400, 120), 
                action: () => {
                    SceneManager.Instance.SwitchScene(new LevelScene(GameWaves.GetLevel(new Rectangle(0,0, 1920, 1080), null)));
                });
            settingsButton = new UIButton("Settings", new Vector2(Utils.ScreenWidth / 2, Utils.ScreenHeight / 2 + 180), new Vector2(400, 120),
                action: () => {
                    state = States.Settings;
                    UpdateMainMenuUI();
                });
            exitButton = new UIButton("Exit", new Vector2(Utils.ScreenWidth / 2, Utils.ScreenHeight / 2 + (180*2)), new Vector2(400, 120), 
                hoverColor: Color.DarkRed,
                action: () => {
                    AgalagGame.Instance.Exit();
                });

            settingsTitle = new UIText("Settings", new Vector2(Utils.ScreenWidth / 2, 50f), Prefabs.GetFont("Title"));
            settingsTitle.SetAlign(TextAlign.Center);
            settingsTitle.SetActive(false);

            fullscreenText = new UIText("Fullscreen: ", new Vector2(Utils.ScreenWidth / 2 - 380, Utils.ScreenHeight / 2 - 180), Prefabs.GetFont("Button"));
            fullscreenText.SetActive(false);

            toggleFullscreen = new UIToggleButton(new Vector2(Utils.ScreenWidth / 2 + 100, 380), new Vector2(80, 80), 
                _isChecked =>
                {
                    AgalagGame.Instance.SetFullscreen(_isChecked);
                });
            toggleFullscreen.SetActive(false);

            masterText = new UIText("Master Volume: ", new Vector2(Utils.ScreenWidth / 2 - 380, Utils.ScreenHeight / 2 - 80), Prefabs.GetFont("Button"));
            masterText.SetActive(false);

            masterSlider = new UISlider(new Vector2(Utils.ScreenWidth / 2 + 80, Utils.ScreenHeight / 2 - 60), new Vector2(280, 15), 7,
                action: volume => {
                    AudioSettings.SetMasterVolume(volume);
                });
            masterSlider.SetActive(false);

            musicText = new UIText("Music Volume: ", new Vector2(Utils.ScreenWidth / 2 - 380, Utils.ScreenHeight / 2 + 20), Prefabs.GetFont("Button"));
            musicText.SetActive(false);

            musicSlider = new UISlider(new Vector2(Utils.ScreenWidth / 2 + 80, Utils.ScreenHeight / 2 + 40), new Vector2(280, 15), 7,
                action: volume => {
                    AudioSettings.SetMusicVolume(volume);
                });
            musicSlider.SetActive(false);

            sfxText = new UIText("SFX Volume: ", new Vector2(Utils.ScreenWidth / 2 - 380, Utils.ScreenHeight / 2 + 120), Prefabs.GetFont("Button"));
            sfxText.SetActive(false);

            sfxSlider = new UISlider(new Vector2(Utils.ScreenWidth / 2 + 80, Utils.ScreenHeight / 2 + 140), new Vector2(280, 15), 7,
                action: volume => {
                    AudioSettings.SetSFXVolume(volume);
                });
            sfxSlider.SetActive(false);

            backButton = new UIButton("Back", new Vector2(Utils.ScreenWidth / 2, Utils.ScreenHeight * .80f), new Vector2(350, 100),
                action: () => {
                    state = States.MainOptions;
                    UpdateMainMenuUI();
                });
            backButton.SetActive(false);

            this.isInitialized = true;
        }

        private void UpdateMainMenuUI()
        {
            title.SetActive(state == States.MainOptions);
            startButton.SetActive(state == States.MainOptions);
            settingsButton.SetActive(state == States.MainOptions);
            exitButton.SetActive(state == States.MainOptions);

            settingsTitle.SetActive(state == States.Settings);
            fullscreenText.SetActive(state == States.Settings);
            toggleFullscreen.SetActive(state == States.Settings);
            masterText.SetActive(state == States.Settings);
            masterSlider.SetActive(state == States.Settings);
            musicText.SetActive(state == States.Settings);
            musicSlider.SetActive(state == States.Settings);
            sfxText.SetActive(state == States.Settings);
            sfxSlider.SetActive(state == States.Settings);
            backButton.SetActive(state == States.Settings);
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
            if (state != States.MainOptions && InputHandler.Instance.GetPause())
            {
                state = States.MainOptions;
                UpdateMainMenuUI();
            }
        }

    }
}
