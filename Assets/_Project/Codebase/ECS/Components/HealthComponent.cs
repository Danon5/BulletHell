using Unity.Entities;

namespace BulletHell.ECS.Components
{
    public struct HealthComponent : IComponentData
    {
        public bool invincible;
        public int maxHealth;
        public int health;
    }
}