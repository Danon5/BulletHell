using Unity.Entities;
using Unity.Transforms;

namespace BulletHell.ECS.SystemGroups
{
    [UpdateBefore(typeof(TransformSystemGroup))]
    public sealed class PhysicsSystemGroup : ComponentSystemGroup
    {
        
    }
}