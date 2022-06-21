using Unity.Entities;

namespace BulletHell.ECS.Components
{
    public struct SpriteFlipXComponent : IComponentData
    {
        public bool flipped;
    }
}