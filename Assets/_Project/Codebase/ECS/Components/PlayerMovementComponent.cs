using Unity.Entities;

namespace BulletHell.ECS.Components
{
    [GenerateAuthoringComponent]
    public struct PlayerMovementComponent : IComponentData
    {
        public float speed;
    }
}