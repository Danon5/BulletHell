using Unity.Entities;

namespace BulletHell.ECS.SystemGroups
{
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public sealed class PostPhysicsSystemGroup : ComponentSystemGroup
    {
        
    }
}