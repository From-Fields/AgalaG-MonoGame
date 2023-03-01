using agalag.game;
using System.Collections.Generic;
using System.Diagnostics;

namespace agalag.engine
{
    public static class CollisionManager 
    {
        public static void CheckCollisions(List<MonoEntity> entities)
        {
            //foreach (var e in entities) { Debug.WriteLine($"entity: {e}"); } // Verificar se bullet ta listado
            List<MonoEntity> colliders = entities.FindAll((e) => e.HasCollider);
            int length = colliders.Count;

            for (int i = 0; i < length - 1; i++)
            {
                for (int j = i + 1; j < length; j++)
                {
                    //if (colliders[i] is Bullet || colliders[j] is EnemyKamikaze)
                    //{
                    //    Debug.WriteLine("Colisor de bullet encontrado");
                    //    Debug.WriteLine($"colisor[i]: ({colliders[i].Transform.position.X}, {colliders[i].Transform.position.Y})");
                    //    Debug.WriteLine($"colisor[j]: ({colliders[j].Transform.position.X}, {colliders[j].Transform.position.Y})");
                    //}
                    if(colliders[i].Collider.HasCollided(colliders[j].Collider))
                    {
                        if (colliders[i] is Bullet || colliders[j] is Bullet)
                        {
                            Debug.WriteLine("Bullet colidiu");
                        }
                        colliders[i].OnCollision(colliders[j]);
                        colliders[j].OnCollision(colliders[i]);
                    }
                }
        }
    }
            }
}