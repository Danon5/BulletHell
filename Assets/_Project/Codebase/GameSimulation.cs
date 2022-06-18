using BulletHell.ECS.Components.Singletons;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace BulletHell
{
    public sealed class GameSimulation : MonoBehaviour
    {
        private EntityManager _entityManager;
        
        private void Start()
        {
            Application.targetFrameRate = GameConstants.TARGET_FRAMERATE;
            QualitySettings.vSyncCount = 0;
            GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
            GraphicsSettings.transparencySortAxis = Vector3.up;

            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            Entity gameDataEntity = _entityManager.CreateEntityQuery(
                typeof(GameDataSingletonComponent)).GetSingletonEntity();

            GameDataSingletonComponent gameData = _entityManager.GetComponentData<GameDataSingletonComponent>(gameDataEntity);  

            _entityManager.Instantiate(gameData.playerPrefab);

            for (int i = 0; i < 400; i++)
            {
                Entity enemy = _entityManager.Instantiate(gameData.enemyPrefab);
                _entityManager.SetComponentData(enemy, new Translation
                {
                    Value = new float3
                    {
                        x = Random.Range(-50f, 50f),
                        y = Random.Range(-50f, 50f)
                    }
                });
            }
        }
    }
}