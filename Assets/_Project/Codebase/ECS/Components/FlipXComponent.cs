using Unity.Entities;

namespace BulletHell.ECS.Components
{
    [GenerateAuthoringComponent]
    public struct FlipXComponent : IComponentData
    {
        public Entity spriteEntity;
        public bool flipped;
    }
}