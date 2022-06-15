using Unity.Entities;
using Unity.Mathematics;

namespace BulletHell.ECS.Components
{
    [GenerateAuthoringComponent]
    public struct QuadRendererComponent : IComponentData
    {
        public int2 textureDimensions;
    }
}