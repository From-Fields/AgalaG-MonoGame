using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agalag.game
{
    public enum EntityTag
    {
       None,
       Player,
       Enemy,
       PlayerBullet,
       PickUp
    }

    public static class Utils
    {
       public static readonly Dictionary<EntityTag, string> Tags = new()
       {
           { EntityTag.None, "unknown" },
           { EntityTag.Player, "Player" },
           { EntityTag.Enemy, "Enemy"},
           { EntityTag.PlayerBullet, "PlayerBullet" },
           { EntityTag.PickUp, "PickUp" },
       };
    }
}
