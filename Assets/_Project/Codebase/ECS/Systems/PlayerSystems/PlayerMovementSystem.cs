using BulletHell.ECS.Components;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace BulletHell.ECS.Systems.PlayerSystems
{
    [UpdateBefore(typeof(RigidbodyMovementSystem))]
    public partial class PlayerMovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Vector2 moveAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

            Entities.ForEach((
                ref Translation translation,
                ref RigidbodyComponent rigidbody,
                in PlayerComponent player) =>
            {
                rigidbody.velocity = moveAxis * 5f;
            }).ScheduleParallel();
        }
    }
}