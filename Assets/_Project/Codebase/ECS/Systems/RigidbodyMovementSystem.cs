using BulletHell.ECS.Components;
using BulletHell.ECS.SystemGroups;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace BulletHell.ECS.Systems
{
    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    [UpdateBefore(typeof(CollisionResolutionSystem))]
    public partial class RigidbodyMovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((
                ref Translation translation,
                in RigidbodyComponent rigidbody) =>
            {
                translation.Value += new float3(rigidbody.velocity.x, rigidbody.velocity.y, 0f) * GameConstants.TIMESTEP;
            }).ScheduleParallel();
        }
    }
}