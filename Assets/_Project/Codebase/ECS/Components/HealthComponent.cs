using Unity.Entities;

namespace BulletHell.ECS.Components
{
    public struct HealthComponent : IComponentData
    {
        public int health;
    }
}