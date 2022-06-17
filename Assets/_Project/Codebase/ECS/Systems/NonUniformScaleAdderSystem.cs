using BulletHell.ECS.Components;
using Unity.Entities;
using Unity.Transforms;

namespace BulletHell.ECS.Systems
{
    public partial class NonUniformScaleAdderSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;
        
        protected override void OnStartRunning()
        {
            _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            EntityCommandBuffer commandBuffer = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer();
            EntityCommandBuffer.ParallelWriter parallelWriter = commandBuffer.AsParallelWriter();
            
            Entities.ForEach((
                int entityInQueryIndex,
                Entity entity,
                in NonUniformScaleAdderComponent nonUniformScaleAdderComponent) =>
            {
                parallelWriter.AddComponent<NonUniformScale>(entityInQueryIndex, entity);
                parallelWriter.SetComponent(entityInQueryIndex, entity, new NonUniformScale
                {
                    Value = nonUniformScaleAdderComponent.defaultValue
                });
                parallelWriter.RemoveComponent<NonUniformScaleAdderComponent>(entityInQueryIndex, entity);
            }).ScheduleParallel();
            
            _endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}