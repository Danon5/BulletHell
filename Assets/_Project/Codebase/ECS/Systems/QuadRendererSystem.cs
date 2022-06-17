using BulletHell.ECS.Components;
using BulletHell.ECS.Components.Singletons;
using BulletHell.ECS.Tags;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace BulletHell.ECS.Systems
{
    public partial class QuadRendererSystem : SystemBase
    {
        private GameDataSingletonComponent _gameData;
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;

        protected override void OnStartRunning()
        {
            Entity gameDataEntity = EntityManager.CreateEntityQuery(
                typeof(GameDataSingletonComponent)).GetSingletonEntity();
            _gameData = EntityManager.GetComponentData<GameDataSingletonComponent>(gameDataEntity);

            _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            int ppu = _gameData.ppu;
            EntityCommandBuffer commandBuffer = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer();
            EntityCommandBuffer.ParallelWriter parallelWriter = commandBuffer.AsParallelWriter();

            Entities.ForEach((
                Entity entity,
                ref QuadRendererComponent quadRenderer,
                in RenderMesh renderMesh,
                in QuadRendererRequiresTextureDimensionsUpdateTag quadRendererRequiresTextureDimensionsUpdateTag) =>
            {
                Texture mainTex = renderMesh.material.mainTexture;
                quadRenderer.textureDimensions = new int2(mainTex.width, mainTex.height);
                
                commandBuffer.RemoveComponent<QuadRendererRequiresTextureDimensionsUpdateTag>(entity);
                commandBuffer.AddComponent<QuadRendererRequiresRescaleTag>(entity);
            }).WithoutBurst().Run();

            Entities.ForEach((
                int entityInQueryIndex,
                Entity entity,
                ref NonUniformScale scale,
                in QuadRendererComponent quadRenderer,
                in QuadRendererRequiresRescaleTag quadRendererRequiresRescaleTag) =>
            {
                float3 newScale = new float3
                {
                    x = quadRenderer.textureDimensions.x / (float) ppu,
                    y = quadRenderer.textureDimensions.y / (float) ppu,
                    z = 1f
                };

                scale.Value = newScale;
                
                parallelWriter.RemoveComponent<QuadRendererRequiresRescaleTag>(entityInQueryIndex, entity);
            }).ScheduleParallel();
            
            _endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}