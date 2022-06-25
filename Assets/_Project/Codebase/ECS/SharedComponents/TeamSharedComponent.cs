using Unity.Entities;

namespace BulletHell.ECS.SharedComponents
{
    public struct TeamSharedComponent : ISharedComponentData
    {
        public TeamId teamId;
    }
}