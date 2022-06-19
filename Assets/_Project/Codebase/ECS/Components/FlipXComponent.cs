using Unity.Entities;

namespace BulletHell.ECS.Components
{
    public struct FlipXComponent : IComponentData
    {
        public Entity spriteEntity;
        public bool flipped;
    }
}