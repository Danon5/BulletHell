using Unity.Entities;
using Unity.Mathematics;

namespace BulletHell.ECS.Components
{
    public struct MovementComponent : IComponentData
    {
        public float speed;
        public float2 velocity;
    }
}