using BulletHell.ECS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace BulletHell.ECS.Systems
{
    public partial class FlipXSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((
                in FlipXComponent flipX,
                in MovementComponent playerMovement) =>
            {
                float xVel = playerMovement.velocity.x;

                if (math.abs(xVel) > .01)
                {
                    LocalToParent localToParent = EntityManager.GetComponentData<LocalToParent>(flipX.graphicsEntity);
                    
                    float4x4 translationMatrix = float4x4.TRS(
                        localToParent.Position,
                        quaternion.identity, 
                        new float3(xVel < 0f ? -1f : 1f, 1f, 1f));
                    localToParent.Value = translationMatrix;
                    
                    EntityManager.SetComponentData(flipX.graphicsEntity, localToParent);
                }
            }).WithoutBurst().Run();
        }
    }
}