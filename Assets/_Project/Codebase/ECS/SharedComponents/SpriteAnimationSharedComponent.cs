using Unity.Entities;

namespace BulletHell.ECS.SharedComponents
{
    public struct SpriteAnimationSharedComponent : ISharedComponentData
    {
        public AnimationId animationId;
    }
}