using BulletHell.ECS.Components;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace BulletHell.ECS.Systems.PlayerSystems
{
    [UpdateBefore(typeof(PhysicsMovementSystem))]
    public partial class PlayerMovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Vector2 moveAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

            Entities.ForEach((
                ref Translation translation,
                ref MovementComponent movement,
                in PlayerComponent player) =>
            {
                movement.velocity = moveAxis * movement.speed;
            }).ScheduleParallel();
        }
    }
}