using System;
using Microsoft.Xna.Framework;

namespace agalag.engine
{
    public class Collision
    {
        private CollisionEntity _first, _second;
        private Vector2 _normal;

        public CollisionEntity Self => _first;
        public CollisionEntity Other => _second;
        public Vector2 Normal => _normal;

        public bool IsSolid => _second.isSolid;

        public Collision(MonoEntity first, MonoEntity second)
        {
            this._first = new CollisionEntity(first);
            this._second = new CollisionEntity(second);
            this._normal = GetNormal(first, second);
        }

        private Vector2 GetNormal(MonoEntity first, MonoEntity second)
        {
            Vector2 contact = second.Collider.ClosestPoint(first.Transform.position);
            Vector2 normal = (first.Transform.position - contact);
            normal.Normalize();

            return normal;
        }
    }

    public class CollisionEntity
    {
        public Vector2 velocity { get; private set; }
        public Vector2 position { get; private set; }
        public bool isSolid { get; private set; }
        public bool isStatic { get; private set; }

        public MonoEntity entity { get; private set; }
        
        public CollisionEntity(MonoEntity entity)
        {
            velocity = entity.Transform.velocity;
            position = entity.Transform.position;
            isSolid = entity.Collider.IsSolid;
            this.entity = entity;
        }
    }
}