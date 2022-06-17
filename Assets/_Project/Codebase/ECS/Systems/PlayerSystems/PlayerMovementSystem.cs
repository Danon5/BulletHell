using BulletHell.ECS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace BulletHell.ECS.Systems.PlayerSystems
{
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
                translation.Value += new float3(movement.velocity.x, movement.velocity.y, 0f) * GameConstants.TARGET_TIMESTEP;
            }).Run();
        }
    }
}