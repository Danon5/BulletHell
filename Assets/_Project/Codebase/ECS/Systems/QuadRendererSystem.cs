using BulletHell.ECS.Components;
using BulletHell.ECS.Components.Singletons;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace BulletHell.ECS.Systems
{
    [DisableAutoCreation]
    public partial class QuadRendererSystem : SystemBase
    {
        private GameDataSingletonComponent _gameData;
        
        protected override void OnCreate()
        {
            Entity gameDataEntity = EntityManager.CreateEntityQuery(
                typeof(GameDataSingletonComponent)).GetSingletonEntity();
            _gameData = EntityManager.GetComponentData<GameDataSingletonComponent>(gameDataEntity);
        }

        protected override void OnUpdate()
        {
            int ppu = _gameData.ppu;

            Entities.ForEach((
                Entity entity,
                ref LocalToWorld localToWorld,
                in QuadRendererComponent quadRenderer) =>
            {
                float3 newScale = new float3
                {
                    x = quadRenderer.textureDimensions.x / (float) ppu,
                    y = quadRenderer.textureDimensions.y / (float) ppu,
                    z = 1f
                };

                localToWorld.Value = float4x4.TRS(
                    localToWorld.Position,
                    localToWorld.Rotation,
                    newScale);
            }).Run();
        }
    }
}