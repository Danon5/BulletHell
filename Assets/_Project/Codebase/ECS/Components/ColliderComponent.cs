using Unity.Entities;

namespace BulletHell.ECS.Components
{
    public struct ColliderComponent : IComponentData
    {
        public ColliderType colliderType;
        public bool isTrigger;
        public bool tracksCollisions;
        public float circleColliderRadius;
    }
}