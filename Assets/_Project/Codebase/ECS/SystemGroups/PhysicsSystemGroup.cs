using Unity.Entities;

namespace BulletHell.ECS.SystemGroups
{
    [UpdateAfter((typeof(GameplaySystemGroup)))]
    public sealed class PhysicsSystemGroup : ComponentSystemGroup
    {
        
    }
}