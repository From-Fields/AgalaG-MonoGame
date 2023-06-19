using agalag.engine.content;
using Microsoft.Xna.Framework.Graphics;

namespace agalag.game
{
    
public class RepairPowerUp : PowerUp
{
    public override Texture2D Sprite => Prefabs.GetTextureOfType<RepairPowerUp>();
    public override bool IsInstant => true;

    public override void OnPickup(Player player) 
    {
        player.Heal(player.MaxHealth);
    }
    public override void OnEnd() { }  // Do Nothing
}
}