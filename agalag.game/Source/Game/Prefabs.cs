using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game.prefabs {
    public static class Prefabs {
        private static ContentManager _content;

        public static void DefineContent(ContentManager content) => _content = content;

        public static Texture2D StandardBullet => StandardBulletTeste();

        private static Texture2D StandardBulletTeste()
        {
            if (_content == null)
            {
                Debug.WriteLine("Deu ruim!");
            }
            return _content.Load<Texture2D>("Sprites/bullet_player");
        }
    }
}
