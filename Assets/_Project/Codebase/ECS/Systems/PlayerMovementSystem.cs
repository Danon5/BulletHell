using BulletHell.ECS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace BulletHell.ECS.Systems
{
    public partial class PlayerMovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float3 moveAxis = new float3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
            float deltaTime = Time.DeltaTime;
            
            Entities.ForEach((
                ref Translation translation,
                in PlayerMovementComponent playerMovement) =>
            {
                translation.Value += moveAxis * playerMovement.speed * deltaTime;
            }).Run();
        }
    }
}