using Unity.Entities;

namespace BulletHell.ECS.Components
{
    [GenerateAuthoringComponent]
    public struct FlipXComponent : IComponentData
    {
        public Entity graphicsEntity;
        public bool flipped;
    }
}