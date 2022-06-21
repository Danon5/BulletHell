using Unity.Entities;
using Unity.Mathematics;

namespace BulletHell.ECS.Components
{
    public struct RigidbodyComponent : IComponentData
    {
        public float2 velocity;
    }
}