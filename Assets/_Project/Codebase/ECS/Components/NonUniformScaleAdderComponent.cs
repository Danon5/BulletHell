using Unity.Entities;
using Unity.Mathematics;

namespace BulletHell.ECS.Components
{
    [GenerateAuthoringComponent]
    public struct NonUniformScaleAdderComponent : IComponentData
    {
        public float3 defaultValue;
    }
}