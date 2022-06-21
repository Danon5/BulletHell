using BulletHell.ECS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace BulletHell.ECS.Systems
{
    [UpdateBefore(typeof(SpriteRenderSystem))]
    public partial class SpriteFlipXSystem : SystemBase
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
                Entity entity,
                ref SpriteFlipXComponent flipX,
                ref SpriteComponent sprite,
                in RigidbodyComponent rigidbody) =>
            {
                float xVel = rigidbody.velocity.x;

                if (math.abs(xVel) <= .01) return;

                bool shouldBeFlipped = xVel < 0f;

                if (shouldBeFlipped == flipX.flipped) return;

                Rotation newRotation = new Rotation
                {
                    Value = new quaternion(0f, shouldBeFlipped ? 1f : 0f, 0f, 1f)
                };

                flipX.flipped = shouldBeFlipped;
                parallelWriter.SetComponent(entityInQueryIndex, entity, newRotation);
            }).ScheduleParallel();

            _endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}