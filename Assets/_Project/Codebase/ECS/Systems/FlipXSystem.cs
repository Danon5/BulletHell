using BulletHell.ECS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace BulletHell.ECS.Systems
{
    public partial class FlipXSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;

        protected override void OnStartRunning()
        {
            _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            EntityCommandBuffer.ParallelWriter parallelWriter = 
                _endSimulationEntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();

            Entities.ForEach((
                int entityInQueryIndex,
                ref FlipXComponent flipX,
                in MovementComponent playerMovement) =>
            {
                float xVel = playerMovement.velocity.x;

                if (math.abs(xVel) <= .01) return;

                bool shouldBeFlipped = xVel < 0f;

                if (shouldBeFlipped == flipX.flipped) return;

                NonUniformScale newScale = new NonUniformScale
                {
                    Value = new float3(shouldBeFlipped ? -1f : 1f, 1f, 1f)
                };

                flipX.flipped = shouldBeFlipped;
                parallelWriter.SetComponent(entityInQueryIndex, flipX.graphicsEntity, newScale);
            }).ScheduleParallel();
            
            _endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}