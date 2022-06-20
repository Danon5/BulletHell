using Unity.Entities;

namespace BulletHell.ECS.Components
{
    public struct SpriteAnimatorComponent : IComponentData
    {
        public float animationTime;
        public float animationTimeScale;
    }
}