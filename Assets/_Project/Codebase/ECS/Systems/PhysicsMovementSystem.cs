using BulletHell.ECS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

namespace BulletHell.ECS.Systems
{
    public partial class PhysicsMovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((
                ref PhysicsVelocity physicsVelocity,
                in MovementComponent movement) =>
            {
                physicsVelocity.Linear = new float3(movement.velocity.x, movement.velocity.y, 0f);
            }).ScheduleParallel();
        }
    }
}