using Unity.Entities;

namespace BulletHell.ECS.SystemGroups
{
    [UpdateAfter((typeof(PostPhysicsSystemGroup)))]
    public class RenderingSystemGroup : ComponentSystemGroup
    {
        
    }
}