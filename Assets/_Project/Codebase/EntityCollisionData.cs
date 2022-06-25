using Unity.Entities;
using Unity.Mathematics;

namespace BulletHell
{
    public struct EntityCollisionData
    {
        public Entity other;
        public float2 point;
        public float2 normal;
    }
}