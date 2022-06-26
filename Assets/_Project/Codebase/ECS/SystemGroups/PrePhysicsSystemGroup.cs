using Unity.Entities;

namespace BulletHell.ECS.SystemGroups
{
    [UpdateBefore(typeof(PhysicsSystemGroup))]
    public sealed class PrePhysicsSystemGroup : ComponentSystemGroup
    {
        
    }
}