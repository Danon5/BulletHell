using Unity.Entities;

namespace BulletHell.ECS.SharedData
{
    public struct SpriteAnimationSharedData : ISharedComponentData
    {
        public AnimationId animationId;
    }
}