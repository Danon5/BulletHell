using BulletHell.ECS.Components;
using BulletHell.ECS.SystemGroups;
using Unity.Entities;

namespace BulletHell.ECS.Systems
{
    [UpdateInGroup((typeof(EndFrameSystemGroup)))]
    public partial class NoHealthDestroySystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;

        protected override void OnCreate()
        {
            _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            EntityCommandBuffer commandBuffer = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer();

            Entities.ForEach((
                Entity entity,
                HealthComponent health) =>
            {
                if (health.health <= 0)
                    commandBuffer.DestroyEntity(entity);
            }).Schedule();
            
            Dependency.Complete();
        }
    }
}