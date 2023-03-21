
namespace agalag.game
{
    public interface iEnemy: iEntity
    {
        public float DesiredSpeed { get; }
        public float CurrentAcceleration { get; }
    }
}