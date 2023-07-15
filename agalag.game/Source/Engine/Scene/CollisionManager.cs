using agalag.game;
using System.Collections.Generic;
using System.Diagnostics;

namespace agalag.engine
{
    public static class CollisionManager 
    {
        public static void CheckCollisions(List<MonoEntity> entities)
        {
            List<MonoEntity> colliders = entities.FindAll((e) => e.HasCollider);
            int length = colliders.Count;

            for (int i = 0; i < length - 1; i++)
            {
                for (int j = i + 1; j < length; j++)
                {
                    if(colliders[j].Collider == null || colliders[i].Collider == null)
                        continue;
                    if(!TagUtils.GetInteraction(colliders[j].Tag, colliders[i].Tag))
                        continue;
                    if(colliders[i].Collider.HasCollided(colliders[j].Collider))
                    {
                        
                        // colliders[i].OnCollision(colliders[j]);
                        // colliders[j].OnCollision(colliders[i]);
                        colliders[i].AddCollision(colliders[j]);
                        colliders[j].AddCollision(colliders[i]);
                    }
                }
            }
        }
    }
}