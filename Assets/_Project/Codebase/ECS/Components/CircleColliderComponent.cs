using Unity.Entities;

namespace BulletHell.ECS.Components
{
    public struct CircleColliderComponent : IComponentData
    {
        public float radius;
    }
}