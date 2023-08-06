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
    class GameplayUI : Singleton<GameplayUI>
    {
        private List<UISprite> lifes = new();
        private List<UISprite> shields = new();

        private UISprite weaponIcon;
        private UIText ammoText;

        private UIText scoreCounter;

        public void Initialize()
        {
            for (int i = 0; i < 3; i++)
            {
                var pos = new Vector2(60 + 60 * i, Utils.ScreenHeight - 120);
                var lifeIcon = new UISprite(Prefabs.GetSprite(spriteKey: "playerShip"), pos, Color.White, scale: Vector2.One * 0.5f);
                lifes.Add(lifeIcon);
            }

            for (int i = 0; i < 6; i++)
            {
                var pos = new Vector2(60 + 60 * i, Utils.ScreenHeight - 60);
                var shieldIcon = new UISprite(Prefabs.GetSprite<ShieldPowerUp>(), pos, Color.White, scale: Vector2.One);
                shields.Add(shieldIcon);
                shieldIcon.SetActive(false);
            }

            weaponIcon = new UISprite(Prefabs.GetSprite<Bullet>(),
                new Vector2(Utils.ScreenWidth - 360, Utils.ScreenHeight - 120), Color.White, scale: Vector2.One * .8f);
            ammoText = new UIText("-", new Vector2(Utils.ScreenWidth - 120, Utils.ScreenHeight - 150), Prefabs.GetFont("Button"));
            ammoText.SetAlign(TextAlign.Center);

            _ = new UIText("Score:", new Vector2(Utils.ScreenWidth - 360, Utils.ScreenHeight - 60));
            scoreCounter = new UIText("99999999", new Vector2(Utils.ScreenWidth - 240, Utils.ScreenHeight - 80), Color.Orange, Prefabs.GetFont("Button"));
        }

        public void UpdateLifeHUD(int life)
        {
            for (int i = 0; i < lifes.Count; i++)
            {
                lifes[i].SetActive(life - 1 >= i);
            }
        }

        public void UpdateShieldHUD(int shield)
        {
            for (int i = 0; i < shields.Count; i++)
            {
                shields[i].SetActive(shield - 1 >= i);
            }
        }

        public void UpdateWeaponIcon(Sprite icon)
        {
            weaponIcon.SetSprite(icon);
        }

        public void UpdateAmmoCount(string ammoCount)
        {
            ammoText.SetText(ammoCount);
        }

        public void UpdateScore(int score)
        {
            scoreCounter.SetText(score.ToString());
        }
    }
}
