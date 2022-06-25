using Unity.Entities;

namespace BulletHell.ECS.SystemGroups
{
    [UpdateAfter((typeof(PhysicsSystemGroup)))]
    public class RenderingSystemGroup : ComponentSystemGroup
    {
        
    }
}