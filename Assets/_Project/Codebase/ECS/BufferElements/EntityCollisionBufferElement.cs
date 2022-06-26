using Unity.Entities;

namespace BulletHell.ECS.BufferElements
{
    public struct EntityCollisionBufferElement : IBufferElementData
    {
        public EntityCollisionData value;
    }
}