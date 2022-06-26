using Unity.Entities;

namespace BulletHell.ECS.SystemGroups
{
    [UpdateAfter(typeof(RenderingSystemGroup))]
    public sealed class EndFrameSystemGroup : ComponentSystemGroup
    {
        
    }
}