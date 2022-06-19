using Unity.Entities;
using Unity.Mathematics;

namespace BulletHell.ECS.Components
{
    public struct SpriteComponent : IComponentData
    {
        public float4 uvRangeAndOffset;
    }
}